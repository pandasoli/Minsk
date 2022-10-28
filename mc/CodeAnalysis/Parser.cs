
namespace Minsk.CodeAnalysis
{
  internal sealed class Parser
  {
    private List<string> _diags = new List<string>();
    private readonly SyntaxToken[] _tokens;
    private int _pos;

    public Parser(string text) {
      var tokens = new List<SyntaxToken>();
      var lex = new Lexer(text);
      SyntaxToken token;

      do {
        token = lex.Lex();

        if (
          token.Kind != SyntaxKind.WhiteSpaceTk &&
          token.Kind != SyntaxKind.BadTokenTk
        )
          tokens.Add(token);
      }
      while (token.Kind != SyntaxKind.EOFTk);

      _tokens = tokens.ToArray();
      _diags.AddRange(lex.Diags);
    }

    public IEnumerable<string> Diags => _diags;

    private SyntaxToken Peek(int offset = 0) {
      var idx = _pos + offset;

      if (idx >= _tokens.Length)
        return _tokens[_tokens.Length - 1];

      return _tokens[idx];
    }

    private SyntaxToken Current => Peek();

    private SyntaxToken Next() {
      var current = Current;
      _pos++;
      return current;
    }

    private SyntaxToken Match(SyntaxKind kind) {
      if (Current.Kind == kind)
        return Next();

      _diags.Add($"🎰️ Parser: Unexpected token <{Current.Kind}>, expected <{kind}>.");
      return new SyntaxToken(kind, Current.Pos, null, null);
    }

    public SyntaxTree Parse() {
      var expr = ParseExpr();
      var eOF = Match(SyntaxKind.EOFTk);

      return new SyntaxTree(_diags, expr, eOF);
    }

    private ExprSyntax ParseExpr(int parentPrece = 0) {
      ExprSyntax left;
      var unaryOpPrece = Current.Kind.GetUnaryOpPrece();

      if (unaryOpPrece != 0 && unaryOpPrece >= parentPrece) {
        var op = Next();
        var operand = ParseExpr(unaryOpPrece);
        left = new UnaryExpr(op, operand);
      }
      else {
        left = ParsePrimExpr();
      }

      while (true) {
        var prece = Current.Kind.GetBinaryOpPrece();
        if (prece == 0 || prece <= parentPrece)
          break;

        var op = Next();
        var right = ParseExpr(prece);
        left = new BinaryExpr(left, op, right);
      }

      return left;
    }

    private ExprSyntax ParsePrimExpr() {
      if (Current.Kind == SyntaxKind.OpenParenTk) {
        var left = Next();
        var expr = ParseExpr();
        var right = Match(SyntaxKind.CloseParenTk);

        return new ParenExprNode(left, expr, right);
      }

      var num = Match(SyntaxKind.NumberTk);
      return new LitExpr(num);
    }

  }

}


namespace Minsk.CodeAnalysis.Syntax
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
          token.Kind != SyntaxKind.WhiteSpace &&
          token.Kind != SyntaxKind.BadToken
        )
          tokens.Add(token);
      }
      while (token.Kind != SyntaxKind.EOF);

      _tokens = tokens.ToArray();
      _diags.AddRange(lex.Diags);
    }

    public IEnumerable<string> Diags => _diags;

    public SyntaxTree Parse() {
      var expr = ParseExpr();
      var eOF = Match(SyntaxKind.EOF);

      return new SyntaxTree(_diags, expr, eOF);
    }

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

    private ExpressionNode ParseExpr(int parentPrec = 0) {
      ExpressionNode left;
      var unaryOpPrec = Current.Kind.GetUnaryOpPreced();

      if (unaryOpPrec != 0 && unaryOpPrec >= parentPrec) {
        var op = Next();
        var operand = ParseExpr(unaryOpPrec);

        left = new UnaryExprSyntax(op, operand);
      }
      else {
        left = ParsePrim();
      }

      while (true) {
        var preced = Current.Kind.GetBinaryOpPrec();

        if (preced == 0 || preced <= parentPrec)
          break;

        var op = Next();
        var right = ParseExpr(preced);

        left = new BinaryExprSyntax(left, op, right);
      }

      return left;
    }

    private ExpressionNode ParsePrim() {
      if (Current.Kind == SyntaxKind.OpenParen) {
        // var left = Next();
        var expr = ParseExpr();
        /* var right =  */ Match(SyntaxKind.CloseParen);

        return expr /* new ParenExprNode(left, expr, right) */;
      }

      if (
        Current.Kind == SyntaxKind.TrueKeyword ||
        Current.Kind == SyntaxKind.FalseKeyword
      ) {
        var token = Next();
        var val = Current.Kind == SyntaxKind.TrueKeyword;
        return new LiteralExprSyntax(token, val);
      }

      var num = Match(SyntaxKind.Number);
      return new LiteralExprSyntax(num);
    }

  }

}

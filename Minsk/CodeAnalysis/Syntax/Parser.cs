
namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Parser
  {
    private DiagBag _diags = new DiagBag();
    private readonly SyntaxToken[] _tokens;
    private int _pos;

    public DiagBag Diags => _diags;

    public Parser(string text) {
      var tokens = new List<SyntaxToken>();
      var lex = new Lexer(text);
      SyntaxToken token;

      do {
        token = lex.Lex();

        if (
          token.Kind != SyntaxKind.WhiteSpaceTk &&
          token.Kind != SyntaxKind.BadTk
        )
          tokens.Add(token);
      }
      while (token.Kind != SyntaxKind.EOFTk);

      _tokens = tokens.ToArray();
      _diags.AddRange(lex.Diags);
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

      _diags.ReportUnexpctTk(Current.Span, Current.Kind, kind);
      return new SyntaxToken(kind, Current.Pos, null, null);
    }

    public SyntaxTree Parse() {
      var expr = ParseExpr();
      var eOF = Match(SyntaxKind.EOFTk);

      return new SyntaxTree(_diags, expr, eOF);
    }

    private ExprSyntax ParseExpr() {
      return ParseAssignExpr();
    }

    private ExprSyntax ParseAssignExpr() {
      if (
        Peek(0).Kind == SyntaxKind.IdTk &&
        Peek(1).Kind == SyntaxKind.EqsTk
      ) {
        var id = Next();
        var op = Next();
        var right = ParseAssignExpr();
        return new AssignmExpr(id, op, right);
      }

      return ParseBinaryExpr();
    }

    private ExprSyntax ParseBinaryExpr(int parentPrece = 0) {
      ExprSyntax left;
      var unaryOpPrece = Current.Kind.GetUnaryOpPrece();

      if (unaryOpPrece != 0 && unaryOpPrece >= parentPrece) {
        var op = Next();
        var operand = ParseBinaryExpr(unaryOpPrece);
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
        var right = ParseBinaryExpr(prece);
        left = new BinaryExpr(left, op, right);
      }

      return left;
    }

    private ExprSyntax ParsePrimExpr() {
      switch (Current.Kind) {
        case SyntaxKind.OpenParenTk:
          {
            var left = Next();
            var expr = ParseExpr();
            var right = Match(SyntaxKind.CloseParenTk);

            return new ParenExpr(left, expr, right);
          }

        case SyntaxKind.TrueKw:
        case SyntaxKind.FalseKw:
          {
            var token = Next();
            var val = token.Kind == SyntaxKind.TrueKw;
            return new LitExpr(token, val);
          }

        case SyntaxKind.IdTk:
          {
            var id = Next();
            return new NameExpr(id);
          }

        default:
          {
            var num = Match(SyntaxKind.NumberTk);
            return new LitExpr(num);
          }
      }
    }

  }

}

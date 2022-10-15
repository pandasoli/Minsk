
namespace Minsk.CodeAnalysis
{
  class Parser
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
      var eOF = Match(SyntaxKind.EOF);

      return new SyntaxTree(_diags, expr, eOF);
    }

    private ExpressionNode ParseExpr() {
      var left = ParseTerm();

      while (
        Current.Kind == SyntaxKind.Plus ||
        Current.Kind == SyntaxKind.Dash
      ) {
        var op = Next();
        var right = ParseTerm();

        left = new BinaryNode(left, op, right);
      }

      return left;
    }

    private ExpressionNode ParseTerm() {
      var left = ParseFactor();

      while (
        Current.Kind == SyntaxKind.Star ||
        Current.Kind == SyntaxKind.Slash
      ) {
        var op = Next();
        var right = ParseFactor();

        left = new BinaryNode(left, op, right);
      }

      return left;
    }

    private ExpressionNode ParseFactor() {
      if (Current.Kind == SyntaxKind.OpenParen) {
        var left = Next();
        var expr = ParseExpr();
        var right = Match(SyntaxKind.CloseParen);

        return new ParenExprNode(left, expr, right);
      }

      var num = Match(SyntaxKind.Number);
      return new NumberNode(num);
    }

  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class SyntaxTree {
    public SyntaxTree(IEnumerable<Diag> diags, ExprSyntax root, SyntaxToken eOF) {
      Diags = diags.ToArray();
      Root = root;
      EOF = eOF;
    }

    public IReadOnlyList<Diag> Diags { get; }
    public ExprSyntax Root { get; }
    public SyntaxToken EOF { get; }

    public static SyntaxTree Parse(string text) {
      var par = new Parser(text);
      return par.Parse();
    }

    public static IEnumerable<SyntaxToken> ParseTokens(string text) {
      var lex = new Lexer(text);

      while (true) {
        var token = lex.Lex();

        if (token.Kind == SyntaxKind.EOFTk)
          break;

        yield return token;
      }
    }
  }

}

using System.Collections.Immutable;
using Minsk.CodeAnalysis.Text;

namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class SyntaxTree {
    public SyntaxTree(SourceText text, ImmutableArray<Diag> diags, ExprSyntax root, SyntaxToken eOF) {
      Text = text;
      Diags = diags;
      Root = root;
      EOF = eOF;
    }

    public SourceText Text { get; }
    public ImmutableArray<Diag> Diags { get; }
    public ExprSyntax Root { get; }
    public SyntaxToken EOF { get; }

    public static SyntaxTree Parse(string text) => Parse(SourceText.From(text));
    public static SyntaxTree Parse(SourceText text) {
      var par = new Parser(text);
      return par.Parse();
    }

    public static IEnumerable<SyntaxToken> ParseTokens(string text) => ParseTokens(SourceText.From(text));
    public static IEnumerable<SyntaxToken> ParseTokens(SourceText text) {
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

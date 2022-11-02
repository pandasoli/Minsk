
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
  }

}

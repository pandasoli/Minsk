
namespace Minsk.CodeAnalysis
{
  public sealed class SyntaxTree {
    public SyntaxTree(IEnumerable<string> diags, ExprSyntax root, SyntaxToken eOF) {
      Diags = diags.ToArray();
      Root = root;
      EOF = eOF;
    }

    public IReadOnlyList<string> Diags { get; }
    public ExprSyntax Root { get; }
    public SyntaxToken EOF { get; }

    public static SyntaxTree Parse(string text) {
      var par = new Parser(text);
      return par.Parse();
    }
  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class SyntaxTree {
    public SyntaxTree(IEnumerable<Diagnostic> diags, ExpressionNode root, SyntaxToken eOF) {
      Diags = diags.ToArray();
      Root = root;
      EOF = eOF;
    }

    public IReadOnlyList<Diagnostic> Diags { get; }
    public ExpressionNode Root { get; }
    public SyntaxToken EOF { get; }

    public static SyntaxTree Parse(string text) {
      var par = new Parser(text);
      return par.Parse();
    }
  }

}
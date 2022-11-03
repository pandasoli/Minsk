
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class NameExpr : ExprSyntax {
    public NameExpr(SyntaxToken id) {
      Id = id;
    }

    public SyntaxToken Id { get; }

    public override SyntaxKind Kind => SyntaxKind.NameExpr;
    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Id;
    }
  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  public abstract class ExprSyntax {
    public abstract SyntaxKind Kind { get; }

    public abstract IEnumerable<ExprSyntax> GetChildren();
  }

}

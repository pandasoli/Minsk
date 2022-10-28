
namespace Minsk.CodeAnalysis
{
  public sealed class BinaryExpr : ExprSyntax {
    public BinaryExpr(ExprSyntax left, SyntaxToken op, ExprSyntax right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public ExprSyntax Left { get; }
    public SyntaxToken Op { get; }
    public ExprSyntax Right { get; }

    public override SyntaxKind Kind => SyntaxKind.BinaryNode;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Left;
      yield return Op;
      yield return Right;
    }
  }

}

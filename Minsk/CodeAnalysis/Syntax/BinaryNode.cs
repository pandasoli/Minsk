
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class BinaryExprSyntax : ExpressionNode {
    public BinaryExprSyntax(ExpressionNode left, SyntaxToken op, ExpressionNode right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public ExpressionNode Left { get; }
    public SyntaxToken Op { get; }
    public ExpressionNode Right { get; }

    public override SyntaxKind Kind => SyntaxKind.BinaryExpr;

    public override IEnumerable<ExprSyntax> GetChildren() {
      yield return Left;
      yield return Op;
      yield return Right;
    }
  }

}

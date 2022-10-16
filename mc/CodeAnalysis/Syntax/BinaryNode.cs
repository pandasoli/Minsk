
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class BinaryNode : ExpressionNode {
    public BinaryNode(ExpressionNode left, SyntaxToken op, ExpressionNode right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public ExpressionNode Left { get; }
    public SyntaxToken Op { get; }
    public ExpressionNode Right { get; }

    public override SyntaxKind Kind => SyntaxKind.BinaryNode;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Left;
      yield return Op;
      yield return Right;
    }
  }

}

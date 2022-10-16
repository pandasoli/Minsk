
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class UnaryNode : ExpressionNode {
    public UnaryNode(SyntaxToken op, ExpressionNode operand) {
      Op = op;
      Operand = operand;
    }

    public SyntaxToken Op { get; }
    public ExpressionNode Operand { get; }

    public override SyntaxKind Kind => SyntaxKind.UnaryNode;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Op;
      yield return Operand;
    }
  }

}

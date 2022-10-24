
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class UnaryExprSyntax : ExpressionNode {
    public UnaryExprSyntax(SyntaxToken op, ExpressionNode operand) {
      Op = op;
      Operand = operand;
    }

    public SyntaxToken Op { get; }
    public ExpressionNode Operand { get; }

    public override SyntaxKind Kind => SyntaxKind.UnaryExpr;

    public override IEnumerable<ExprSyntax> GetChildren() {
      yield return Op;
      yield return Operand;
    }
  }

}

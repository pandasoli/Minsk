
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class UnaryExprSyntax : ExprSyntax {
    public UnaryExprSyntax(SyntaxToken op, ExprSyntax operand) {
      Op = op;
      Operand = operand;
    }

    public SyntaxToken Op { get; }
    public ExprSyntax Operand { get; }

    public override SyntaxKind Kind => SyntaxKind.UnaryExpr;

    public override IEnumerable<ExprSyntax> GetChildren() {
      yield return Op;
      yield return Operand;
    }
  }

}

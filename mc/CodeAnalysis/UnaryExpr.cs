
namespace Minsk.CodeAnalysis
{
  public sealed class UnaryExpr : ExprSyntax {
    public UnaryExpr(SyntaxToken op, ExprSyntax operand) {
      Op = op;
      Operand = operand;
    }

    public SyntaxToken Op { get; }
    public ExprSyntax Operand { get; }

    public override SyntaxKind Kind => SyntaxKind.UnaryExpr;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Op;
      yield return Operand;
    }
  }

}

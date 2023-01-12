
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class UnaryExpr : ExprSyntax {
    public UnaryExpr(SyntaxToken op, ExprSyntax operand) {
      Op = op;
      Operand = operand;
    }

    public SyntaxToken Op { get; }
    public ExprSyntax Operand { get; }

    public override SyntaxKind Kind => SyntaxKind.UnaryExpr;
  }

}

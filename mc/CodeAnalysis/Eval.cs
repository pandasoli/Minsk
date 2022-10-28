
namespace Minsk.CodeAnalysis
{
  public sealed class Eval
  {
    private readonly ExprSyntax _root;

    public Eval(ExprSyntax root) {
      _root = root;
    }

    public int Evaluate() {
      return EvalExpr(_root);
    }

    private int EvalExpr(ExprSyntax node) {
      if (node is LitExpr num)
        return num.Token.Val != null ? (int) num.Token.Val : 0;

      if (node is UnaryExpr unary) {
        var operand = EvalExpr(unary.Operand);

        if (unary.Op.Kind == SyntaxKind.PlusTk) return operand;
        if (unary.Op.Kind == SyntaxKind.DashTk) return -operand;

        throw new Exception($"Unexpected unary operator {unary.Op.Kind}.");
      }

      if (node is BinaryExpr bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case SyntaxKind.PlusTk: return left + right;
          case SyntaxKind.DashTk: return left - right;
          case SyntaxKind.StarTk: return left * right;
          case SyntaxKind.SlashTk: return left / right;
          default:
            throw new Exception($"Unexpected binary operator {bin.Op.Kind}.");
        }
      }

      if (node is ParenExprNode par)
        return EvalExpr(par.Expr);

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }
}

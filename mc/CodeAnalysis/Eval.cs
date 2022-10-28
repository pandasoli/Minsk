
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

      if (node is BinaryExpr bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case SyntaxKind.Plus: return left + right;
          case SyntaxKind.Dash: return left - right;
          case SyntaxKind.Star: return left * right;
          case SyntaxKind.Slash: return left / right;
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
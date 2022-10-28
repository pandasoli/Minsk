using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;


namespace Minsk.CodeAnalysis
{
  internal sealed class Eval
  {
    private readonly BoundExpr _root;

    public Eval(BoundExpr root) {
      _root = root;
    }

    public int Evaluate() {
      return EvalExpr(_root);
    }

    private int EvalExpr(BoundExpr node) {
      if (node is BoundLitExpr num)
        return (int) num.Val;

      if (node is BoundUnaryExpr unary) {
        var operand = EvalExpr(unary.Operand);

        if (unary.Op == BoundUnaryOpKind.Identity) return operand;
        if (unary.Op == BoundUnaryOpKind.Negation) return -operand;

        throw new Exception($"Unexpected unary operator {unary.Op}.");
      }

      if (node is BoundBinaryExpr bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op) {
          case BoundBinaryOpKind.Add: return left + right;
          case BoundBinaryOpKind.Sub: return left - right;
          case BoundBinaryOpKind.Mul: return left * right;
          case BoundBinaryOpKind.Div: return left / right;

          default:
            throw new Exception($"Unexpected binary operator {bin.Op}.");
        }
      }

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }
}

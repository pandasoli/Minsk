using Minsk.CodeAnalysis.Syntax;
using Minsk.CodeAnalysis.Binding;

namespace Minsk.CodeAnalysis
{
  internal sealed class Evaluator
  {
    private readonly BoundExpr _root;

    public Evaluator(BoundExpr root) {
      _root = root;
    }

    public object Evaluate() {
      return EvalExpr(_root);
    }

    private object EvalExpr(BoundExpr node) {
      if (node is BoundLiteralExpr num)
        return num.Val;

      if (node is BoundBinaryExpr bin) {
        var left = (int) EvalExpr(bin.Left);
        var right = (int) EvalExpr(bin.Right);

        switch (bin.Op) {
          case BoundBinaryOpKind.Add: return left + right;
          case BoundBinaryOpKind.Sub: return left - right;
          case BoundBinaryOpKind.Mul: return left * right;
          case BoundBinaryOpKind.Div: return left / right;

          default:
            throw new Exception($"Unexpected binary operator {bin.Op}.");
        }
      }

      if (node is BoundUnaryExpr unary) {
        var operand = (int) EvalExpr(unary.Operand);

        if (unary.Op == BoundUnaryOpKind.Identity) return operand;
        if (unary.Op == BoundUnaryOpKind.Negation) return -operand;

        throw new Exception($"Unexpected unary operator {unary.Op}.");
      }

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }

}

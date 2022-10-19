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
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case BoundBinaryOpKind.Add: return (int) left + (int) right;
          case BoundBinaryOpKind.Sub: return (int) left - (int) right;
          case BoundBinaryOpKind.Mul: return (int) left * (int) right;
          case BoundBinaryOpKind.Div: return (int) left / (int) right;

          case BoundBinaryOpKind.LogicalAnd: return (bool) left && (bool) right;
          case BoundBinaryOpKind.LogicalOr:  return (bool) left || (bool) right;

          default:
            throw new Exception($"🛰️ Evaluator: unexpected binary operator {bin.Op}.");
        }
      }

      if (node is BoundUnaryExpr unary) {
        var operand = EvalExpr(unary.Operand);

        switch (unary.Op.Kind) {
          case BoundUnaryOpKind.Identity: return (int) operand;
          case BoundUnaryOpKind.Negation: return -(int) operand;
          case BoundUnaryOpKind.LogicalNegation: return !(bool) operand;

          default:
            throw new Exception($"🛰️ Evaluator: unexpected unary operator {unary.Op}.");
        }
      }

      throw new Exception($"🛰️ Evaluator: unexpected node {node.Kind}.");
    }
  }

}

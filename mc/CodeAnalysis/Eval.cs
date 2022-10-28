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

    public object Evaluate() {
      return EvalExpr(_root);
    }

    private object EvalExpr(BoundExpr node) {
      if (node is BoundLitExpr num)
        return num.Val;

      if (node is BoundUnaryExpr unary) {
        var operand = EvalExpr(unary.Operand);

        if (unary.Op == BoundUnaryOpKind.Identity) return  (int) operand;
        if (unary.Op == BoundUnaryOpKind.Neg)      return -(int) operand;
        if (unary.Op == BoundUnaryOpKind.LgcNeg)   return !(bool) operand;

        throw new Exception($"Unexpected unary operator {unary.Op}.");
      }

      if (node is BoundBinaryExpr bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op) {
          case BoundBinaryOpKind.Add: return (int) left + (int) right;
          case BoundBinaryOpKind.Sub: return (int) left - (int) right;
          case BoundBinaryOpKind.Mul: return (int) left * (int) right;
          case BoundBinaryOpKind.Div: return (int) left / (int) right;

          case BoundBinaryOpKind.LgcAnd: return (bool) left && (bool) right;
          case BoundBinaryOpKind.LgcOr:  return (bool) left || (bool) right;

          default:
            throw new Exception($"Unexpected binary operator {bin.Op}.");
        }
      }

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }
}

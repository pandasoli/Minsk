using Minsk.CodeAnalysis.Binding;

namespace Minsk.CodeAnalysis
{
  internal sealed class Eval
  {
    private readonly BoundExpr _root;
    private readonly Dictionary<VarSymbol, object?> _vars;

    public Eval(BoundExpr root, Dictionary<VarSymbol, object?> vars) {
      _root = root;
      _vars = vars;
    }

    public object? Evaluate() {
      return EvalExpr(_root);
    }

    private object? EvalExpr(BoundExpr node) {
      if (node is BoundLitExpr num)
        return num.Val;

      if (node is BoundVarExpr var)
        return _vars[var.Var];

      if (node is BoundAssignmsExpr ass) {
        var val = EvalExpr(ass.Expr);
        _vars[ass.Var] = val;
        return val;
      }

      if (node is BoundUnaryExpr unary) {
        var operand = EvalExpr(unary.Operand);

        if (unary.Op.Kind == BoundUnaryOpKind.Identity) return  (int)  (operand ?? 0);
        if (unary.Op.Kind == BoundUnaryOpKind.Neg)      return -(int)  (operand ?? 0);
        if (unary.Op.Kind == BoundUnaryOpKind.LgcNeg)   return !(bool) (operand ?? true);

        throw new Exception($"Unexpected unary operator {unary.Op}.");
      }

      if (node is BoundBinaryExpr bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case BoundBinaryOpKind.Add: return (int) (left ?? 0) + (int) (right ?? 0);
          case BoundBinaryOpKind.Sub: return (int) (left ?? 0) - (int) (right ?? 0);
          case BoundBinaryOpKind.Mul: return (int) (left ?? 0) * (int) (right ?? 0);
          case BoundBinaryOpKind.Div: return (int) (left ?? 0) / (int) (right ?? 0);

          case BoundBinaryOpKind.LgcAnd: return (bool) (left ?? false) && (bool) (right ?? false);
          case BoundBinaryOpKind.LgcOr:  return (bool) (left ?? false) || (bool) (right ?? false);

          case BoundBinaryOpKind.Eqs:    return  Equals(left, right);
          case BoundBinaryOpKind.NotEqs: return !Equals(left, right);

          default:
            throw new Exception($"Unexpected binary operator {bin.Op}.");
        }
      }

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }
}

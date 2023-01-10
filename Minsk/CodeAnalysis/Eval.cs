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
      switch (node.Kind)
      {
        case BoundNodeKind.LitExpr:      return EvalLitExpr((BoundLitExpr)          node);
        case BoundNodeKind.VarExpr:      return EvalVarExpr((BoundVarExpr)          node);
        case BoundNodeKind.AssignmsExpr: return EvalAssignmExpr((BoundAssignmsExpr) node);
        case BoundNodeKind.UnaryExpr:    return EvalUnaryExpr((BoundUnaryExpr)      node);
        case BoundNodeKind.BinaryExpr:      return EvalBinExpr((BoundBinExpr)       node);

        default:
          throw new Exception($"Unexpected node {node.Kind}.");
      }
    }

    private object EvalLitExpr(BoundLitExpr num) {
      return num.Val;
    }

    private object? EvalVarExpr(BoundVarExpr var) {
      return _vars[var.Var];
    }

    private object? EvalAssignmExpr(BoundAssignmsExpr ass) {
      var val = EvalExpr(ass.Expr);
      _vars[ass.Var] = val;
      return val;
    }

    private object EvalUnaryExpr(BoundUnaryExpr unary) {
      var operand = EvalExpr(unary.Operand);

      if (unary.Op.Kind == BoundUnaryOpKind.Identity) return  (int)  (operand ?? 0);
      if (unary.Op.Kind == BoundUnaryOpKind.Neg)      return -(int)  (operand ?? 0);
      if (unary.Op.Kind == BoundUnaryOpKind.LgcNeg)   return !(bool) (operand ?? true);

      throw new Exception($"Unexpected unary operator {unary.Op}.");
    }

    private object EvalBinExpr(BoundBinExpr bin) {
      var left = EvalExpr(bin.Left);
      var right = EvalExpr(bin.Right);

      switch (bin.Op.Kind) {
        case BoundBinOpKind.Add: return (int) (left ?? 0) + (int) (right ?? 0);
        case BoundBinOpKind.Sub: return (int) (left ?? 0) - (int) (right ?? 0);
        case BoundBinOpKind.Mul: return (int) (left ?? 0) * (int) (right ?? 0);
        case BoundBinOpKind.Div: return (int) (left ?? 0) / (int) (right ?? 0);

        case BoundBinOpKind.LgcAnd: return (bool) (left ?? false) && (bool) (right ?? false);
        case BoundBinOpKind.LgcOr:  return (bool) (left ?? false) || (bool) (right ?? false);

        case BoundBinOpKind.Eqs:    return  Equals(left, right);
        case BoundBinOpKind.NotEqs: return !Equals(left, right);

        default:
          throw new Exception($"Unexpected binary operator {bin.Op}.");
      }
    }
  }
}

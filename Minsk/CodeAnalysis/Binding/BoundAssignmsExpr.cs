namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundAssignmsExpr : BoundExpr {
    public BoundAssignmsExpr(VarSymbol var, BoundExpr expr) {
      Var = var;
      Expr = expr;
    }

    public VarSymbol Var { get; }
    public BoundExpr Expr { get; }

    public override Type Type => Expr.Type;
    public override BoundNodeKind Kind => BoundNodeKind.AssignmsExpr;
  }

}

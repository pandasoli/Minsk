namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundVarExpr : BoundExpr {
    public BoundVarExpr(VarSymbol var) {
      Var = var;
    }

    public VarSymbol Var { get; }

    public override Type Type => Var.Type;
    public override BoundNodeKind Kind => BoundNodeKind.VarExpr;
  }

}

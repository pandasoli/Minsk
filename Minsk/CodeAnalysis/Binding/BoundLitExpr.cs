namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundLitExpr : BoundExpr
  {
    public BoundLitExpr(object val) {
      Val = val;
    }

    public object Val { get; }

    public override Type Type => Val.GetType();
    public override BoundNodeKind Kind => BoundNodeKind.LitExpr;
  }

}

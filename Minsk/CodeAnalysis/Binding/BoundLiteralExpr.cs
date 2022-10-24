namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundLiteralExpr : BoundExpr
  {
    public BoundLiteralExpr(object val) {
      Val = val;
    }

    public object Val { get; }

    public override Type Type => Val.GetType();
    public override BoundNodeKind Kind => BoundNodeKind.LiteralExpr;
  }

}

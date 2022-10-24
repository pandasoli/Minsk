namespace Minsk.CodeAnalysis.Binding
{
  internal abstract class BoundExpr : BoundNode
  {
    public abstract Type Type { get; }
  }

}

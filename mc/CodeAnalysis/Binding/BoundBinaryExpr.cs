namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinaryExpr : BoundExpr
  {
    public BoundBinaryExpr(BoundExpr left, BoundBinaryOpKind op, BoundExpr right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public BoundExpr Left { get; }
    public BoundBinaryOpKind Op { get; }
    public BoundExpr Right { get; }

    public override Type Type => Left.Type;
    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpr;
  }

}

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinaryExpr : BoundExpr
  {
    public BoundBinaryExpr(BoundExpr left, BoundBinaryOp op, BoundExpr right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public BoundExpr Left { get; }
    public BoundBinaryOp Op { get; }
    public BoundExpr Right { get; }

    public override Type Type => Op.Type;
    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpr;
  }

}

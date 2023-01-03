namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinExpr : BoundExpr
  {
    public BoundBinExpr(BoundExpr left, BoundBinOp op, BoundExpr right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public BoundExpr Left { get; }
    public BoundBinOp Op { get; }
    public BoundExpr Right { get; }

    public override Type Type => Op.Type;
    public override BoundNodeKind Kind => BoundNodeKind.BinaryExpr;
  }

}

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundUnaryExpr : BoundExpr
  {
    public BoundUnaryExpr(BoundUnaryOp op, BoundExpr operand) {
      Op = op;
      Operand = operand;
    }

    public BoundUnaryOp Op { get; }
    public BoundExpr Operand { get; }

    public override Type Type => Operand.Type;
    public override BoundNodeKind Kind => BoundNodeKind.UnaryExpr;
  }

}

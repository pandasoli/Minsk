
namespace Minsk.CodeAnalysis
{
  sealed class ParenExprNode : ExpressionNode
  {
    public ParenExprNode(SyntaxToken open, ExpressionNode expr, SyntaxToken close) {
      Open = open;
      Expr = expr;
      Close = close;
    }

    public override SyntaxKind Kind => SyntaxKind.ParenExpr;

    public SyntaxToken Open { get; }
    public ExpressionNode Expr { get; }
    public SyntaxToken Close { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
      yield return Open;
      yield return Expr;
      yield return Close;
    }
  }

}

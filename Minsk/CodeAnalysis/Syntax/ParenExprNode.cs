
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class ParenExprNode : ExprSyntax
  {
    public ParenExprNode(SyntaxToken open, ExprSyntax expr, SyntaxToken close) {
      Open = open;
      Expr = expr;
      Close = close;
    }

    public override SyntaxKind Kind => SyntaxKind.ParenExpr;

    public SyntaxToken Open { get; }
    public ExprSyntax Expr { get; }
    public SyntaxToken Close { get; }

    public override IEnumerable<ExprSyntax> GetChildren()
    {
      yield return Open;
      yield return Expr;
      yield return Close;
    }
  }

}

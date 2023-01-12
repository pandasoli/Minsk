
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class ParenExpr : ExprSyntax
  {
    public ParenExpr(SyntaxToken open, ExprSyntax expr, SyntaxToken close) {
      Open = open;
      Expr = expr;
      Close = close;
    }

    public override SyntaxKind Kind => SyntaxKind.ParenExpr;

    public SyntaxToken Open { get; }
    public ExprSyntax Expr { get; }
    public SyntaxToken Close { get; }
  }

}

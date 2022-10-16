
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class LiteralExprSyntax : ExpressionNode {
    public LiteralExprSyntax(SyntaxToken lit) {
      Token = lit;
    }

    public override SyntaxKind Kind => SyntaxKind.LiteralExpr;
    public SyntaxToken Token { get; }

    public override IEnumerable<ExprSyntax> GetChildren() {
      yield return Token;
    }
  }

}

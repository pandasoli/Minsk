
namespace Minsk.CodeAnalysis
{
  sealed class LitExpr : ExprSyntax {
    public LitExpr(SyntaxToken lit) {
      Token = lit;
    }

    public override SyntaxKind Kind => SyntaxKind.NumberExpr;
    public SyntaxToken Token { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

}

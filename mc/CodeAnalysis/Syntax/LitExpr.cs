
namespace Minsk.CodeAnalysis.Syntax
{
  sealed class LitExpr : ExprSyntax {
    public LitExpr(SyntaxToken lit) {
      Token = lit;
    }

    public override SyntaxKind Kind => SyntaxKind.LitExpr;
    public SyntaxToken Token { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

}

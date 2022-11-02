
namespace Minsk.CodeAnalysis.Syntax
{
  sealed class LitExpr : ExprSyntax {
    public LitExpr(SyntaxToken lit) : this(lit, lit.Val) {}

    public LitExpr(SyntaxToken lit, object? val) {
      Token = lit;
      Val = val;
    }

    public override SyntaxKind Kind => SyntaxKind.LitExpr;
    public SyntaxToken Token { get; }
    public object? Val { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class LiteralExprSyntax : ExpressionNode {
    public LiteralExprSyntax(SyntaxToken lit) : this(lit, lit.Val) {}
    public LiteralExprSyntax(SyntaxToken lit, object? val) {
      Token = lit;
      Val = val;
    }

    public override SyntaxKind Kind => SyntaxKind.LiteralExpr;
    public SyntaxToken Token { get; }
    public object? Val { get; }

    public override IEnumerable<ExprSyntax> GetChildren() {
      yield return Token;
    }
  }

}

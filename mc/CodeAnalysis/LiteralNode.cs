
namespace Minsk.CodeAnalysis
{
  public sealed class LiteralNode : ExpressionNode {
    public LiteralNode(SyntaxToken lit) {
      Token = lit;
    }

    public override SyntaxKind Kind => SyntaxKind.LiteralNode;
    public SyntaxToken Token { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

}

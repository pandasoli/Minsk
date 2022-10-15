
namespace Minsk.CodeAnalysis
{
  sealed class NumberNode : ExpressionNode {
    public NumberNode(SyntaxToken num) {
      Token = num;
    }

    public override SyntaxKind Kind => SyntaxKind.NumberNode;
    public SyntaxToken Token { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

}

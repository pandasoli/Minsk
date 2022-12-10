
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class BinExpr : ExprSyntax {
    public BinExpr(ExprSyntax left, SyntaxToken op, ExprSyntax right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public ExprSyntax Left { get; }
    public SyntaxToken Op { get; }
    public ExprSyntax Right { get; }

    public override SyntaxKind Kind => SyntaxKind.BinExpr;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Left;
      yield return Op;
      yield return Right;
    }
  }

}

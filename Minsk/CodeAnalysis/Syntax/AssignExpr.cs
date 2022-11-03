
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class AssignmExpr : ExprSyntax {
    public AssignmExpr(SyntaxToken id, SyntaxToken eqs, ExprSyntax expr) {
      Id = id;
      Eqs = eqs;
      Expr = expr;
    }

    public SyntaxToken Id { get; }
    public SyntaxToken Eqs { get; }
    public ExprSyntax Expr { get; }

    public override SyntaxKind Kind => SyntaxKind.AssignExpr;
    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Id;
      yield return Eqs;
      yield return Expr;
    }
  }

}

using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{
  public sealed class Evaluator
  {
    private readonly ExpressionNode _root;

    public Evaluator(ExpressionNode root) {
      _root = root;
    }

    public int Evaluate() {
      return EvalExpr(_root);
    }

    private int EvalExpr(ExpressionNode node) {
      if (node is LiteralNode num)
        return num.Token.Val != null ? (int) num.Token.Val : 0;

      if (node is BinaryNode bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case SyntaxKind.Plus: return left + right;
          case SyntaxKind.Dash: return left - right;
          case SyntaxKind.Star: return left * right;
          case SyntaxKind.Slash: return left / right;

          default:
            throw new Exception($"Unexpected binary operator {bin.Op.Kind}.");
        }
      }

      if (node is UnaryNode unary) {
        var operand = EvalExpr(unary.Operand);

        if (unary.Op.Kind == SyntaxKind.Plus) return operand;
        if (unary.Op.Kind == SyntaxKind.Dash) return -operand;

        throw new Exception($"Unexpected unary operator {unary.Op.Kind}.");
      }

      if (node is ParenExprNode par)
        return EvalExpr(par.Expr);

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }

}

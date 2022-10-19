
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class Binder
  {
    private readonly List<string> _diags = new List<string>();

    public BoundExpr BindExpr(ExprSyntax syntax) {
      switch (syntax.Kind) {
        case SyntaxKind.LiteralExpr: return BindLiteralExpr((LiteralExprSyntax) syntax);
        case SyntaxKind.UnaryExpr:   return BindUnaryExpr((UnaryExprSyntax) syntax);
        case SyntaxKind.BinaryExpr:  return BindBinaryExpr((BinaryExprSyntax) syntax);

        default:
          throw new Exception($"🌎️ Binder: Unexpected syntax {syntax.Kind}.");
      }
    }

    public IEnumerable<string> Diags => _diags;

    private BoundExpr BindLiteralExpr(LiteralExprSyntax syntax) {
      var val = syntax.Val ?? 0;
      return new BoundLiteralExpr(val);
    }

    private BoundExpr BindUnaryExpr(UnaryExprSyntax syntax) {
      var boundOperand = BindExpr(syntax.Operand);
      var boundOp = BoundUnaryOp.Bind(syntax.Op.Kind, boundOperand.Type);

      if (boundOp == null) {
        _diags.Add($"🌎️ Binder: Unary operator '{syntax.Op.Text}' is not defined for type {boundOperand.Type}.");
        return boundOperand;
      }

      return new BoundUnaryExpr(boundOp, boundOperand);
    }

    private BoundExpr BindBinaryExpr(BinaryExprSyntax syntax) {
      var boundLeft = BindExpr(syntax.Left);
      var boundRight = BindExpr(syntax.Right);
      var boundOp = BoundBinaryOp.Bind(syntax.Op.Kind, boundLeft.Type, boundRight.Type);

      if (boundOp == null) {
        _diags.Add($"🌎️ Binder: Unary operator '{syntax.Op.Text}' is not defined for type '{boundLeft.Type}' and '{boundRight.Type}.");
        return boundLeft;
      }

      return new BoundBinaryExpr(boundLeft, boundOp, boundRight);
    }

  }

}

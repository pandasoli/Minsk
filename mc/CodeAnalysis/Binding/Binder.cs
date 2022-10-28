
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class Binder
  {
    private readonly List<string> _diags = new List<string>();

    public BoundExpr BindExpr(ExprSyntax syntax) {
      switch (syntax.Kind) {
        case SyntaxKind.BinaryExpr: return BindBinaryExpr((BinaryExpr) syntax);
        case SyntaxKind.UnaryExpr:  return BindUnaryExpr((UnaryExpr) syntax);
        case SyntaxKind.LitExpr:    return BindLitExpr((LitExpr) syntax);

        default:
          throw new Exception($"Unexpected syntax {syntax.Kind}.");
      }
    }

    public IEnumerable<string> Diags => _diags;

    private BoundExpr BindLitExpr(LitExpr syntax) {
      var val = syntax.Val ?? 0;
      return new BoundLitExpr(val);
    }

    private BoundExpr BindUnaryExpr(UnaryExpr syntax) {
      var boundOperand = BindExpr(syntax.Operand);
      var boundOp = BoundUnaryOp.Bind(syntax.Op.Kind, boundOperand.Type);

      if (boundOp == null) {
        _diags.Add($"Unary operator '{syntax.Op.Text}' is not defined for type {boundOperand.Type}.");
        return boundOperand;
      }

      return new BoundUnaryExpr(boundOp, boundOperand);
    }

    private BoundExpr BindBinaryExpr(BinaryExpr syntax) {
      var boundLeft = BindExpr(syntax.Left);
      var boundRight = BindExpr(syntax.Right);
      var boundOp = BoundBinaryOp.Bind(syntax.Op.Kind, boundLeft.Type, boundRight.Type);

      if (boundOp == null) {
        _diags.Add($"Binary operator '{syntax.Op.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
        return boundLeft;
      }

      return new BoundBinaryExpr(boundLeft, boundOp, boundRight);
    }
  }

}

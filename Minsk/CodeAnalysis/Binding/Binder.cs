
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class Binder
  {
    private readonly DiagBag _diags = new DiagBag();

    public BoundExpr BindExpr(ExprSyntax syntax) {
      switch (syntax.Kind) {
        case SyntaxKind.BinaryExpr: return BindBinaryExpr((BinaryExpr) syntax);
        case SyntaxKind.UnaryExpr:  return BindUnaryExpr((UnaryExpr) syntax);
        case SyntaxKind.LitExpr:    return BindLitExpr((LitExpr) syntax);
        case SyntaxKind.ParenExpr:  return BindParenExpr((ParenExpr) syntax);

        default:
          throw new Exception($"Unexpected syntax {syntax.Kind}.");
      }
    }

    public DiagBag Diags => _diags;

    private BoundExpr BindLitExpr(LitExpr syntax) {
      var val = syntax.Val ?? 0;
      return new BoundLitExpr(val);
    }

    private BoundExpr BindUnaryExpr(UnaryExpr syntax) {
      var boundOperand = BindExpr(syntax.Operand);
      var boundOp = BoundUnaryOp.Bind(syntax.Op.Kind, boundOperand.Type);

      if (boundOp == null) {
        _diags.ReportUndefUnaryOp(syntax.Op.Span, syntax.Op?.Text ?? "", boundOperand.Type);
        return boundOperand;
      }

      return new BoundUnaryExpr(boundOp, boundOperand);
    }

    private BoundExpr BindBinaryExpr(BinaryExpr syntax) {
      var boundLeft = BindExpr(syntax.Left);
      var boundRight = BindExpr(syntax.Right);
      var boundOp = BoundBinaryOp.Bind(syntax.Op.Kind, boundLeft.Type, boundRight.Type);

      if (boundOp == null) {
        _diags.ReportUndefBinaryOp(syntax.Op.Span, syntax.Op?.Text ?? "", boundLeft.Type, boundRight.Type);
        return boundLeft;
      }

      return new BoundBinaryExpr(boundLeft, boundOp, boundRight);
    }

    private BoundExpr BindParenExpr(ParenExpr syntax) {
      return BindExpr(syntax.Expr);
    }
  }

}

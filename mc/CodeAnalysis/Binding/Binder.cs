
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
      var boundOp = BindUnaryOp(syntax.Op.Kind, boundOperand.Type);

      if (boundOp == null) {
        _diags.Add($"🌎️ Binder: Unary operator '{syntax.Op.Text}' is not defined for type {boundOperand.Type}.");
        return boundOperand;
      }

      return new BoundUnaryExpr(boundOp.Value, boundOperand);
    }

    private BoundExpr BindBinaryExpr(BinaryExprSyntax syntax) {
      var boundLeft = BindExpr(syntax.Left);
      var boundRight = BindExpr(syntax.Right);
      var boundOp = BindBinaryOp(syntax.Op.Kind, boundLeft.Type, boundRight.Type);

      if (boundOp == null) {
        _diags.Add($"🌎️ Binder: Unary operator '{syntax.Op.Text}' is not defined for type '{boundLeft.Type}' and '{boundRight.Type}.");
        return boundLeft;
      }

      return new BoundBinaryExpr(boundLeft, boundOp.Value, boundRight);
    }


    private BoundUnaryOpKind? BindUnaryOp(SyntaxKind kind, Type operandType) {
      if (operandType == typeof(int))
        switch (kind) {
          case SyntaxKind.Plus: return BoundUnaryOpKind.Identity;
          case SyntaxKind.Dash: return BoundUnaryOpKind.Negation;
        }

      if (operandType == typeof(bool))
        switch (kind) {
          case SyntaxKind.Bang: return BoundUnaryOpKind.LogicalNegation;
        }

      return null;
    }

    private BoundBinaryOpKind? BindBinaryOp(SyntaxKind kind, Type leftType, Type rightType) {
      if (leftType == typeof(int) && rightType == typeof(int))
        switch (kind)
        {
          case SyntaxKind.Plus:  return BoundBinaryOpKind.Add;
          case SyntaxKind.Dash:  return BoundBinaryOpKind.Sub;
          case SyntaxKind.Star:  return BoundBinaryOpKind.Mul;
          case SyntaxKind.Slash: return BoundBinaryOpKind.Div;
        }

      if (leftType == typeof(bool) && rightType == typeof(bool))
        switch (kind)
        {
          case SyntaxKind.AmpersandAmpersand:
            return BoundBinaryOpKind.LogicalAnd;
          case SyntaxKind.PipePipe:
            return BoundBinaryOpKind.LogicalOr;
        }

      return null;
    }

  }

}

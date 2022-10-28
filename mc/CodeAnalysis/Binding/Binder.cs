
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
      var boundOpKind = BindUnaryOpKind(syntax.Op.Kind, boundOperand.Type);

      if (boundOpKind == null) {
        _diags.Add($"Unary operator '{syntax.Op.Text}' is not defined for type {boundOperand.Type}.");
        return boundOperand;
      }

      return new BoundUnaryExpr(boundOpKind.Value, boundOperand);
    }

    private BoundExpr BindBinaryExpr(BinaryExpr syntax) {
      var boundLeft = BindExpr(syntax.Left);
      var boundRight = BindExpr(syntax.Right);
      var boundOpKind = BindBinaryOpKind(syntax.Op.Kind, boundLeft.Type, boundRight.Type);

      if (boundOpKind == null) {
        _diags.Add($"Binary operator '{syntax.Op.Text}' is not defined for types {boundLeft.Type} and {boundRight.Type}.");
        return boundLeft;
      }

      return new BoundBinaryExpr(boundLeft, boundOpKind.Value, boundRight);
    }

    private BoundUnaryOpKind? BindUnaryOpKind(SyntaxKind kind, Type operandType) {
      if (operandType == typeof(int))
        switch (kind) {
          case SyntaxKind.PlusTk: return BoundUnaryOpKind.Identity;
          case SyntaxKind.DashTk: return BoundUnaryOpKind.Neg;
        }

      if (operandType == typeof(bool))
        switch (kind) {
          case SyntaxKind.BangTk: return BoundUnaryOpKind.LgcNeg;
        }

      return null;
    }

    private BoundBinaryOpKind? BindBinaryOpKind(SyntaxKind kind, Type leftType, Type rightType) {
      if (leftType == typeof(int) && rightType == typeof(int))
        switch (kind) {
          case SyntaxKind.PlusTk:  return BoundBinaryOpKind.Add;
          case SyntaxKind.DashTk:  return BoundBinaryOpKind.Sub;
          case SyntaxKind.StarTk:  return BoundBinaryOpKind.Mul;
          case SyntaxKind.SlashTk: return BoundBinaryOpKind.Div;
        }

      if (leftType == typeof(bool) && rightType == typeof(bool))
        switch (kind) {
          case SyntaxKind.ApsdApsdTk: return BoundBinaryOpKind.LgcAnd;
          case SyntaxKind.PipePipeTk: return BoundBinaryOpKind.LgcOr;
        }

      return null;
    }
  }

}

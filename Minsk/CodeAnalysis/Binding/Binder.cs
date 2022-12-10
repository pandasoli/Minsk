
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class Binder
  {
    private readonly DiagBag _diags = new DiagBag();
    private readonly Dictionary<VarSymbol, object> _vars;

    public Binder(Dictionary<VarSymbol, object> vars) {
      _vars = vars;
    }

    public BoundExpr BindExpr(ExprSyntax syntax) {
      switch (syntax.Kind) {
        case SyntaxKind.BinExpr:    return BindBinaryExpr((BinExpr) syntax);
        case SyntaxKind.UnaryExpr:  return BindUnaryExpr((UnaryExpr) syntax);
        case SyntaxKind.LitExpr:    return BindLitExpr((LitExpr) syntax);
        case SyntaxKind.ParenExpr:  return BindParenExpr((ParenExpr) syntax);
        case SyntaxKind.NameExpr:   return BindNameExpr((NameExpr) syntax);
        case SyntaxKind.AssignExpr: return BindAssignExpr((AssignmExpr) syntax);

        default:
          throw new Exception($"Unexpected syntax {syntax.Kind}.");
      }
    }

    private BoundExpr BindNameExpr(NameExpr syntax) {
      var name = syntax.Id.Text ?? "";
      var var = _vars.Keys.FirstOrDefault(var => var.Name == name);

      if (var == null) {
        _diags.ReportUndefName(syntax.Id.Span, name);
        return new BoundLitExpr(0);
      }

      return new BoundVarExpr(var);
    }

    private BoundExpr BindAssignExpr(AssignmExpr syntax) {
      var name = syntax.Id.Text ?? "";
      var boundExpr = BindExpr(syntax.Expr);

      var exitingVar = _vars.Keys.FirstOrDefault(var => var.Name == name);

      if (exitingVar != null)
        _vars.Remove(exitingVar);

      var var = new VarSymbol(name, boundExpr.Type);

      return new BoundAssignmsExpr(var, boundExpr);
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

    private BoundExpr BindBinaryExpr(BinExpr syntax) {
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

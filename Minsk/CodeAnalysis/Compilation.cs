using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{
  public class Compilation
  {
    public Compilation(SyntaxTree syntax) {
      Syntax = syntax;
    }

    public SyntaxTree Syntax { get; }

    public EvaluatRes Evaluate(Dictionary<VarSymbol, object?> vars) {
      var binder = new Binder(vars);
      var boundExpr = binder.BindExpr(Syntax.Root);

      var diags = Syntax.Diags.Concat(binder.Diags).ToArray();

      if (diags.Any())
        return new EvaluatRes(diags, null);

      var eval = new Eval(boundExpr, vars);
      var val = eval.Evaluate();

      return new EvaluatRes(Array.Empty<Diag>(), val);
    }
  }
}

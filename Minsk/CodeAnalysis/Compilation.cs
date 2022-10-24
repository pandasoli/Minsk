using Minsk.CodeAnalysis.Syntax;
using Minsk.CodeAnalysis.Binding;

namespace Minsk.CodeAnalysis
{
  public sealed class Compilation
  {
    public Compilation(SyntaxTree syntax) {
      Syntax = syntax;
    }

    public SyntaxTree Syntax { get; }

    public EvaluationResult Evaluate() {
      var binder = new Binder();
      var boundExpr = binder.BindExpr(Syntax.Root);

      var diags = Syntax.Diags.Concat(binder.Diags).ToArray();

      if (diags.Any())
        return new EvaluationResult(diags, null);

      var eval = new Evaluator(boundExpr);
      var val = eval.Evaluate();

      return new EvaluationResult(Array.Empty<Diagnostic>(), val);
    }
  }

}

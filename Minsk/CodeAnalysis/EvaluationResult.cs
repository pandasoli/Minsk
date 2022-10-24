namespace Minsk.CodeAnalysis
{
  public sealed class EvaluationResult {
    public EvaluationResult(IEnumerable<Diagnostic> diags, object? val) {
      Diags = diags.ToArray();
      Val = val;
    }

    public IReadOnlyList<Diagnostic> Diags { get; }
    public object? Val { get; }
  }

}

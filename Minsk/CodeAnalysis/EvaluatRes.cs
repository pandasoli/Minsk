namespace Minsk.CodeAnalysis
{
  public sealed class EvaluatRes {
    public EvaluatRes(IEnumerable<Diag> diags, object? val) {
      Diags = diags.ToArray();
      Val = val;
    }

    public IReadOnlyList<Diag> Diags { get; }
    public object? Val { get; }
  }
}

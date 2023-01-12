using System.Collections.Immutable;

namespace Minsk.CodeAnalysis
{
  public sealed class EvaluatRes {
    public EvaluatRes(ImmutableArray<Diag> diags, object? val) {
      Diags = diags;
      Val = val;
    }

    public ImmutableArray<Diag> Diags { get; }
    public object? Val { get; }
  }
}

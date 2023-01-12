using Minsk.CodeAnalysis.Text;

namespace Minsk.CodeAnalysis
{
  public sealed class Diag
  {
    public Diag(TextSpan span, string msg) {
      Span = span;
      Msg = msg;
    }

    public TextSpan Span { get; }
    public string Msg { get; }

    public override string ToString() => Msg;
  }

}

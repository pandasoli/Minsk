namespace Minsk.CodeAnalysis
{
  public sealed class Diagnostic {
    public Diagnostic(TextSpan span, string msg) {
      Span = span;
      Msg = msg;
    }

    public TextSpan Span { get; }
    public string Msg { get; }

    public override string ToString() => Msg;
  }

}

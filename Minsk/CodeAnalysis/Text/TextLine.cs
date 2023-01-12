namespace Minsk.CodeAnalysis.Text
{
  public sealed class TextLine {
    public TextLine(SourceText text, int start, int len, int lenIncLineBreak) {
      Text = text;
      Start = start;
      Len = len;
      LenIncLineBreak = lenIncLineBreak;
    }

    public SourceText Text { get; }
    public int Start { get; }
    public int Len { get; }
    public int LenIncLineBreak { get; }
    public int End => Start + Len;

    public TextSpan Span => new TextSpan(Start, Len);
    public TextSpan SpanIncLineBreak => new TextSpan(Start, LenIncLineBreak);

    public override string ToString() => Text.ToString(Span);
  }

}

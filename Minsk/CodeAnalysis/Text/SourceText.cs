using System.Collections.Immutable;

namespace Minsk.CodeAnalysis.Text
{
  public sealed class SourceText
  {
    public ImmutableArray<TextLine> Lines { get; }
    public string _text { get; }

    private SourceText(string text) {
      _text = text;
      Lines = ParseLines(this, text);
    }

    public int GetLineIndex(int pos) {
      // Possible update:
      //   return _text.Substring(0, pos).Split('\n').Length;

      var lower = 0;
      var upper = Lines.Length - 1;

      while (lower <= upper) {
        var index = lower + (upper - lower) / 2;
        var start = Lines[index].Start;

        if (pos == start)
          return index;

        if (start > pos)
          upper = index + 1;
        else
          lower = index + 1;
      }

      return lower - 1;
    }

    private static ImmutableArray<TextLine> ParseLines(SourceText sourceText, string text) {
      var res = ImmutableArray.CreateBuilder<TextLine>();

      var pos = 0;
      var lineStart = 0;

      while (pos < text.Length) {
        var lineBreakWidth = GetLineBreakWidth(text, pos);

        if (lineBreakWidth == 0)
          pos++;
        else {
          AddLine(res, sourceText, pos, lineStart, lineBreakWidth);

          pos += lineBreakWidth;
          lineStart = pos;
        }
      }

      if (pos > lineStart)
        AddLine(res, sourceText, pos, lineStart, 0);

      return res.ToImmutable();
    }

    private static void AddLine(ImmutableArray<TextLine>.Builder res, SourceText sourceText, int pos, int lineStart, int lineBreakWidth) {
      var lineLen = pos - lineStart;
      var lineLenIncLineBreak = lineLen + lineBreakWidth;
      var line = new TextLine(sourceText, lineStart, lineLen, lineLenIncLineBreak);
      res.Add(line);
    }

    private static int GetLineBreakWidth(string text, int pos) {
      var ch = text[pos];
      var l = pos + 1 >= text.Length ? '\0' : text[pos + 1];

      if (ch == '\r' && l == '\n')
        return 2;

      if (ch == '\r' || ch == '\n')
        return 1;

      return 0;
    }

    public static SourceText From(string text) {
      return new SourceText(text);
    }

    public char this[int index] => _text[index];
    public int Length => _text.Length;

    public override string ToString() => _text;
    public string ToString(int start, int len) => _text.Substring(start, len);
    public string ToString(TextSpan span) => ToString(span.Start, span.Len);
  }

}

namespace Minsk.CodeAnalysis.Text
{
  public struct TextSpan
  {
    public TextSpan(int start, int len) {
      Start = start;
      Len = len;
    }

    public int Start { get; }
    public int Len { get; }
    public int End => Start + Len;

    public static TextSpan FromBounds(int start, int end) {
      int len = end - start;
      return new TextSpan(start, len);
    }
  }

}

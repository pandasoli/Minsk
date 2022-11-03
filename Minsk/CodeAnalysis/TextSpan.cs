namespace Minsk.CodeAnalysis
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
  }

}

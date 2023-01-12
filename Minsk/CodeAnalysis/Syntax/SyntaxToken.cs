
namespace Minsk.CodeAnalysis.Syntax
{
  public sealed class SyntaxToken : SyntaxNode
  {
    public SyntaxToken(SyntaxKind kind, int pos, string? text, object? val) {
      Kind = kind;
      Pos = pos;
      Text = text;
      Val = val;
    }

    public override SyntaxKind Kind { get; }
    public int Pos { get; }
    public string? Text { get; }
    public object? Val { get; }
    public TextSpan Span => new TextSpan(Pos, Text?.Length ?? 0);
  }

}

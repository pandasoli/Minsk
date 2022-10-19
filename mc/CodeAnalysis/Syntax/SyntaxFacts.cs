
namespace Minsk.CodeAnalysis.Syntax
{
  internal static class SyntaxFacts
  {
    public static int GetBinaryOpPrec(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.Star:
        case SyntaxKind.Slash:
          return 5;

        case SyntaxKind.Plus:
        case SyntaxKind.Dash:
          return 4;

        case SyntaxKind.EqualsEquals:
        case SyntaxKind.BangEquals:
          return 3;

        case SyntaxKind.AmpersandAmpersand:
          return 2;

        case SyntaxKind.PipePipe:
          return 1;

        default:
          return 0;
      }
    }

    public static int GetUnaryOpPreced(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.Plus:
        case SyntaxKind.Dash:
        case SyntaxKind.Bang:
          return 6;

        default:
          return 0;
      }
    }

    public static SyntaxKind GetKeywordKind(string text) {
      switch (text) {
        case "true":  return SyntaxKind.TrueKeyword;
        case "false": return SyntaxKind.FalseKeyword;

        default:
          return SyntaxKind.Identifier;
      }
    }
  }

}

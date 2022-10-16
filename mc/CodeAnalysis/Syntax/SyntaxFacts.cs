
namespace Minsk.CodeAnalysis.Syntax
{
  internal static class SyntaxFacts
  {
    public static int GetBinaryOpPrec(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.Star:
        case SyntaxKind.Slash:
          return 2;

        case SyntaxKind.Plus:
        case SyntaxKind.Dash:
          return 1;

        default:
          return 0;
      }
    }

    public static int GetUnaryOpPreced(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.Plus:
        case SyntaxKind.Dash:
          return 3;

        default:
          return 0;
      }
    }
  }

}


namespace Minsk.CodeAnalysis
{
  internal static class SyntaxFacts
  {
    public static int GetBinaryOpPrece(this SyntaxKind kind) {
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
  }

}

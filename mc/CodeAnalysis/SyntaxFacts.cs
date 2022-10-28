
namespace Minsk.CodeAnalysis
{
  internal static class SyntaxFacts
  {
    public static int GetUnaryOpPrece(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.PlusTk:
        case SyntaxKind.DashTk:
          return 3;

        default:
          return 0;
      }
    }
    
    public static int GetBinaryOpPrece(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.StarTk:
        case SyntaxKind.SlashTk:
          return 2;

        case SyntaxKind.PlusTk:
        case SyntaxKind.DashTk:
          return 1;

        default:
          return 0;
      }
    }
  }

}

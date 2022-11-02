
namespace Minsk.CodeAnalysis.Syntax
{
  internal static class SyntaxFacts
  {
    public static int GetUnaryOpPrece(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.PlusTk:
        case SyntaxKind.DashTk:
        case SyntaxKind.BangTk:
          return 6;

        default:
          return 0;
      }
    }
    
    public static int GetBinaryOpPrece(this SyntaxKind kind) {
      switch (kind) {
        case SyntaxKind.StarTk:
        case SyntaxKind.SlashTk:
          return 5;

        case SyntaxKind.PlusTk:
        case SyntaxKind.DashTk:
          return 4;

        case SyntaxKind.EqsEqsTk:
        case SyntaxKind.BangEqsTk:
          return 3;

        case SyntaxKind.ApsdApsdTk:
          return 2;

        case SyntaxKind.PipePipeTk:
          return 1;

        default:
          return 0;
      }
    }

    public static SyntaxKind GetKeywordKind(string text) {
      switch (text) {
        case "true":  return SyntaxKind.TrueKw;
        case "false": return SyntaxKind.FalseKw;

        default:
          return SyntaxKind.IdTk;
      }
    }
  }

}


namespace Minsk.CodeAnalysis
{
  public enum SyntaxKind {
    // Tokens
    BadTokenTk,
    EOFTk,

    NumberTk,
    WhiteSpaceTk,

    PlusTk,
    DashTk,
    StarTk,
    SlashTk,
    OpenParenTk,
    CloseParenTk,

    // Expressions
    BinaryExpr,
    UnaryExpr,
    LitExpr,
    ParenExpr
  }

}

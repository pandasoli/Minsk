
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadTokenTk,
    EOFTk,

    NumberTk,
    IdTk,
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
    ParenExpr,

    // Keywords
    FalseKw,
    TrueKw
  }

}

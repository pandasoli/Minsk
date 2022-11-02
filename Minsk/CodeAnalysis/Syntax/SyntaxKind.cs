
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadTk,
    EOFTk,

    NumberTk,
    IdTk,
    WhiteSpaceTk,

    PlusTk,
    DashTk,
    BangTk,
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
    TrueKw,

    // Comparators
    ApsdApsdTk,
    PipePipeTk,
    EqsEqsTk,
    BangEqsTk
  }

}

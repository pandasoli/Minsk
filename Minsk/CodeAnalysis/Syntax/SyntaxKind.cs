
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadTk,
    EOFTk,

    NumberTk,
    IdTk,
    NameExpr,
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
    AssignExpr,

    // Keywords
    FalseKw,
    TrueKw,

    // Comparators
    ApsdApsdTk,
    PipePipeTk,
    EqsEqsTk,
    EqsTk,
    BangEqsTk
  }

}

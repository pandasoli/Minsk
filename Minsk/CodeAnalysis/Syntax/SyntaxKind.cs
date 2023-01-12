
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadTk,
    EOFTk,

    NumTk,
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
    BinExpr,
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
    EqsTk,
    EqsEqsTk,
    BangEqsTk
  }

}

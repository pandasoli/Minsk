
namespace Minsk.CodeAnalysis
{
  public enum SyntaxKind {
    // Tokens
    BadToken,
    EOF,

    Number,
    WhiteSpace,

    Plus,
    Dash,
    Star,
    Slash,
    OpenParen,
    CloseParen,

    // Expressions
    BinaryExpr,
    NumberExpr,
    ParenExpr
  }

}

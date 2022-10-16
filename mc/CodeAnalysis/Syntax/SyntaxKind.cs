
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadToken,
    EOF,

    WhiteSpace,

    Number,
    Plus,
    Dash,
    Star,
    Slash,

    OpenParen,
    CloseParen,

    // Nodes
    LiteralExpr,
    BinaryExpr,
    UnaryExpr,
    ParenExpr
  }

}

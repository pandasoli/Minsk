
namespace Minsk.CodeAnalysis.Syntax
{
  public enum SyntaxKind {
    // Tokens
    BadToken,
    EOF,

    WhiteSpace,

    Number,
    Identifier,
    Plus,
    Dash,
    Star,
    Slash,

    Bang,
    AmpersandAmpersand,
    PipePipe,

    OpenParen,
    CloseParen,

    // Keyword
    TrueKeyword,
    FalseKeyword,

    // Nodes
    LiteralExpr,
    BinaryExpr,
    UnaryExpr,
    ParenExpr
  }

}

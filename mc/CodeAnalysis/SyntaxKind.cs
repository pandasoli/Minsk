
namespace Minsk.CodeAnalysis
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
    LiteralNode,
    BinaryNode,
    ParenExpr
  }

}

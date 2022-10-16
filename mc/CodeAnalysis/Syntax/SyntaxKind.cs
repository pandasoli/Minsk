
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
    LiteralNode,
    BinaryNode,
    UnaryNode,
    ParenExpr
  }

}

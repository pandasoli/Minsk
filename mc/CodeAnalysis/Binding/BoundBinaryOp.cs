using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinaryOp
  {
    private BoundBinaryOp(SyntaxKind syntax, BoundBinaryOpKind kind, Type type) : this(syntax, kind, type, type, type) {}
    private BoundBinaryOp(SyntaxKind syntax, BoundBinaryOpKind kind, Type left, Type right, Type res) {
      Syntax = syntax;
      Kind = kind;
      Left = left;
      Right = right;
      Res = res;
    }

    public SyntaxKind Syntax { get; }
    public BoundBinaryOpKind Kind { get; }
    public Type Left { get; }
    public Type Right { get; }
    public Type Res { get; }

    private static BoundBinaryOp[] _ops = {
      new BoundBinaryOp(SyntaxKind.Plus, BoundBinaryOpKind.Add, typeof(int)),
      new BoundBinaryOp(SyntaxKind.Dash, BoundBinaryOpKind.Sub, typeof(int)),
      new BoundBinaryOp(SyntaxKind.Star, BoundBinaryOpKind.Mul, typeof(int)),
      new BoundBinaryOp(SyntaxKind.Slash, BoundBinaryOpKind.Div, typeof(int)),

      new BoundBinaryOp(SyntaxKind.AmpersandAmpersand, BoundBinaryOpKind.LogicalAnd, typeof(bool)),
      new BoundBinaryOp(SyntaxKind.PipePipe, BoundBinaryOpKind.LogicalOr, typeof(bool))
    };

    public static BoundBinaryOp? Bind(SyntaxKind syntax, Type left, Type right) {
      foreach (var op in _ops) {
        if (op.Syntax == syntax && op.Left == left && op.Right == right)
          return op;
      }

      return null;
    }
  }

}

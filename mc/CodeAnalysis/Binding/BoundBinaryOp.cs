using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinaryOp {
    private BoundBinaryOp(SyntaxKind syntaxKind, BoundBinaryOpKind kind, Type type) : this(syntaxKind, kind, type, type, type) {}

    private BoundBinaryOp(SyntaxKind syntaxKind, BoundBinaryOpKind kind, Type left, Type right, Type res) {
      SyntaxKind = syntaxKind;
      Kind = kind;
      Left = left;
      Right = right;
      Res = res;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundBinaryOpKind Kind { get; }
    public Type Left { get; }
    public Type Right { get; }
    public Type Res { get; }

    private static BoundBinaryOp[] _ops = {
      new BoundBinaryOp(SyntaxKind.PlusTk, BoundBinaryOpKind.Add, typeof(int)),
      new BoundBinaryOp(SyntaxKind.DashTk, BoundBinaryOpKind.Sub, typeof(int)),
      new BoundBinaryOp(SyntaxKind.StarTk, BoundBinaryOpKind.Mul, typeof(int)),
      new BoundBinaryOp(SyntaxKind.SlashTk, BoundBinaryOpKind.Div, typeof(int)),

      new BoundBinaryOp(SyntaxKind.ApsdApsdTk, BoundBinaryOpKind.LgcAnd, typeof(bool)),
      new BoundBinaryOp(SyntaxKind.PipePipeTk, BoundBinaryOpKind.LgcOr, typeof(bool))
    };

    public static BoundBinaryOp? Bind(SyntaxKind syntaxKind, Type left, Type right) {
      foreach (var op in _ops) {
        if (op.SyntaxKind == syntaxKind && op.Left == left && op.Right == right)
          return op;
      }

      return null;
    }
  }

}

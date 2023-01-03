using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundBinOp {
    private BoundBinOp(SyntaxKind syntaxKind, BoundBinOpKind kind, Type type) : this(syntaxKind, kind, type, type, type) {}
    private BoundBinOp(SyntaxKind syntaxKind, BoundBinOpKind kind, Type operand, Type res) : this(syntaxKind, kind, operand, operand, res) {}

    private BoundBinOp(SyntaxKind syntaxKind, BoundBinOpKind kind, Type left, Type right, Type res) {
      SyntaxKind = syntaxKind;
      Kind = kind;
      Left = left;
      Right = right;
      Type = res;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundBinOpKind Kind { get; }
    public Type Left { get; }
    public Type Right { get; }
    public Type Type { get; }

    private static BoundBinOp[] _ops = {
      new BoundBinOp(SyntaxKind.PlusTk, BoundBinOpKind.Add, typeof(int)),
      new BoundBinOp(SyntaxKind.DashTk, BoundBinOpKind.Sub, typeof(int)),
      new BoundBinOp(SyntaxKind.StarTk, BoundBinOpKind.Mul, typeof(int)),
      new BoundBinOp(SyntaxKind.SlashTk, BoundBinOpKind.Div, typeof(int)),

      new BoundBinOp(SyntaxKind.ApsdApsdTk, BoundBinOpKind.LgcAnd, typeof(bool)),
      new BoundBinOp(SyntaxKind.PipePipeTk, BoundBinOpKind.LgcOr, typeof(bool)),

      new BoundBinOp(SyntaxKind.EqsEqsTk, BoundBinOpKind.Eqs, typeof(int), typeof(bool)),
      new BoundBinOp(SyntaxKind.BangEqsTk, BoundBinOpKind.NotEqs, typeof(int), typeof(bool)),
      new BoundBinOp(SyntaxKind.EqsEqsTk, BoundBinOpKind.Eqs, typeof(bool)),
      new BoundBinOp(SyntaxKind.BangEqsTk, BoundBinOpKind.NotEqs, typeof(bool))
    };

    public static BoundBinOp? Bind(SyntaxKind syntaxKind, Type left, Type right) {
      foreach (var op in _ops) {
        if (op.SyntaxKind == syntaxKind && op.Left == left && op.Right == right)
          return op;
      }

      return null;
    }
  }

}

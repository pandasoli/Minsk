using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundUnaryOp {
    private BoundUnaryOp(SyntaxKind syntaxKind, BoundUnaryOpKind kind, Type operand) : this(syntaxKind, kind, operand, operand) {}

    private BoundUnaryOp(SyntaxKind syntaxKind, BoundUnaryOpKind kind, Type operand, Type res) {
      SyntaxKind = syntaxKind;
      Kind = kind;
      Operand = operand;
      Res = res;
    }

    public SyntaxKind SyntaxKind { get; }
    public BoundUnaryOpKind Kind { get; }
    public Type Operand { get; }
    public Type Res { get; }

    private static BoundUnaryOp[] _ops = {
      new BoundUnaryOp(SyntaxKind.BangTk, BoundUnaryOpKind.LgcNeg, typeof(bool)),

      new BoundUnaryOp(SyntaxKind.PlusTk, BoundUnaryOpKind.Identity, typeof(int)),
      new BoundUnaryOp(SyntaxKind.DashTk, BoundUnaryOpKind.Neg, typeof(int))
    };

    public static BoundUnaryOp? Bind(SyntaxKind syntaxKind, Type operand) {
      foreach (var op in _ops) {
        if (op.SyntaxKind == syntaxKind && op.Operand == operand)
          return op;
      }

      return null;
    }
  }

}

using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis.Binding
{
  internal sealed class BoundUnaryOp
  {
    private BoundUnaryOp(SyntaxKind syntax, BoundUnaryOpKind kind, Type operand) : this(syntax, kind, operand, operand) {}
    private BoundUnaryOp(SyntaxKind syntax, BoundUnaryOpKind kind, Type operand, Type res) {
      Syntax = syntax;
      Kind = kind;
      Operand = operand;
      Res = res;
    }

    public SyntaxKind Syntax { get; }
    public BoundUnaryOpKind Kind { get; }
    public Type Operand { get; }
    public Type Res { get; }

    private static BoundUnaryOp[] _ops = {
      new BoundUnaryOp(SyntaxKind.Bang, BoundUnaryOpKind.LogicalNegation, typeof(bool)),

      new BoundUnaryOp(SyntaxKind.Plus, BoundUnaryOpKind.Identity, typeof(int)),
      new BoundUnaryOp(SyntaxKind.Dash, BoundUnaryOpKind.Negation, typeof(int))
    };

    public static BoundUnaryOp? Bind(SyntaxKind syntax, Type operand) {
      foreach (var op in _ops) {
        if (op.Syntax == syntax && op.Operand == operand)
          return op;
      }

      return null;
    }
  }

}

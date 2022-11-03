using System.Collections;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{
  public sealed class DiagBag : IEnumerable<Diag>
  {
    private readonly List<Diag> _diags = new List<Diag>();

    public IEnumerator<Diag> GetEnumerator() => _diags.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(DiagBag diags) {
      _diags.AddRange(diags._diags);
    }

    private void Report(TextSpan span, string msg) {
      var diag = new Diag(span, msg);
      _diags.Add(diag);
    }


    public void ReportInvNum(TextSpan span, string buff, Type type) {
      var msg = $"The number {buff} is not a valid {type}.";
      Report(span, msg);
    }

    public void ReportBadCh(int pos, char ch) {
      var span = new TextSpan(pos, 1);
      var msg = $"Bad character input: '{ch}'.";

      Report(span, msg);
    }

    public void ReportUnexpctTk(TextSpan span, SyntaxKind kind, SyntaxKind expect) {
      var msg = $"Unexpected token <{kind}>, expected <{expect}>.";
      Report(span, msg);
    }

    public void ReportUndefUnaryOp(TextSpan span, string opText, Type operandType) {
      var msg = $"Unary operator '{opText}' is not defined for type {operandType}.";
      Report(span, msg);
    }

    public void ReportUndefBinaryOp(TextSpan span, string op, Type left, Type right) {
      var msg = $"Binary operator '{op}' is not defined for types {left} and {right}.";
      Report(span, msg);
    }

    public void ReportUndefName(TextSpan span, string name) {
      var msg = $"Variable '{name}' doesn't exist.";
      Report(span, msg);
    }
  }
}

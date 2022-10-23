using System.Collections;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk.CodeAnalysis
{
  internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
  {
    private readonly List<Diagnostic> _diags = new List<Diagnostic>();

    public IEnumerator<Diagnostic> GetEnumerator() => _diags.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    private void Report(TextSpan span, string msg) {
      var diag = new Diagnostic(span, msg);
      _diags.Add(diag);
    }

    public void ReportInvalidNumber(TextSpan span, string text, Type type) {
      var msg = $"The number {text} is not a valid {type}.";
      Report(span, msg);
    }

    public void AddRange(DiagnosticBag diags) {
      _diags.AddRange(diags._diags);
    }

    public void ReportBadChar(int pos, char current) {
      var span = new TextSpan(pos, 1);
      var msg = $"Bad character input: '{current}'.";

      Report(span, msg);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind currentKind, SyntaxKind expectedKind) {
      var msg = $"Unexpected token <{currentKind}>, expected <{expectedKind}>.";
      Report(span, msg);
    }

    public void ReportUndefUnaryOp(TextSpan span, string? opText, Type operandType) {
      var msg = $"Unary operator '{opText}' is not defined for type {operandType}.";
      Report(span, msg);
    }

    public void ReportUndefBinaryOp(TextSpan span, string? opText, Type leftType, Type rightType) {
      var msg = $"Unary operator '{opText}' is not defined for type '{leftType}' and '{rightType}.";
      Report(span, msg);
    }
  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Lexer
  {
    private DiagnosticBag _diags = new DiagnosticBag();
    private readonly string _text;
    private int _pos;

    public Lexer(string text) {
      _text = text;
    }

    public DiagnosticBag Diags => _diags;

    private char Current => Peek();
    private char Lookhead => Peek(1);

    private char Peek(int offset = 0)
    {
      var idx = _pos + offset;

      if (idx >= _text.Length)
        return '\0';

      return _text[idx];
    }

    private void Next() {
      _pos++;
    }

    public SyntaxToken Lex()
    {
      if (_pos >= _text.Length)
        return new SyntaxToken(SyntaxKind.EOF, _pos, "\0", null);

      var start = _pos;

      if (char.IsDigit(Current)) {
        while (char.IsDigit(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        if (!int.TryParse(buff, out var res))
          _diags.ReportInvalidNumber(new TextSpan(start, len), _text, typeof(int));

        return new SyntaxToken(SyntaxKind.Number, start, buff, res);
      }
      else if (char.IsWhiteSpace(Current)) {
        while (char.IsWhiteSpace(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        return new SyntaxToken(SyntaxKind.WhiteSpace, start, buff, null);
      }
      else if (char.IsLetter(Current)) {
        while (char.IsLetter(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);
        var kind = SyntaxFacts.GetKeywordKind(buff);

        return new SyntaxToken(kind, start, buff, null);
      }
      else {
        var kind = SyntaxKind.BadToken;
        var pos = _pos + 1;
        var buff = _text[_pos].ToString();

        switch (Current) {
          case '+': kind = SyntaxKind.Plus;       break;
          case '-': kind = SyntaxKind.Dash;       break;
          case '*': kind = SyntaxKind.Star;       break;
          case '/': kind = SyntaxKind.Slash;      break;
          case '(': kind = SyntaxKind.OpenParen;  break;
          case ')': kind = SyntaxKind.CloseParen; break;
          case '&':
            if (Lookhead == '&') {
              kind = SyntaxKind.AmpersandAmpersand;
              pos++;
              buff = "&&";
            }

            break;
          case '|':
            if (Lookhead == '|') {
              kind = SyntaxKind.PipePipe;
              pos++;
              buff = "||";
            }

            break;
          case '=':
            if (Lookhead == '=') {
              kind = SyntaxKind.EqualsEquals;
              pos++;
              buff = "==";
            }

            break;
          case '!':
            if (Lookhead == '=') {
              kind = SyntaxKind.BangEquals;
              pos++;
              buff = "==";
            }
            else {
              kind = SyntaxKind.Bang;
            }

            break;
        }

        if (kind == SyntaxKind.BadToken)
          _diags.ReportBadChar(_pos, Current);

        _pos = pos;
        return new SyntaxToken(kind, start, buff, null);
      }
    }
  }

}

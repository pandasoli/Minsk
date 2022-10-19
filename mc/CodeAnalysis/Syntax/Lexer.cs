
namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Lexer
  {
    private List<string> _diags = new List<string>();
    private readonly string _text;
    private int _pos;

    public Lexer(string text) {
      _text = text;
    }

    public IEnumerable<string> Diags => _diags;

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

      if (char.IsDigit(Current)) {
        var start = _pos;

        while (char.IsDigit(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        if (!int.TryParse(buff, out var res))
          _diags.Add($"🔨️ Lexer: the number {_text} is not a valid Int32.");

        return new SyntaxToken(SyntaxKind.Number, start, buff, res);
      }
      else if (char.IsWhiteSpace(Current)) {
        var start = _pos;

        while (char.IsWhiteSpace(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        return new SyntaxToken(SyntaxKind.WhiteSpace, start, buff, null);
      }
      else if (char.IsLetter(Current)) {
        var start = _pos;

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
        var buff = _text[_pos /* -1 */].ToString();

        switch (Current) {
          case '+': kind = SyntaxKind.Plus;       break;
          case '-': kind = SyntaxKind.Dash;       break;
          case '*': kind = SyntaxKind.Star;       break;
          case '/': kind = SyntaxKind.Slash;      break;
          case '(': kind = SyntaxKind.OpenParen;  break;
          case ')': kind = SyntaxKind.CloseParen; break;
          case '!': kind = SyntaxKind.Bang;       break;
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
        }

        if (kind == SyntaxKind.BadToken)
          _diags.Add($"🔨️ Lexer: bad character input: '{Current}'.");

        _pos = pos;
        return new SyntaxToken(kind, _pos, buff, null);
      }
    }
  }

}

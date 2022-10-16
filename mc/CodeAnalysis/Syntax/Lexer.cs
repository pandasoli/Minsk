
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

    private char Current {
      get {
        if (_pos >= _text.Length)
          return '\0';

        return _text[_pos];
      }
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
          _diags.Add($"The number {_text} is not a valid Int32.");

        return new SyntaxToken(SyntaxKind.Number, start, buff, res);
      }

      if (char.IsWhiteSpace(Current)) {
        var start = _pos;

        while (char.IsWhiteSpace(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        return new SyntaxToken(SyntaxKind.WhiteSpace, start, buff, null);
      }

      var kind =
        Current == '+' ? SyntaxKind.Plus :
        Current == '-' ? SyntaxKind.Dash :
        Current == '*' ? SyntaxKind.Star :
        Current == '*' ? SyntaxKind.Slash :
        Current == '(' ? SyntaxKind.OpenParen :
        Current == ')' ? SyntaxKind.CloseParen :
        SyntaxKind.BadToken;

      if (kind == SyntaxKind.BadToken)
        _diags.Add($"🔨️ Lexer: bad character input: '{Current}'.");

      return new SyntaxToken(kind, _pos++, _text[_pos - 1].ToString(), null);
    }
  }

}

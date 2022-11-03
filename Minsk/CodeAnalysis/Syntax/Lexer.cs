
namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Lexer
  {
    private DiagBag _diags = new DiagBag();
    private readonly string _text;
    private int _pos;

    public Lexer(string text) {
      _text = text;
    }

    public DiagBag Diags => _diags;


    private char Current => Peek();
    private char Lookahead => Peek(1);

    private char Peek(int offset = 0) {
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
        return new SyntaxToken(SyntaxKind.EOFTk, _pos, "\0", null);

      var start = _pos;

      if (char.IsDigit(Current))
      {
        while (char.IsDigit(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);
        if (!int.TryParse(buff, out var res))
          _diags.ReportInvNum(new TextSpan(start, len), buff, typeof(int));

        return new SyntaxToken(SyntaxKind.NumberTk, start, buff, res);
      }
      else if (char.IsWhiteSpace(Current))
      {
        while (char.IsWhiteSpace(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);

        return new SyntaxToken(SyntaxKind.WhiteSpaceTk, start, buff, null);
      }
      else if (char.IsLetter(Current))
      {
        while (char.IsLetter(Current))
          Next();

        var len = _pos - start;
        var buff = _text.Substring(start, len);
        var kind = SyntaxFacts.GetKeywordKind(buff);

        return new SyntaxToken(kind, start, buff, null);
      }
      else
      {
        var kind = SyntaxKind.BadTk;
        var pos = _pos + 1;
        var buff = _text[_pos].ToString();

        switch (Current) {
          case '+': kind = SyntaxKind.PlusTk;       break;
          case '-': kind = SyntaxKind.DashTk;       break;
          case '*': kind = SyntaxKind.StarTk;       break;
          case '/': kind = SyntaxKind.SlashTk;      break;
          case '(': kind = SyntaxKind.OpenParenTk;  break;
          case ')': kind = SyntaxKind.CloseParenTk; break;
          case '&':
            if (Lookahead == '&') {
              kind = SyntaxKind.ApsdApsdTk;
              pos++;
              buff = "&&";
            }

            break;
          case '|':
            if (Lookahead == '|') {
              kind = SyntaxKind.PipePipeTk;
              pos++;
              buff = "||";
            }

            break;
          case '=':
            if (Lookahead == '=') {
              kind = SyntaxKind.EqsEqsTk;
              pos++;
              buff = "==";
            }
            else
              kind = SyntaxKind.EqsTk;

            break;
          case '!':
            if (Lookahead == '=') {
              kind = SyntaxKind.BangEqsTk;
              pos++;
              buff = "==";
            }
            else
              kind = SyntaxKind.BangTk;

            break;
        }

        if (kind == SyntaxKind.BadTk)
          _diags.ReportBadCh(pos, Current);

        _pos = pos;
        return new SyntaxToken(kind, start, buff, null);
      }
    }
  }

}


namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Lexer
  {
    private readonly DiagBag _diags = new DiagBag();
    private readonly string _text;

    private int _pos;

    private int _start;
    private SyntaxKind _kind;
    private object? _val;

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


    public SyntaxToken Lex()
    {
      _start = _pos;
      _kind = SyntaxKind.BadTk;
      _val = null;

      switch (Current) {
        case '\0':        _kind = SyntaxKind.EOFTk;        break;
        case '+':  _pos++; _kind = SyntaxKind.PlusTk;       break;
        case '-':  _pos++; _kind = SyntaxKind.DashTk;       break;
        case '*':  _pos++; _kind = SyntaxKind.StarTk;       break;
        case '/':  _pos++; _kind = SyntaxKind.SlashTk;      break;
        case '(':  _pos++; _kind = SyntaxKind.OpenParenTk;  break;
        case ')':  _pos++; _kind = SyntaxKind.CloseParenTk; break;

        case '&':
          if (Lookahead == '&') {
            _kind = SyntaxKind.ApsdApsdTk;
            _pos += 2;
          }

          break;

        case '|':
          if (Lookahead == '|') {
            _kind = SyntaxKind.PipePipeTk;
            _pos += 2;
          }

          break;

        case '=':
          _pos++;

          if (Current == '=') {
            _kind = SyntaxKind.EqsEqsTk;
            _pos++;
          }
          else
            _kind = SyntaxKind.EqsTk;

          break;

        case '!':
          _pos++;

          if (Current == '=') {
            _kind = SyntaxKind.BangEqsTk;
            _pos++;
          }
          else
            _kind = SyntaxKind.BangTk;

          break;

        case '0': case '1': case '2': case '3': case '4':
        case '5': case '6': case '7': case '8': case '9':
          ReadNumTk();
          break;

        case ' ': case '\t': case '\n': case '\r':
          ReadWhiteSpaceTk();
          break;

        default:
          if      (char.IsLetter(Current))     ReadIdOrKw();
          // The up code only cover the most used whites paces
          else if (char.IsWhiteSpace(Current)) ReadWhiteSpaceTk();
          else {
            _diags.ReportBadCh(_pos, Current);
            _pos++;
          }

          break;
      }

      int len = _pos - _start;
      string buff = SyntaxFacts.GetText(_kind) ?? _text.Substring(_start, len);

      return new SyntaxToken(_kind, _start, buff, _val);
    }

    private void ReadNumTk() {
      while (char.IsDigit(Current))
        _pos++;

      var len = _pos - _start;
      var buff = _text.Substring(_start, len);
      if (!int.TryParse(buff, out var res))
        _diags.ReportInvNum(new TextSpan(_start, len), _text, typeof(int));

      _val = res;
      _kind = SyntaxKind.NumberTk;
    }

    private void ReadWhiteSpaceTk() {
      while (char.IsWhiteSpace(Current))
        _pos++;

      _kind = SyntaxKind.WhiteSpaceTk;
    }

    private void ReadIdOrKw() {
      while (char.IsLetter(Current))
        _pos++;

      var len = _pos - _start;
      var buff = _text.Substring(_start, len);
      _kind = SyntaxFacts.GetKeywordKind(buff);
    }
  }

}

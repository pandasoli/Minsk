
using System.Collections.Immutable;

namespace Minsk.CodeAnalysis.Syntax
{
  internal sealed class Parser
  {
    private readonly DiagBag _diags = new DiagBag();
    private readonly ImmutableArray<SyntaxToken> _tokens;
    private int _pos;

    public DiagBag Diags => _diags;

    public Parser(string text) {
      var tokens = new List<SyntaxToken>();
      var lex = new Lexer(text);
      SyntaxToken token;

      do {
        token = lex.Lex();

        if (
          token.Kind != SyntaxKind.WhiteSpaceTk &&
          token.Kind != SyntaxKind.BadTk
        )
          tokens.Add(token);
      }
      while (token.Kind != SyntaxKind.EOFTk);

      _tokens = tokens.ToImmutableArray();
      _diags.AddRange(lex.Diags);
    }

    private SyntaxToken Peek(int offset = 0) {
      var idx = _pos + offset;

      if (idx >= _tokens.Length)
        return _tokens[_tokens.Length - 1];

      return _tokens[idx];
    }

    private SyntaxToken Current => Peek();

    private SyntaxToken Next() {
      var current = Current;
      _pos++;
      return current;
    }

    private SyntaxToken Match(SyntaxKind kind) {
      if (Current.Kind == kind)
        return Next();

      _diags.ReportUnexpctTk(Current.Span, Current.Kind, kind);
      return new SyntaxToken(kind, Current.Pos, null, null);
    }

    public SyntaxTree Parse() {
      var expr = ParseExpr();
      var eOF = Match(SyntaxKind.EOFTk);

      return new SyntaxTree(_diags.ToImmutableArray(), expr, eOF);
    }

    private ExprSyntax ParseExpr() {
      return ParseAssignExpr();
    }

    private ExprSyntax ParseAssignExpr() {
      if (
        Peek(0).Kind == SyntaxKind.IdTk &&
        Peek(1).Kind == SyntaxKind.EqsTk
      ) {
        var id = Next();
        var op = Next();
        var right = ParseAssignExpr();
        return new AssignmExpr(id, op, right);
      }

      return ParseBinaryExpr();
    }

    private ExprSyntax ParseBinaryExpr(int parentPrece = 0) {
      ExprSyntax left;
      var unaryOpPrece = Current.Kind.GetUnaryOpPrece();

      if (unaryOpPrece != 0 && unaryOpPrece >= parentPrece) {
        var op = Next();
        var operand = ParseBinaryExpr(unaryOpPrece);
        left = new UnaryExpr(op, operand);
      }
      else {
        left = ParsePrimExpr();
      }

      while (true) {
        var prece = Current.Kind.GetBinOpPrece();
        if (prece == 0 || prece <= parentPrece)
          break;

        var op = Next();
        var right = ParseBinaryExpr(prece);
        left = new BinExpr(left, op, right);
      }

      return left;
    }

    private ExprSyntax ParsePrimExpr() {
      switch (Current.Kind) {
        case SyntaxKind.OpenParenTk:
          return ParseParenExpr();

        case SyntaxKind.TrueKw:
        case SyntaxKind.FalseKw:
          return ParseBoolLit();

        case SyntaxKind.NumTk:
          return ParseNumLit();

        case SyntaxKind.IdTk:
        default:
          return ParseNameExpr();
      }
    }

    private ExprSyntax ParseNumLit() {
      var num = Match(SyntaxKind.NumTk);
      return new LitExpr(num);
    }

    private ExprSyntax ParseBoolLit() {
      var isTrue = Current.Kind == SyntaxKind.TrueKw;
      var token = Match(isTrue ? SyntaxKind.TrueKw : SyntaxKind.FalseKw);
      return new LitExpr(token, isTrue);
    }

    private ExprSyntax ParseNameExpr() {
      var id = Match(SyntaxKind.IdTk);
      return new NameExpr(id);
    }

    private ExprSyntax ParseParenExpr() {
      var left = Match(SyntaxKind.OpenParenTk);
      var expr = ParseExpr();
      var right = Match(SyntaxKind.CloseParenTk);

      return new ParenExpr(left, expr, right);
    }
  }

}

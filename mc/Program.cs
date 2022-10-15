using System;
using System.Collections.Generic;
using System.Linq;

namespace Minsk
{
  class Program
  {
    static void Main(string[] args) {
      bool showTree = false;

      while (true) {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("> ");
        Console.ResetColor();

        var line = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(line))
          return;

        if (line == "#showTree") {
          showTree = !showTree;
          Console.ForegroundColor = showTree ? ConsoleColor.Green : ConsoleColor.Yellow;
          Console.WriteLine("  🌴️ " + (showTree ? "Showing" : "Not showing") + " parse trees." );
          Console.ResetColor();
          continue;
        }
        else if (line == "#cls") {
          Console.Clear();
          continue;
        }

        var ast = SyntaxTree.Parse(line);

        if (showTree)
          PrettyPrint(ast.Root);

        if (ast.Diags.Any()) {
          Console.ForegroundColor = ConsoleColor.Red;

          foreach (var diag in ast.Diags) {
            Console.WriteLine($"  {diag}  ");
          }

          Console.ResetColor();
        }
        else {
          var eval = new Evaluator(ast.Root);
          var res = eval.Evaluate();

          Console.ForegroundColor = ConsoleColor.DarkCyan;
          Console.Write("< ");
          Console.ResetColor();
          Console.Write(res);

          Console.WriteLine();
        }
      }
    }

    static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true) {
      var marker = isLast ? "╰─ " : "├─ ";

      Console.ForegroundColor = ConsoleColor.DarkGray;
      Console.Write(indent);
      Console.Write(marker);
      Console.ForegroundColor = ConsoleColor.Blue;
      Console.Write(node.Kind);
      Console.ResetColor();

      if (node is SyntaxToken tok && tok.Val != null) {
        Console.Write(": ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.Write(tok.Val);

        Console.ResetColor();
      }

      Console.WriteLine();

      indent += isLast ? "   " : "│  ";
      var last = node.GetChildren().LastOrDefault();

      foreach (var child in node.GetChildren())
        PrettyPrint(child, indent, child == last);
    }
  }

  enum SyntaxKind {
    Number,
    WhiteSpace,
    Plus,
    Dash,
    Star,
    Slash,
    OpenParen,
    CloseParen,
    BadToken,
    EOF,
    BinaryNode,
    NumberNode,
    ParenExpr
  }

  class SyntaxToken : SyntaxNode
  {
    public SyntaxToken(SyntaxKind kind, int pos, string? text, object? val) {
      Kind = kind;
      Pos = pos;
      Text = text;
      Val = val;
    }

    public override SyntaxKind Kind { get; }
    public int Pos { get; }
    public string? Text { get; }
    public object? Val { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      return Enumerable.Empty<SyntaxNode>();
    }
  }

  class Lexer
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

      if ("+-*/()".Contains(Current)) {
        var kind =
          Current == '+' ? SyntaxKind.Plus :
          Current == '-' ? SyntaxKind.Dash :
          Current == '*' ? SyntaxKind.Star :
          Current == '*' ? SyntaxKind.Slash :
          Current == '(' ? SyntaxKind.OpenParen :
          SyntaxKind.CloseParen;

        return new SyntaxToken(kind, _pos++, _text[_pos - 1].ToString(), null);
      }

      _diags.Add($"🔨️ Lexer: bad character input: '{Current}'.");
      return new SyntaxToken(SyntaxKind.BadToken, _pos++, _text[_pos - 1].ToString(), null);
    }
  }

  abstract class SyntaxNode {
    public abstract SyntaxKind Kind { get; }

    public abstract IEnumerable<SyntaxNode> GetChildren();
  }

  abstract class ExpressionNode : SyntaxNode {}

  sealed class NumberNode : ExpressionNode {
    public NumberNode(SyntaxToken num) {
      Token = num;
    }

    public override SyntaxKind Kind => SyntaxKind.NumberNode;
    public SyntaxToken Token { get; }

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Token;
    }
  }

  sealed class BinaryNode : ExpressionNode {
    public BinaryNode(ExpressionNode left, SyntaxToken op, ExpressionNode right) {
      Left = left;
      Op = op;
      Right = right;
    }

    public ExpressionNode Left { get; }
    public SyntaxToken Op { get; }
    public ExpressionNode Right { get; }

    public override SyntaxKind Kind => SyntaxKind.BinaryNode;

    public override IEnumerable<SyntaxNode> GetChildren() {
      yield return Left;
      yield return Op;
      yield return Right;
    }
  }

  sealed class ParenExprNode : ExpressionNode
  {
    public ParenExprNode(SyntaxToken open, ExpressionNode expr, SyntaxToken close) {
      Open = open;
      Expr = expr;
      Close = close;
    }

    public override SyntaxKind Kind => SyntaxKind.ParenExpr;

    public SyntaxToken Open { get; }
    public ExpressionNode Expr { get; }
    public SyntaxToken Close { get; }

    public override IEnumerable<SyntaxNode> GetChildren()
    {
      yield return Open;
      yield return Expr;
      yield return Close;
    }
  }

  sealed class SyntaxTree {
    public SyntaxTree(IEnumerable<string> diags, ExpressionNode root, SyntaxToken eOF) {
      Diags = diags.ToArray();
      Root = root;
      EOF = eOF;
    }

    public IReadOnlyList<string> Diags { get; }
    public ExpressionNode Root { get; }
    public SyntaxToken EOF { get; }

    public static SyntaxTree Parse(string text) {
      var par = new Parser(text);
      return par.Parse();
    }
  }

  class Parser
  {
    private List<string> _diags = new List<string>();
    private readonly SyntaxToken[] _tokens;
    private int _pos;

    public Parser(string text) {
      var tokens = new List<SyntaxToken>();
      var lex = new Lexer(text);
      SyntaxToken token;

      do {
        token = lex.Lex();

        if (
          token.Kind != SyntaxKind.WhiteSpace &&
          token.Kind != SyntaxKind.BadToken
        )
          tokens.Add(token);
      }
      while (token.Kind != SyntaxKind.EOF);

      _tokens = tokens.ToArray();
      _diags.AddRange(lex.Diags);
    }

    public IEnumerable<string> Diags => _diags;

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

      _diags.Add($"🎰️ Parser: Unexpected token <{Current.Kind}>, expected <{kind}>.");
      return new SyntaxToken(kind, Current.Pos, null, null);
    }

    public SyntaxTree Parse() {
      var expr = ParseExpr();
      var eOF = Match(SyntaxKind.EOF);

      return new SyntaxTree(_diags, expr, eOF);
    }

    private ExpressionNode ParseExpr() {
      var left = ParseTerm();

      while (
        Current.Kind == SyntaxKind.Plus ||
        Current.Kind == SyntaxKind.Dash
      ) {
        var op = Next();
        var right = ParseTerm();

        left = new BinaryNode(left, op, right);
      }

      return left;
    }

    private ExpressionNode ParseTerm() {
      var left = ParseFactor();

      while (
        Current.Kind == SyntaxKind.Star ||
        Current.Kind == SyntaxKind.Slash
      ) {
        var op = Next();
        var right = ParseFactor();

        left = new BinaryNode(left, op, right);
      }

      return left;
    }

    private ExpressionNode ParseFactor() {
      if (Current.Kind == SyntaxKind.OpenParen) {
        var left = Next();
        var expr = ParseExpr();
        var right = Match(SyntaxKind.CloseParen);

        return new ParenExprNode(left, expr, right);
      }

      var num = Match(SyntaxKind.Number);
      return new NumberNode(num);
    }

  }

  class Evaluator
  {
    private readonly ExpressionNode _root;

    public Evaluator(ExpressionNode root) {
      _root = root;
    }

    public int Evaluate() {
      return EvalExpr(_root);
    }

    private int EvalExpr(ExpressionNode node) {
      if (node is NumberNode num)
        return num.Token.Val != null ? (int) num.Token.Val : 0;

      if (node is BinaryNode bin) {
        var left = EvalExpr(bin.Left);
        var right = EvalExpr(bin.Right);

        switch (bin.Op.Kind) {
          case SyntaxKind.Plus: return left + right;
          case SyntaxKind.Dash: return left - right;
          case SyntaxKind.Star: return left * right;
          case SyntaxKind.Slash: return left / right;
          default:
            throw new Exception($"Unexpected binary operator {bin.Op.Kind}.");
        }
      }

      if (node is ParenExprNode par)
        return EvalExpr(par.Expr);

      throw new Exception($"Unexpected node {node.Kind}.");
    }
  }

}

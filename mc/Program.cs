using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk
{
  internal static class Program
  {
    private static void Main() {
      var showTree = false;

      Console.WriteLine(" \x1b[42m\x1b[30m   Minsk Programming language <3   \x1b[0m\n");

      while (true) {
        Console.Write("\x1b[36m𐡸\x1b[0m ");
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
        var binder = new Binder();
        var boundExpr = binder.BindExpr(ast.Root);

        var diags = ast.Diags.Concat(binder.Diags).ToArray();

        if (showTree)
          PrettyPrint(ast.Root);

        if (diags.Any()) {
          Console.ForegroundColor = ConsoleColor.Red;

          foreach (var diag in diags) {
            Console.WriteLine($"  {diag}  ");
          }

          Console.ResetColor();
        }
        else {
          var eval = new Eval(boundExpr);
          var res = eval.Evaluate();

          Console.WriteLine($"\x1b[2;36m𐡷\x1b[2;37m {res}\x1b[0m");
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
}

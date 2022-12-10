using Minsk.CodeAnalysis;
using Minsk.CodeAnalysis.Binding;
using Minsk.CodeAnalysis.Syntax;

namespace Minsk
{
  internal static class Program
  {
    private static void Main() {
      var showTree = false;
      var vars = new Dictionary<VarSymbol, object?>();

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
        var comp = new Compilation(ast);
        var res = comp.Evaluate(vars);

        var diags = res.Diags;

        if (showTree)
          PrettyPrint(ast.Root);

        if (!diags.Any()) {
          Console.WriteLine($"\x1b[2;36m𐡷\x1b[2;37m {res.Val}\x1b[0m");
        }
        else {
          foreach (var diag in diags) {
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"  {diag}");
            Console.ResetColor();

            var prefix = line.Substring(0, diag.Span.Start);
            var error = line.Substring(diag.Span.Start, diag.Span.Len);
            var suffix = line.Substring(diag.Span.End);

            Console.Write($"  ╰─ {prefix}");

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write(error);
            Console.ResetColor();

            Console.Write(suffix);
            Console.WriteLine();
          }

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
}

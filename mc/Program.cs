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
        Console.Write("\x1b[36m⮳\x1b[0m ");
        var code = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(code))
          return;

        if (code == "#showTree") {
          showTree = !showTree;
          Console.ForegroundColor = showTree ? ConsoleColor.Green : ConsoleColor.Yellow;
          Console.WriteLine("  🌴️ " + (showTree ? "Showing" : "Not showing") + " parse trees." );
          Console.ResetColor();
          continue;
        }
        else if (code == "#cls") {
          Console.Clear();
          continue;
        }

        var ast = SyntaxTree.Parse(code);
        var comp = new Compilation(ast);
        var res = comp.Evaluate(vars);

        var diags = res.Diags;

        if (showTree)
          ast.Root.WriteTo(Console.Out);

        if (!diags.Any()) {
          Console.WriteLine($"\x1b[2;36m⮱\x1b[2;37m {res.Val}\x1b[0m");
        }
        else {
          var text = ast.Text;

          foreach (var diag in diags) {
            var lineIndex = text.GetLineIndex(diag.Span.Start);
            var lineNumber = lineIndex + 1;
            var ch = diag.Span.Start - text.Lines[lineIndex].Start + 1;

            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"({lineNumber}, {ch}): ");
            Console.WriteLine(diag);
            Console.ResetColor();

            var prefix = code.Substring(0, diag.Span.Start);
            var error = code.Substring(diag.Span.Start, diag.Span.Len);
            var suffix = code.Substring(diag.Span.End);

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
  }
}

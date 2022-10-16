﻿using Minsk.CodeAnalysis;


namespace Minsk
{
  internal static class Program
  {
    private static void Main() {
      var showTree = false;

      Console.Write(" ");
      Console.BackgroundColor = ConsoleColor.DarkGreen;
      Console.ForegroundColor = ConsoleColor.Black;
      Console.Write("   Minsk Programming language <3   ");
      Console.ResetColor();
      Console.WriteLine("\n");

      while (true) {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write("•> ");
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
          Console.Write("<• ");
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
}

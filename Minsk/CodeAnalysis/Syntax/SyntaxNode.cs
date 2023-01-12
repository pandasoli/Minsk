using System.Reflection;
using Minsk.CodeAnalysis.Text;

namespace Minsk.CodeAnalysis.Syntax
{
  public abstract class SyntaxNode
  {
    public abstract SyntaxKind Kind { get; }

    public virtual TextSpan Span
    {
      get
      {
        var first = GetChildren().First().Span;
        var last = GetChildren().Last().Span;
        return TextSpan.FromBounds(first.Start, last.End);
      }
    }

    public IEnumerable<SyntaxNode> GetChildren()
    {
      var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

      foreach (var prop in props)
      {
        if (typeof(SyntaxNode).IsAssignableFrom(prop.PropertyType))
        {
          var child = (SyntaxNode)prop.GetValue(this);
          yield return child;
        }
        else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(prop.PropertyType))
        {
          var children = (IEnumerable<SyntaxNode>)prop.GetValue(this);

          foreach (var child in children)
            yield return child;
        }
      }
    }

    public void WriteTo(TextWriter writer)
    {
      PrettyPrint(writer, this);
    }

    private static void PrettyPrint(TextWriter writer, SyntaxNode node, string indent = "", bool isLast = true) {
      var marker = isLast ? "╰─ " : "├─ ";

      Console.ForegroundColor = ConsoleColor.DarkGray;
      writer.Write(indent);
      writer.Write(marker);
      Console.ForegroundColor = ConsoleColor.Blue;
      writer.Write(node.Kind);
      Console.ResetColor();

      if (node is SyntaxToken tok && tok.Val != null)
      {
        writer.Write(": ");
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        writer.Write(tok.Val);

        Console.ResetColor();
      }

      Console.WriteLine();

      indent += isLast ? "   " : "│  ";
      var last = node.GetChildren().LastOrDefault();

      foreach (var child in node.GetChildren())
        PrettyPrint(writer, child, indent, child == last);
    }

    public override string ToString() {
      using (var writer = new StringWriter()) {
        WriteTo(writer);
        return writer.ToString();
      }
    }
  }

}

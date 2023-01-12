using System.Reflection;

namespace Minsk.CodeAnalysis.Syntax
{
  public abstract class SyntaxNode {
    public abstract SyntaxKind Kind { get; }

    public IEnumerable<SyntaxNode> GetChildren() {
      var props = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

      foreach (var prop in props) {
        if (typeof(SyntaxNode).IsAssignableFrom(prop.PropertyType)) {
          var child = (SyntaxNode) prop.GetValue(this);
          yield return child;
        }
        else if (typeof(IEnumerable<SyntaxNode>).IsAssignableFrom(prop.PropertyType)) {
          var children = (IEnumerable<SyntaxNode>) prop.GetValue(this);

          foreach (var child in children)
            yield return child;
        }
      }
    }
  }

}

namespace Minsk.CodeAnalysis
{
  public sealed class VarSymbol {
    internal VarSymbol(string name, Type type) {
      Name = name;
      Type = type;
    }

    public string Name { get; }
    public Type Type { get; }
  }
}

using System.Collections.Generic;
using System.Linq;
using Minsk.CodeAnalysis.Syntax;
using Xunit;

namespace Minsk.Tests.CodeAnalysis.Syntax
{
  public class LexerTests
  {
    [Theory]
    [MemberData(nameof(GetTokensData))]
    public void Lexer_Lexs_Token(SyntaxKind kind, string text) {
      var tokens = SyntaxTree.ParseTokens(text);

      var token = Assert.Single(tokens);
      Assert.Equal(kind, token.Kind);
      Assert.Equal(text, token.Text);
    }

    [Theory]
    [MemberData(nameof(GetTokenPairsData))]
    public void Lexer_Lexs_TokenPairs(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text) {
      var text = t1Text + t2Text;
      var tokens = SyntaxTree.ParseTokens(text).ToArray();

      Assert.Equal(2, tokens.Length);

      Assert.Equal(tokens[0].Kind, t1Kind);
      Assert.Equal(tokens[0].Text, t1Text);

      Assert.Equal(tokens[1].Kind, t2Kind);
      Assert.Equal(tokens[1].Text, t2Text);
    }

    [Theory]
    [MemberData(nameof(GetTokenPairsWithSepData))]
    public void Lexer_Lexs_TokenPairs_WithSeps(SyntaxKind t1Kind, string t1Text, SyntaxKind sepKind, string sepText, SyntaxKind t2Kind, string t2Text) {
      var text = t1Text + sepText + t2Text;
      var tokens = SyntaxTree.ParseTokens(text).ToArray();

      Assert.Equal(3, tokens.Length);

      Assert.Equal(tokens[0].Kind, t1Kind);
      Assert.Equal(tokens[0].Text, t1Text);

      Assert.Equal(tokens[1].Kind, sepKind);
      Assert.Equal(tokens[1].Text, sepText);

      Assert.Equal(tokens[2].Kind, t2Kind);
      Assert.Equal(tokens[2].Text, t2Text);
    }

    public static IEnumerable<object[]> GetTokensData() {
      foreach (var tk in GetTokens().Concat(GetSeps()))
        yield return new object[] { tk.kind, tk.text };
    }

    public static IEnumerable<object[]> GetTokenPairsData() {
      foreach (var tk in GetTokenPairs())
        yield return new object[] { tk.t1Kind, tk.t1Text, tk.t2Kind, tk.t2Text };
    }

    public static IEnumerable<object[]> GetTokenPairsWithSepData() {
      foreach (var tk in GetTokenPairsWithSeparator())
        yield return new object[] { tk.t1Kind, tk.t1Text, tk.sepKind, tk.sepText, tk.t2Kind, tk.t2Text };
    }

    private static IEnumerable<(SyntaxKind kind, string text)> GetTokens() {
      return new[] {
        (SyntaxKind.NumberTk, "1"),
        (SyntaxKind.NumberTk, "123"),

        (SyntaxKind.IdTk, "a"),
        (SyntaxKind.IdTk, "abc"),

        (SyntaxKind.PlusTk, "+"),
        (SyntaxKind.DashTk, "-"),
        (SyntaxKind.BangTk, "!"),
        (SyntaxKind.StarTk, "*"),
        (SyntaxKind.SlashTk, "/"),
        (SyntaxKind.OpenParenTk, "("),
        (SyntaxKind.CloseParenTk, ")"),

        (SyntaxKind.FalseKw, "false"),
        (SyntaxKind.TrueKw, "true"),

        (SyntaxKind.ApsdApsdTk, "&&"),
        (SyntaxKind.PipePipeTk, "||"),
        (SyntaxKind.EqsEqsTk, "=="),
        (SyntaxKind.EqsTk, "="),
        (SyntaxKind.BangEqsTk, "!=")
      };
    }
    private static IEnumerable<(SyntaxKind kind, string text)> GetSeps() {
      return new[] {
        (SyntaxKind.WhiteSpaceTk, " "),
        (SyntaxKind.WhiteSpaceTk, "  "),
        (SyntaxKind.WhiteSpaceTk, "\r"),
        (SyntaxKind.WhiteSpaceTk, "\n"),
        (SyntaxKind.WhiteSpaceTk, "\r\n")
      };
    }

    private static bool RequiresSep(SyntaxKind t1Kind, SyntaxKind t2Kind) {
      var t1Kw = t1Kind.ToString().EndsWith("Kw");
      var t2Kw = t2Kind.ToString().EndsWith("Kw");

      if (t1Kind == SyntaxKind.IdTk && t2Kind == SyntaxKind.IdTk)
        return true;

      if (t1Kw && t2Kw)
        return true;

      if (t1Kw && t2Kind == SyntaxKind.IdTk) return true;
      if (t1Kind == SyntaxKind.IdTk && t2Kw) return true;

      if (t1Kind == SyntaxKind.NumberTk && t2Kind == SyntaxKind.NumberTk)
        return true;

      if (t1Kind == SyntaxKind.BangTk && t2Kind == SyntaxKind.EqsTk   ) return true;
      if (t1Kind == SyntaxKind.BangTk && t2Kind == SyntaxKind.EqsEqsTk) return true;

      if (t1Kind == SyntaxKind.EqsTk && t2Kind == SyntaxKind.EqsTk   ) return true;
      if (t1Kind == SyntaxKind.EqsTk && t2Kind == SyntaxKind.EqsEqsTk) return true;

      return false;
    }

    private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind t2Kind, string t2Text)> GetTokenPairs() {
      foreach (var t1 in GetTokens()) {
        foreach (var t2 in GetTokens()) {
          if (!RequiresSep(t1.kind, t2.kind))
            yield return (t1.kind, t1.text, t2.kind, t2.text);
        }
      }
    }

    private static IEnumerable<(SyntaxKind t1Kind, string t1Text, SyntaxKind sepKind, string sepText, SyntaxKind t2Kind, string t2Text)> GetTokenPairsWithSeparator() {
      foreach (var t1 in GetTokens()) {
        foreach (var t2 in GetTokens()) {
          if (RequiresSep(t1.kind, t2.kind)) {
            foreach (var sep in GetSeps())
              yield return (t1.kind, t1.text, sep.kind, sep.text, t2.kind, t2.text);
          }
        }
      }
    }
  }
}

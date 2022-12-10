using System.Collections.Generic;
using Minsk.CodeAnalysis.Syntax;
using Xunit;


namespace Minsk.Tests.CodeAnalysis.Syntax
{
  public class ParserTests
  {
    [Theory]
    [MemberData(nameof(GetBinOpPairsData))]
    public void Parser_BinExpr_HonorsPreces(SyntaxKind op1, SyntaxKind op2) {
      var op1Prece = SyntaxFacts.GetBinOpPrece(op1);
      var op1Text = SyntaxFacts.GetText(op1);

      var op2Prece = SyntaxFacts.GetBinOpPrece(op2);
      var op2Text = SyntaxFacts.GetText(op2);

      var text = $"a {op1Text} b {op2Text} c";
      var expr = SyntaxTree.Parse(text).Root;

      if (op1Prece >= op2Prece) {
        /*
              op2
             /  \
           op1   c
           / \
          a  b
        */

        using (var e = new AssertingEnumerator(expr)) {
          e.AssertNode(SyntaxKind.BinExpr);

          e.AssertNode(SyntaxKind.BinExpr);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "a");

          e.AssertToken(op1, op1Text);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "b");

          e.AssertToken(op2, op2Text);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "c");
        }
      }
      else {
        /*
            op1
           /  \
          a   op2
              / \
             b   c
        */

        using (var e = new AssertingEnumerator(expr)) {
          e.AssertNode(SyntaxKind.BinExpr);

          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "a");

          e.AssertToken(op1, op1Text);
          e.AssertNode(SyntaxKind.BinExpr);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "b");

          e.AssertToken(op2, op2Text);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "c");
        }
      }
    }

    [Theory]
    [MemberData(nameof(GetUnaryOpPairsData))]
    public void Parser_UnaryExpr_HonorsPreces(SyntaxKind unaryKind, SyntaxKind binKind) {
      var unaryPrece = SyntaxFacts.GetUnaryOpPrece(unaryKind);
      var unaryText = SyntaxFacts.GetText(unaryKind);

      var binPrece = SyntaxFacts.GetBinOpPrece(binKind);
      var binText = SyntaxFacts.GetText(binKind);

      var text = $"{unaryText} a {binText} b";
      var expr = SyntaxTree.Parse(text).Root;

      if (unaryPrece >= binPrece) {
        /*
              bin
             /  \
          unary  b
           |
           a
        */

        using (var e = new AssertingEnumerator(expr)) {
          e.AssertNode(SyntaxKind.BinExpr);
          e.AssertNode(SyntaxKind.UnaryExpr);
          e.AssertToken(unaryKind, unaryText);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "a");
          e.AssertToken(binKind, binText);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "b");
        }
      }
      else {
        /*
           unary
            |
           bin
           / \
          a   b
        */

        using (var e = new AssertingEnumerator(expr)) {
          e.AssertNode(SyntaxKind.UnaryExpr);
          e.AssertToken(unaryKind, unaryText);
          e.AssertNode(SyntaxKind.BinExpr);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "a");
          e.AssertToken(binKind, binText);
          e.AssertNode(SyntaxKind.NameExpr);
          e.AssertToken(SyntaxKind.IdTk, "b");
        }
      }
    }

    private static IEnumerable<object[]> GetBinOpPairsData() {
      foreach (var op1 in SyntaxFacts.GetBinOpKinds())
        foreach (var op2 in SyntaxFacts.GetBinOpKinds())
          yield return new object[] { op1, op2 };
    }

    private static IEnumerable<object[]> GetUnaryOpPairsData() {
      foreach (var unary in SyntaxFacts.GetUnaryOpKinds())
        foreach (var bin in SyntaxFacts.GetBinOpKinds())
          yield return new object[] { unary, bin };
    }
  }
}

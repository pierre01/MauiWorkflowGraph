using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Services
{
    public class RulesService : IRulesService
    {
        // Matches simple lambdas like:
        //   x => x + 1
        //   (a, b) => a * b
        private static readonly Regex LambdaRegex = new Regex(
            @"^\s*                                   # optional leading whitespace
      (?:                                   # either
        [a-zA-Z_]\w*                        #   single identifier
      |                                     # or
        \(\s*                               #   opening paren + optional whitespace
          [a-zA-Z_]\w*                      #   first identifier
          (?:\s*,\s*[a-zA-Z_]\w*)*          #   optional , identifier…
        \s*\)                               #   closing paren
      )                                     # end group
      \s*=>\s*                              # arrow with optional whitespace
      .+                                    # one-or-more chars (the body)
      $",
            RegexOptions.Compiled
            | RegexOptions.IgnorePatternWhitespace
        );

        /// <summary>
        /// Validates if the input string is a valid lambda expression.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>true if valid</returns>
        public bool IsValidExpression(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;
            return LambdaRegex.IsMatch(input);
        }

        public void Tests()
        {
            var tests = new[]
            {
                    "x => x + 1",
                    "(a, b) => a * b",
                    "   (customer, order) => customer.Amount > 100",
                    "(x) => { return x + 1; }",  // block lambdas will still match, but are not fully validated
                    "=> x",                     // invalid
                    "(x,y,z)=> x+y+z"
                };

            foreach (var t in tests)
            {
                Console.WriteLine($"\"{t}\" ⇒ {IsValidExpression(t)}");
            }
        }
    }
}

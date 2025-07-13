using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Services
{
    public class RulesService : IRulesService
    {
        //Create a dictionary of variable names and their values
        public Dictionary<string, object> Variables { get; set; } = new Dictionary<string, object>();

        public RulesService()
        {
            Variables.Add("x", 10.00);
            Variables.Add("y", 12.00);
            Variables.Add("z", 21.00);
            Variables.Add("a", 10);
            Variables.Add("b", 10);
            Variables.Add("c", 10);
            Variables.Add("userId", "John Smith");
            Variables.Add("customer", new {Amount=1200.00});
            Variables.Add("order", 100.00);
        }

        private static readonly Regex LambdaParamRegex = new Regex(
        @"^\s*                                      # leading whitespace
          (?:
            (?<single>[A-Za-z_]\w*)                # single parameter e.g. x
          |
            \(\s*
              (?<param>[A-Za-z_]\w*)               # first param inside ()
              (?:\s*,\s*(?<param>[A-Za-z_]\w*))*    # additional ,param
            \s*\)
          )
          \s*=>                                    # arrow
          .+                                       # body
          $",
        RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        // Matches simple lambdas like:
        //   x => x + 1
        //   (a, b) => a * b
        private static readonly Regex LambdaRegex = new Regex(
            @"^\s*                          # optional leading whitespace
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

        public string GetResult(string expression)
        {
            Tests();
            return "Result from expression";
        }

        /// <summary>
        /// Validates if the input string is a valid lambda expression.
        /// </summary>
        /// <param name="input"></param>
        /// <returns>(true, "") if valid, (false, errorMessage) if not</returns>
        public (bool, string) IsValidExpression(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return (false, "Input is null or whitespace.");
            var formatCorrect = LambdaRegex.IsMatch(input);
            if (!formatCorrect)
                return (false, "Expression format is incorrect.");
            var parameters = ExtractParameters(input);
            if (parameters.Count == 0)
                return (false, "No parameters found in expression.");
            // Check if all parameters are valid identifiers found in variables
            foreach (var param in parameters)
            {
                if (!Variables.ContainsKey(param))
                {
                    Debug.WriteLine($"Parameter '{param}' is not defined in variables.");
                    return (false, $"Parameter '{param}' is not defined in variables.");
                }
            }
            // If we reach here, the expression is valid
            Debug.WriteLine($"Expression '{input}' is valid with parameters: {string.Join(", ", parameters)}");
            return (true, "");
        }

        public static IList<string> ExtractParameters(string lambda)
        {
            var match = LambdaParamRegex.Match(lambda);
            if (!match.Success)
                return Array.Empty<string>();

            var vars = new List<string>();

            // If it was a single identifier, grab that
            if (match.Groups["single"].Success)
            {
                vars.Add(match.Groups["single"].Value);
            }
            else
            {
                // Otherwise grab all captures of the "param" group
                foreach (Capture cap in match.Groups["param"].Captures)
                    vars.Add(cap.Value);
            }

            return vars;
        }

        public void Tests()
        {
            var tests = new[]
            {
                    "x => x + 1",
                    "(a, b) => a * b",
                    "(a, b, userId) => a * b + userId.Length",
                    "   (customer, order) => customer.Amount > 100",
                    "(x) => { return x + 1; }",  // block lambdas will still match, but are not fully validated
                    "=> x",                     // invalid
                    "(x,y,z)=> x+y+z"
                };


            foreach (var ex in tests)
            {
                if (IsValidExpression(ex).Item1)
                {
                    var parameters = RulesService.ExtractParameters(ex);
                    Debug.WriteLine($"\"{ex}\" ⇒ [{string.Join(", ", parameters)}]");
                }
                else
                {
                    Debug.WriteLine($"\"{ex}\" ⇒ <invalid>");
                }
            }
        }
    }
}

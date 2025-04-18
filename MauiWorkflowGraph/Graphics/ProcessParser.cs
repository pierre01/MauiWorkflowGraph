using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Graphics;

public static class ProcessParser
{
    public static ProcessNode Parse(string input)
    {
        int i = 0;
        return ParseList();
        
        ProcessNode ParseList()
        {
            var nodes = new List<ProcessNode>();
            Expect('[');
            while (true)
            {
                SkipWhitespace();
                if (Peek() == '(')
                {
                    nodes.Add(ParseParallel());
                }
                else
                {
                    // mot ou prochaine sous-liste
                    if (Peek() == '[') nodes.Add(ParseList());
                    else
                    {
                        // lire nom
                        var sb = new StringBuilder();
                        while (char.IsLetterOrDigit(Peek()))
                        {
                            sb.Append( input[i++] );
                        }
                        nodes.Add(new LeafNode(sb.ToString()));
                    }
                }
                SkipWhitespace();
                if (Peek() == ',') { i++; continue; }
                if (Peek() == ']') { i++; break; }
            }
            // si qu’un seul enfant, on retourne directement son nœud
            if (nodes.Count == 1) return nodes[0];
            return new SequenceNode(nodes);
        }
        
        ProcessNode ParseParallel()
        {
            // syntaxe: ( slice1 , slice2 , ... )
            Expect('(');
            var branches = new List<ProcessNode>();
            while (true)
            {
                SkipWhitespace();
                if (Peek() == '[')
                    branches.Add(ParseList());
                else if (Peek() == '(')
                    branches.Add(ParseParallel());
                else
                {
                    var sb = new StringBuilder();
                    while (char.IsLetterOrDigit(Peek()))
                        sb.Append(input[i++]);
                    branches.Add(new LeafNode(sb.ToString()));
                }
                SkipWhitespace();
                if (Peek() == ',') { i++; continue; }
                if (Peek() == ')') { i++; break; }
            }
            return new ParallelNode(branches);
        }
        
        char Peek() => i < input.Length ? input[i] : '\0';
        void Expect(char c) { if (Peek()!=c) throw new Exception($"Expected '{c}' at {i}"); i++; }
        void SkipWhitespace() { while (char.IsWhiteSpace(Peek())) i++; }
    }
}


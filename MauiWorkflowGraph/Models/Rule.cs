using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Models;

public class Rule
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Expression { get; set; }
    public string Result { get; set; }
    public Rule(string id, string name, string description, string expression, string result)
    {
        Id = id;
        Name = name;
        Description = description;
        Expression = expression;
        Result = result;
    }
}

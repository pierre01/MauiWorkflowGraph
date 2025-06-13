using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Models;

/// <summary>
/// This is the process to be executed... Can be a Rule, a function, etc.
/// 
/// </summary>
public class FlowProcess
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string Expression { get; set; }
    public string Result { get; set; }
    
    public FlowProcess(string name, string description, string expression, string result)
    {
        Name = name;
        Description = description;
        Expression = expression;
        Result = result;
    }


    private Random _random = new Random();
    
    /// <summary>
    /// Execute process or rule.
    /// </summary>
    /// <returns> true if successful</returns>
    public virtual async Task<bool> Execute()
    {
        await Task.Delay(_random.Next(600, 2000)); // wait for 0.6 to 2 seconds
        // Simulate some processing
        Console.WriteLine($"Executing {Name} with expression: {Expression}");
        // Here you would add the actual logic to execute the process
        return true; // Return true if successful, false otherwise
    }
}

namespace MauiWorkflowGraph.Services
{
    public interface IRulesService
    {
        bool IsValidExpression(string input);
        string GetResult(string expression);
    }
}

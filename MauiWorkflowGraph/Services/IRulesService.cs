namespace MauiWorkflowGraph.Services
{
    public interface IRulesService
    {
        (bool IsValid,string Error) IsValidExpression(string input);
        string GetResult(string expression);
    }
}

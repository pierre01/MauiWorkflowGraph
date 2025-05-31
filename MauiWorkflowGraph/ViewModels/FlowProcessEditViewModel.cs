using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWorkflowGraph.Models;
using System.ComponentModel.DataAnnotations;

namespace MauiWorkflowGraph.ViewModels;

public partial class FlowProcessEditViewModel : ObservableValidator
{
    private readonly FlowProcess _process;


    public FlowProcessEditViewModel(FlowProcess processRule)
    {
        _process = processRule;
        Name = processRule.Name;
        Description = processRule.Description;
        Expression = processRule.Expression;
        Result = processRule.Result;
        IsEditable = true;
    }

    [ObservableProperty]
    [Required()]
    [MinLength(3,ErrorMessage ="Name must have a minimum of 3 Characters")]
    [MaxLength(100)]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Name { get; set; } 

    [ObservableProperty]
    [MaxLength(250)]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Description { get; set; }  

    [ObservableProperty]
    [MaxLength(250)]
    [CustomValidation(typeof(FlowProcessEditViewModel), nameof(ValidateExpression))]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial string Expression { get; set; } 
    
    [ObservableProperty]
    public partial string Result { get; set; } 
    
    [ObservableProperty]
    public partial string Errors { get; set; } 
    
    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(SaveCommand))]
    public partial bool IsEditable { get; set; }

    /// <summary>
    /// Custom validation for the expression property.
    /// </summary>
    /// <param name="expression"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public static ValidationResult ValidateExpression(string expression, ValidationContext context)
    {
        FlowProcessEditViewModel instance = (FlowProcessEditViewModel)context.ObjectInstance;
        bool isValid = true; //instance.service.Validate(expression);

        if (isValid)
        {
            return ValidationResult.Success;
        }

        return new("The expression is not valid");
    }


    [RelayCommand(CanExecute = nameof(HasChanged))]
    private void Save()
    {

        // Validate
        ValidateAllProperties();
        if (HasErrors)
        {
            Errors = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));
            return;
        }

        // Save the process
        _process.Name = Name;
        _process.Description = Description;
        _process.Expression = Expression;
    }

    /// <summary>
    /// Check if the process has been edited
    /// </summary>
    /// <returns>true if edited</returns>
    public bool HasChanged()
    {
        if (!IsEditable)
        {
            return false;
        }
        // check if the process has changed
        if (_process.Name != Name || _process.Description != Description || _process.Expression != Expression)
        {
            return true;
        }
        return false;

    }
}

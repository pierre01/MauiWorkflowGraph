using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiWorkflowGraph.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.ViewModels;

public partial class FlowProcessEditViewModel : ObservableValidator
{
    private readonly FlowProcess _process;

    // create an event to notify when we are closing the view
    public event EventHandler Closing;

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
    public partial string Name { get; set; } 

    [ObservableProperty]
    [MaxLength(250)]
    public partial string Description { get; set; }  

    [ObservableProperty]
    [MaxLength(250)]
    [CustomValidation(typeof(FlowProcessEditViewModel), nameof(ValidateExpression))]
    public partial string Expression { get; set; } 
    
    [ObservableProperty]
    public partial string Result { get; set; } 
    
    [ObservableProperty]
    public partial string Errors { get; set; } 
    
    [ObservableProperty]
    public partial bool IsEditable { get; set; } 

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

    [RelayCommand]
    private async Task Save()
    {
        // Validate
        ValidateAllProperties();
        if (HasErrors)
        {
            //string.Join(" ", GetErrors(nameof(Name)).Select(e => e.ErrorMessage));
            Errors = string.Join(Environment.NewLine, GetErrors().Select(e => e.ErrorMessage));

            return;
        }
        // Save the process
        _process.Name = Name;
        _process.Description = Description;
        _process.Expression = Expression;
        _process.Result = Result;

    }

    /// <summary>
    /// Check if the process has been edited
    /// </summary>
    /// <returns>true if edited</returns>
    public bool HasChanged()
    {
        // check if the process has changed
        if (_process.Name != Name || _process.Description != Description || _process.Expression != Expression)
        {
            return true;
        }
        return false;

    }
}

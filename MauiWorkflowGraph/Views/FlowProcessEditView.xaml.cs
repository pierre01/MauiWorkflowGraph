using CommunityToolkit.Maui.Core.Platform;
using MauiWorkflowGraph.ViewModels;
using System.Runtime.Versioning;

namespace MauiWorkflowGraph.Views;
public partial class FlowProcessEditView : ContentView
{
    public event EventHandler Closing;

    public FlowProcessEditView()
    {
        InitializeComponent();
        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is FlowProcessEditViewModel vm)
        {
            vm.IsEditable = !_isRunning;
            NameEntry.Focus();
        }
    }

    private bool _isRunning = false;
    public bool IsRunning
    {
        get
        {
            return _isRunning;
        }
        set
        {
            _isRunning = value;
            var vm = (FlowProcessEditViewModel)BindingContext;
            if (vm != null)
            {
                    vm.IsEditable = !_isRunning;
            }
        }
    }

    /// <summary>
    /// Hides the soft keyboard associated with the current input field.
    /// </summary>
    /// <remarks>This method is typically used to programmatically dismiss the soft keyboard when it is no
    /// longer needed, such as after user input is complete.</remarks>
    public void HideSoftKeyboard()
    {
        NameEntry.HideKeyboardAsync(CancellationToken.None);
    }

    /// <summary>
    /// Save button was clicked, it is expected to occur after the Save command is executed
    /// </summary>
    private void OnSavedClicked(object sender, EventArgs e)
    {
        var vm = (FlowProcessEditViewModel)BindingContext;
        if (vm != null && !vm.HasErrors) 
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

    }

    private void OnCloseClicked(object sender, EventArgs e)
    {
        Closing?.Invoke(this, EventArgs.Empty);
    }
}
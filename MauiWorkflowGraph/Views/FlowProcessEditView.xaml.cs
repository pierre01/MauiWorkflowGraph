using CommunityToolkit.Maui.Core.Platform;
using MauiWorkflowGraph.ViewModels;

namespace MauiWorkflowGraph.Views;
public partial class FlowProcessEditView : ContentView
{
    public event EventHandler Closing;

    public FlowProcessEditView()
    {
        InitializeComponent();
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
}
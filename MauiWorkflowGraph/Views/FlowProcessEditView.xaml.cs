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

    public void HideSoftKeyboard()
    {
        NameEntry.HideKeyboardAsync(CancellationToken.None);
    }

    private void OnSavedClicked(object sender, EventArgs e)
    {
        var vm = (FlowProcessEditViewModel)BindingContext;
        if (vm != null && !vm.HasErrors) 
        {
            Closing?.Invoke(this, EventArgs.Empty);
        }

    }
}
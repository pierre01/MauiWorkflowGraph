using CommunityToolkit.Maui.Core.Platform;

namespace MauiWorkflowGraph.Views;
public partial class FlowProcessEditView : ContentView
{
	public FlowProcessEditView()
	{
		InitializeComponent();
	}

	public void HideSoftKeyboard()
	{
		NameEntry.HideKeyboardAsync(CancellationToken.None);
	}

}
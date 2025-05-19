using CommunityToolkit.Maui.Core.Platform;
namespace MauiWorkflowGraph.Views;
public partial class ProcessBottomView : ContentView
{
	public ProcessBottomView()
	{
		InitializeComponent();
	}

	public void HideSoftKeyboard()
	{
		NameEntry.HideKeyboardAsync(CancellationToken.None);
	}

}
using MauiWorkflowGraph.Graphics;

namespace MauiWorkflowGraph;

public partial class MainPage : ContentPage
{

    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Exemple d’entrée
        string input = "[A,B,C,([D,C],[C,(C,[A,D])],[B,D,A]),D]";
        var drawable = (GraphRenderer) myGraphicsView.Drawable;
        drawable.UpdateGraph(input);
        myGraphicsView.Invalidate();
    }

}

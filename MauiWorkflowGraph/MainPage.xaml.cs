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
        string input = "[Adonis,Brigitte,Charles,([Denis,Chrid],[Claude,(Colby,[Amanda,Dillan])],[Brad,Dick,Angel]),Dorothy]";
        var drawable = (GraphRenderer) myGraphicsView.Drawable;
        drawable.UpdateGraph(input);
        myGraphicsView.Invalidate();
    }

}

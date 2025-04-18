using MauiWorkflowGraph.Graphics;

namespace MauiWorkflowGraph;

public partial class MainPage : ContentPage
{
    GraphRenderer _renderer;
    public MainPage()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Exemple d’entrée
        string input = "[Adonis,(Arold,Bernard),Brigitte,Charles,([Denis,Chris],[Claude,(Colby,[Amanda,Dillan])],[Brad,Dick,Angel]),Dorothy]";
        //string input = "[A,B,C,([D,C],[C,(C,[A,D])],[B,D,A]),D]";
        _renderer = (GraphRenderer)myGraphicsView.Drawable;
        _renderer.UpdateGraph(input);
        myGraphicsView.Invalidate();
    }

    private void myGraphicsView_StartInteraction(object sender, TouchEventArgs e)
    {
        // On s'intéresse au relâché du doigt/souris
        if (e.Touches.Length > 0)
        {
            var t = e.Touches.First();
            // point en coordonnées de la GraphicsView
            var tapped = _renderer.HitTest(new PointF(t.X, t.Y));
            if (tapped is LeafNode leaf)
            {
                // Action personnalisée, p.ex. afficher une alerte
                DisplayAlert("Processus sélectionné", $"Vous avez cliqué sur : {leaf.Name}", "OK");
            }
        }
    }
}

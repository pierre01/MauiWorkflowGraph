using MauiWorkflowGraph.Graphics;

namespace MauiWorkflowGraph;

public partial class MainPage : ContentPage
{
    GraphRenderer _renderer;



    public MainPage()
    {
        InitializeComponent();
                // Exemple d’entrée
        //string input = "[Adonis,(Arold,Bernard),Brigitte,Charles,([Denis,Chris],[Claude,(Colby,[Amanda,Dillan])],[Brad,Dick,Angel]),Dorothy]";
        string input = "[A1,B1,C1,([D2,C2],[C3,(C4,[A2,D3])],[B2,D4,A3]),D5]";
        _renderer = new GraphRenderer();
        _renderer.UpdateGraph(input);
        myGraphicsView.Drawable = _renderer;

        BindingContext   = _renderer;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();

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

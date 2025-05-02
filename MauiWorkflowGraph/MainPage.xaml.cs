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
        string input = "[Rule1,(Rule2,Rule3),Rule4,Rule5,([Rule6,Rule7],[Rule8,(Rule9,[Rule10,Rule11])],[Rule12,Rule13,Rule14]),Rule15]";
        _renderer = new GraphRenderer();
        _renderer.UpdateGraph(input);
        myGraphicsView.Drawable = _renderer;

        BindingContext = _renderer;
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
                leaf.SelectDeselect();
                myGraphicsView.Invalidate();
            }
        }
    }

    async void OnStartSimulationClicked(object sender, EventArgs e)
    {
        btnStart.IsEnabled = false;
        await SimulateNode(_renderer.Root);
        btnStart.IsEnabled = true;
        await Task.Delay(2000);
        _renderer.ResetStates();
        myGraphicsView.Invalidate();
    }

    Random _random = new Random();
    // Simulation récursive
    async Task SimulateNode(ProcessNode node)
    {
        switch (node)
        {
            case LeafNode leaf:
                leaf.State = NodeState.Executing;
                myGraphicsView.Invalidate();
                await Task.Delay(_random.Next(600,2000));
                leaf.State = NodeState.Completed;
                myGraphicsView.Invalidate();
                break;

            case SequenceNode seq:
                foreach (var child in seq.Children)
                    await SimulateNode(child);
                break;

            case ParallelNode par:
                // lance toutes les branches en même temps
                var tasks = par.Branches
                                .Select(branch => SimulateNode(branch));
                await Task.WhenAll(tasks);
                break;
        }
    }

}

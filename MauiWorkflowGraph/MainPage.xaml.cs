using MauiWorkflowGraph.Graphics;

namespace MauiWorkflowGraph;

public partial class MainPage : ContentPage
{
    GraphRenderer _renderer;



    public MainPage()
    {
        InitializeComponent();
       
        // Between square bracket are sequential processes (rules), between parenthesis are parallel ones
        // The name is the dictionary index found in FlowProcessManager.cs
        string input = "[Rule1,(Rule2,Rule3),Rule4,Rule5,([Rule6,Rule7],[Rule8,(Rule9,[Rule10,Rule11])],[Rule12,Rule13,Rule14]),Rule15]";
        _renderer = new GraphRenderer();
        _renderer.UpdateGraph(input);
        myGraphicsView.Drawable = _renderer;

        BindingContext = _renderer;
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

    /// <summary>
    /// Start the simulation when the button is clicked.
    /// </summary>
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

    /// <summary>
    /// Simulate node execution.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    async Task SimulateNode(ProcessNode node)
    {
        switch (node)
        {
            case LeafNode leaf:
                leaf.State = NodeState.Executing;
                myGraphicsView.Invalidate();
                await Task.Delay(_random.Next(600,2000)); // wait for 0.6 to 2 seconds
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

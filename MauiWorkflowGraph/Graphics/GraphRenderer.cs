using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiWorkflowGraph.Graphics;

[ObservableObject]
public partial class GraphRenderer : IDrawable
{
    public ProcessNode Root { get; private set; }
    private string _input;
   
    public GraphRenderer()
    {
    }

    public ProcessNode UpdateGraph(string newInput)
    {
        _input = newInput;
        Root = ProcessParser.Parse(_input);
        return Root;
    }

    // Méthode publique pour le hit‑test
    public ProcessNode HitTest(PointF point)
        => Root?.HitTest(point);

    [ObservableProperty]
    public partial float ContentWidth  { get; set; } =1000;
    [ObservableProperty]
    public partial float ContentHeight { get; set; }=1000;


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // mesurer et centrer
        var size = Root.Measure(canvas);
        ContentWidth = size.Width;
        ContentHeight = size.Height;
        Root.Bounds = new RectF(
            dirtyRect.Center.X - size.Width/2,
            dirtyRect.Center.Y - size.Height/2,
            size.Width, size.Height);

        // dessiner
        Root.Draw(canvas);
    }
        /// <summary>
    /// Remet l'état de tous les nœuds (feuilles et conteneurs) à Idle.
    /// </summary>
    public void ResetStates()
    {
        if (Root != null)
            ResetStateRecursive(Root);
    }

    private void ResetStateRecursive(ProcessNode node)
    {
        // 1) réinitialise le nœud courant
        node.State = NodeState.Idle;

        // 2) si c'est un conteneur, descends dans ses enfants
        if (node is SequenceNode seq)
        {
            foreach (var child in seq.Children)
                ResetStateRecursive(child);
        }
        else if (node is ParallelNode par)
        {
            foreach (var branch in par.Branches)
                ResetStateRecursive(branch);
        }
        // LeafNode n'a pas d'enfants, donc on arrête là
    }
}


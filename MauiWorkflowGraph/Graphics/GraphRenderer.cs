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

    public float   Scale  { get; set; } = 1f;
    public PointF  Offset { get; set; } = new PointF(0,0);
   
    public GraphRenderer()
    {
    }

    public ProcessNode UpdateGraph(string newInput)
    {
        _input = newInput;
        Root = ProcessParser.Parse(_input);
        return Root;
    }

    /// <summary>
    /// Retourne le ProcessNode sous le point tapé, en tenant compte du scale et de l’offset.
    /// </summary>
    public ProcessNode HitTest(PointF viewPoint)
    {
        if (Root == null)
            return null;

        // 1. On passe du point en coordonnées de la vue
        //    aux coordonnées logiques avant zoom & pan
        float logicalX = (viewPoint.X - Offset.X) / Scale;
        float logicalY = (viewPoint.Y - Offset.Y) / Scale;
        var logicalPoint = new PointF(logicalX, logicalY);

        // 2. On appelle le hit-test classique
        return Root.HitTest(logicalPoint);
    }


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (Root == null)
            return;
        canvas.SaveState();
        // applique zoom et translation
        canvas.Translate(Offset.X, Offset.Y);
        canvas.Scale(Scale, Scale);

        // mesurer et centrer
        var size = Root.Measure(canvas);

        Root.Bounds = new RectF(
            dirtyRect.Center.X - size.Width/2,
            dirtyRect.Center.Y - size.Height/2,
            size.Width, size.Height);

        // dessiner
        Root.Draw(canvas);

        canvas.RestoreState();
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


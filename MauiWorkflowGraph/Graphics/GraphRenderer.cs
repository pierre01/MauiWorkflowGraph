using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiWorkflowGraph.Graphics;

public partial class GraphRenderer : ObservableObject, IDrawable
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
    /// Returns the ProcessNode under the tapped point, taking scale and offset into account.
    /// </summary>
    public ProcessNode HitTest(PointF viewPoint)
    {
        if (Root == null)
            return null;

        // 1. Convert the point from view coordinates
        //    to logical coordinates before zoom & pan
        float logicalX = (viewPoint.X - Offset.X) / Scale;
        float logicalY = (viewPoint.Y - Offset.Y) / Scale;
        var logicalPoint = new PointF(logicalX, logicalY);

        // 2. Call the standard hit-test
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
    /// Resets the state of all nodes (leaves and containers) to Idle.
    /// </summary>
    public void ResetStates()
    {
        if (Root != null)
            ResetStateRecursive(Root);
    }

    private void ResetStateRecursive(ProcessNode node)
    {
        // 1) reset the current node
        node.State = NodeState.Idle;

        // 2) if it's a container, go down into its children
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
        // LeafNode has no children, so we stop here
    }
}


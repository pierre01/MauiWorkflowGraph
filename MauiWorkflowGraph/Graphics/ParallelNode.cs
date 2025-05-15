namespace MauiWorkflowGraph.Graphics;

/// <summary>
/// Represents a parallel node in the workflow graph.
/// </summary>
public class ParallelNode : ProcessNode
{
    public List<ProcessNode> Branches;
    const float Hpadding = 20, Vpadding = 20, Spacing = 20;

    public ParallelNode(IEnumerable<ProcessNode> branches) => Branches = branches.ToList();

    public override SizeF Measure(ICanvas canvas)
    {
        float totalW = 0, maxH = 0;
        foreach (var b in Branches)
        {
            var s = b.Measure(canvas);
            totalW += s.Width + Spacing;
            maxH = MathF.Max(maxH, s.Height);
        }
        totalW -= Spacing;
        return new SizeF(totalW + 2 * Hpadding, maxH + 2 * Vpadding);
    }

    public override void Draw(ICanvas canvas)
    {

        // Boîte : tracer seulement top et bottom
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 4;
        canvas.DrawLine(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y);
        canvas.StrokeSize = 2;
        canvas.DrawLine(Bounds.X, Bounds.Bottom, Bounds.Right, Bounds.Bottom);

        float x = Bounds.X + Hpadding;
        float topY = Bounds.Y + Vpadding;
        float bottomY = Bounds.Bottom;

        foreach (var b in Branches)
        {
            var size = b.Measure(canvas);

            // <-- Aligner chaque branche sur le haut du conteneur
            b.Bounds = new RectF(x, topY, size.Width, size.Height);
            b.Draw(canvas);

            // Flèche depuis le haut du conteneur vers la branche
            float arrowX = b.Bounds.Center.X;
            canvas.DrawLine(arrowX, Bounds.Y, arrowX, b.Bounds.Y);
            canvas.FillColor = Colors.LightGray;
            canvas.SaveState();
            canvas.Translate(arrowX, b.Bounds.Y);
            PathF path = new PathF();
            path.MoveTo(0, 0);
            path.LineTo(-4, -6);
            path.LineTo(4, -6);
            path.Close();
            canvas.DrawPath(path);

            canvas.RestoreState();

            // Flèche de la fin de la branche jusqu’au bas du conteneur
            float branchBottomX = b.Bounds.Center.X;
            float branchBottomY = b.Bounds.Bottom;
            canvas.DrawLine(branchBottomX, branchBottomY, branchBottomX, bottomY);

            x += size.Width + Spacing;
        }
    }
    public override ProcessNode HitTest(PointF point)
    {
        foreach (var b in Branches)
        {
            var hit = b.HitTest(point);
            if (hit != null)
                return hit;
        }
        return null;
    }
}

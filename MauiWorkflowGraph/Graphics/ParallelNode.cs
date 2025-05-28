namespace MauiWorkflowGraph.Graphics;

/// <summary>
/// Represents a parallel node in the workflow graph.
/// </summary>
public class ParallelNode : ProcessNode
{
    public List<ProcessNode> Branches;
    const float Hpadding = 20, Vpadding = 20, Spacing = 20;

    public ParallelNode(IEnumerable<ProcessNode> branches) => Branches = branches.ToList();

    /// <summary>
    /// Measures the size of the parallel node based on its branches.
    /// </summary>
    /// <param name="canvas"></param>
    /// <returns></returns>
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

        // Box: draw only top and bottom
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

            // <-- Align each branch to the top of the container
            b.Bounds = new RectF(x, topY, size.Width, size.Height);
            b.Draw(canvas);

            // Arrow from the top of the container to the branch
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

            // Arrow from the end of the branch to the bottom of the container
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

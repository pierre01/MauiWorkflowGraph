namespace MauiWorkflowGraph.Graphics;

/// <summary>
/// Represents a sequence node in the workflow graph.
/// </summary>
public class SequenceNode : ProcessNode
{
    public List<ProcessNode> Children;
    const float Hpadding = 20, Vpadding = 0, Spacing = 20;

    public SequenceNode(IEnumerable<ProcessNode> children) => Children = children.ToList();

    public override SizeF Measure(ICanvas canvas)
    {
        float totalH = 0, maxW = 0;
        foreach (var c in Children)
        {
            var s = c.Measure(canvas);
            totalH += s.Height + Spacing;
            maxW = MathF.Max(maxW, s.Width);
        }
        totalH -= Spacing;
        return new SizeF(maxW + 2 * Hpadding, totalH + 2 * Vpadding);
    }

    public override void Draw(ICanvas canvas)
    {
        float y = Bounds.Y + Vpadding;
        float cx = Bounds.X + Bounds.Width / 2;

        for (int i = 0; i < Children.Count; i++)
        {
            var c = Children[i];
            var size = c.Measure(canvas);
            c.Bounds = new RectF(cx - size.Width / 2, y, size.Width, size.Height);
            c.Draw(canvas);

            // Flèche vers l’enfant suivant
            if (i < Children.Count - 1)
            {
                float startY = c.Bounds.Bottom;
                float endY = startY + Spacing;
                canvas.StrokeColor = Colors.LightGray;
                canvas.StrokeSize = 2;
                canvas.DrawLine(cx, startY, cx, endY);

                // Tête de flèche vers le bas
                canvas.FillColor = Colors.LightGray;
                canvas.SaveState();
                canvas.Translate(cx, endY);
                PathF path = new PathF();
                path.MoveTo(0, 0);
                path.LineTo(-4, -6);
                path.LineTo(4, -6);
                path.Close();
                canvas.DrawPath(path);
                canvas.RestoreState();
            }

            y += size.Height + Spacing;
        }
    }
    public override ProcessNode HitTest(PointF point)
    {
        // On délègue aux enfants
        foreach (var child in Children)
        {
            var hit = child.HitTest(point);
            if (hit != null)
                return hit;
        }
        return null;
    }
}

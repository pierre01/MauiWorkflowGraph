using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace MauiWorkflowGraph.Graphics;

public abstract class ProcessNode
{
    public RectF Bounds;        // Position et taille calculées
    public abstract SizeF Measure(ICanvas canvas);
    public abstract void Draw(ICanvas canvas);
    protected float FontSize = 14;
    protected IFont Font = new Microsoft.Maui.Graphics.Font("Arial");

}

public class LeafNode : ProcessNode
{
    public string Name;
    public LeafNode(string name) => Name = name;
    public override SizeF Measure(ICanvas canvas)
    {
        canvas.FontColor = Colors.Black;

        var textSize = canvas.GetStringSize(Name, Font, FontSize);

        return new SizeF(textSize.Width + 20, textSize.Height + 16);
    }
    public override void Draw(ICanvas canvas)
    {
        canvas.FillColor = Colors.White;
        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 2;
        canvas.FillRoundedRectangle(Bounds, 4);
        canvas.DrawRoundedRectangle(Bounds, 4);
        // texte centré
        var textSize = canvas.GetStringSize(Name, Font, FontSize);
        float x = Bounds.X + (Bounds.Width - textSize.Width) / 2;
        float y = Bounds.Y + (Bounds.Height - textSize.Height) / 2;
        canvas.DrawString(Name, x, y, textSize.Width, textSize.Height, HorizontalAlignment.Left, VerticalAlignment.Top);
    }
}

// Séquence verticale (nœuds l’un sous l’autre, flèches vers le bas)
public class SequenceNode : ProcessNode
{
    public List<ProcessNode> Children;
    const float Hpadding = 20, Vpadding = 20, Spacing = 20;

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
                //canvas.DrawPath(new PathF()
                //    .MoveTo(0, 0)
                //    .LineTo(-4, -6)
                //    .LineTo(4, -6)
                //    .Close());
                canvas.RestoreState();
            }

            y += size.Height + Spacing;
        }
    }
}


// Parallèle horizontal avec boîte dont seuls top et bottom sont tracés
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
        canvas.StrokeSize = 2;
        canvas.DrawLine(Bounds.X, Bounds.Y, Bounds.Right, Bounds.Y);
        canvas.DrawLine(Bounds.X, Bounds.Bottom, Bounds.Right, Bounds.Bottom);

        float x = Bounds.X + Hpadding;
        float cy = Bounds.Y + Bounds.Height / 2;

        foreach (var b in Branches)
        {
            var size = b.Measure(canvas);
            b.Bounds = new RectF(x, cy - size.Height / 2, size.Width, size.Height);
            b.Draw(canvas);

            // Flèche depuis le haut du conteneur vers cette branche
            float arrowX = b.Bounds.Center.X;
            canvas.DrawLine(arrowX, Bounds.Y, arrowX, b.Bounds.Y);
            // tête de flèche vers le bas
            canvas.FillColor = Colors.LightGray;
            canvas.SaveState();
            canvas.Translate(arrowX, b.Bounds.Y);
            //canvas.DrawPath(new PathF()
            //    .MoveTo(0, 0)
            //    .LineTo(-4, -6)
            //    .LineTo(4, -6)
            //    .Close());
            canvas.RestoreState();

            // **NOUVEAU** : flèche de la fin de la branche jusqu’au bas du conteneur
            float branchBottomX = b.Bounds.Center.X;
            float branchBottomY = b.Bounds.Bottom;
            canvas.DrawLine(branchBottomX, branchBottomY, branchBottomX, Bounds.Bottom);
            // tête de flèche vers le haut
            canvas.SaveState();
            canvas.Translate(branchBottomX, Bounds.Bottom);
            //canvas.DrawPath(new PathF()
            //    .MoveTo(0, 0)
            //    .LineTo(-4, 6)
            //    .LineTo(4, 6)
            //    .Close());
            canvas.RestoreState();

            x += size.Width + Spacing;
        }
    }
}


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
        float x = Bounds.X + (Bounds.Width - textSize.Width)/2;
        float y = Bounds.Y + (Bounds.Height - textSize.Height)/2;
        canvas.DrawString(Name, x, y, textSize.Width,textSize.Height, HorizontalAlignment.Left, VerticalAlignment.Top);
    }
}

public class SequenceNode : ProcessNode
{
    public List<ProcessNode> Children;
    public SequenceNode(IEnumerable<ProcessNode> children) => Children = children.ToList();
    const float Hpadding = 20, Vpadding = 20, Spacing = 20;
    public override SizeF Measure(ICanvas canvas)
    {
        // mesure chaque enfant, on les aligne horizontalement
        float h = 0, maxY = 0;
        foreach (var c in Children)
        {
            var s = c.Measure(canvas);
            h += s.Width + Spacing;
            maxY = MathF.Max(maxY, s.Height);
        }
        h -= Spacing;
        return new SizeF(h + 2*Hpadding, maxY + 2*Vpadding);
    }
    public override void Draw(ICanvas canvas)
    {
        // layout children
        float x = Bounds.X + Hpadding;
        float cy = Bounds.Y + Bounds.Height/2;
        foreach (var c in Children)
        {
            var size = c.Measure(canvas);
            c.Bounds = new RectF(x, cy - size.Height/2, size.Width, size.Height);
            c.Draw(canvas);
            // flèche vers le suivant
            if (c != Children.Last())
            {
                float x1 = c.Bounds.Right;
                float x2 = x1 + Spacing;
                canvas.StrokeColor = Colors.LightGray;
                canvas.DrawLine(x1, cy, x2, cy);
                // petite tête de flèche
                canvas.FillColor = Colors.LightGray;
                canvas.SaveState();
                canvas.Translate(x2, cy);
                canvas.Rotate(0);
                //canvas.DrawPath(new PathF()
                //    .MoveTo(0,0)
                //    .LineTo(-6,-4)
                //    .LineTo(-6,4)
                //    .Close());
                canvas.RestoreState();
            }
            x += size.Width + Spacing;
        }
    }
}

public class ParallelNode : ProcessNode
{
    public List<ProcessNode> Branches;
    const float Hpadding = 20, Vpadding = 20, Spacing = 20;
    public ParallelNode(IEnumerable<ProcessNode> branches) => Branches = branches.ToList();
    public override SizeF Measure(ICanvas canvas)
    {
        float w = 0, totalH = 0;
        foreach (var b in Branches)
        {
            var s = b.Measure(canvas);
            w = MathF.Max(w, s.Width);
            totalH += s.Height + Spacing;
        }
        totalH -= Spacing;
        return new SizeF(w + 2*Hpadding, totalH + 2*Vpadding);
    }
    public override void Draw(ICanvas canvas)
    {
        // layout branches vertically
        float y = Bounds.Y + Vpadding;
        float cx = Bounds.X + Bounds.Width/2;
        foreach (var b in Branches)
        {
            var size = b.Measure(canvas);
            b.Bounds = new RectF(cx - size.Width/2, y, size.Width, size.Height);
            b.Draw(canvas);
            // flèche depuis centre de ParallelNode vers ce branch
            float startX = Bounds.X;
            float startY = Bounds.Y + Bounds.Height/2;
            float endX = b.Bounds.X;
            float endY = b.Bounds.Y + size.Height/2;
            canvas.StrokeColor = Colors.LightGray;
            canvas.DrawLine(startX, startY, endX, endY);
            // tête de flèche au bout
            canvas.FillColor = Colors.LightGray;
            canvas.SaveState();
            canvas.Translate(endX, endY);
            // orientation un peu calculée, simplifions: flèche horizontale
            //canvas.DrawPath(new PathF()
            //    .MoveTo(0,0)
            //    .LineTo(-6,-4)
            //    .LineTo(-6,4)
            //    .Close());
            canvas.RestoreState();
            y += size.Height + Spacing;
        }
    }
}


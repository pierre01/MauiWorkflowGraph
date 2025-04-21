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
    private ProcessNode _root;
    private string _input;
   
    public GraphRenderer()
    {
    }

    public ProcessNode UpdateGraph(string newInput)
    {
        _input = newInput;
        _root = ProcessParser.Parse(_input);
        return _root;
    }

    // Méthode publique pour le hit‑test
    public ProcessNode HitTest(PointF point)
        => _root?.HitTest(point);

    [ObservableProperty]
    public partial float ContentWidth  { get; set; } =1000;
    [ObservableProperty]
    public partial float ContentHeight { get; set; }=1000;


    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        // mesurer et centrer
        var size = _root.Measure(canvas);
        ContentWidth = size.Width;
        ContentHeight = size.Height;
        _root.Bounds = new RectF(
            dirtyRect.Center.X - size.Width/2,
            dirtyRect.Center.Y - size.Height/2,
            size.Width, size.Height);

        // dessiner
        _root.Draw(canvas);
    }
}


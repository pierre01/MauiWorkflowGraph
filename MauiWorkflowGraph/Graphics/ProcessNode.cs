using MauiWorkflowGraph.Models;

namespace MauiWorkflowGraph.Graphics;
public enum NodeState
{
    Idle,
    Executing,
    Completed,
    Error,
}

/// <summary>
/// Base class for all process nodes. for sequential, parallel and leaf nodes.
/// </summary>
public abstract class ProcessNode
{
    public RectF Bounds;        // Position and size of the node in logical coordinates
    public abstract SizeF Measure(ICanvas canvas);
    public abstract void Draw(ICanvas canvas);
    protected float FontSize = 14;
    protected IFont Font = new Microsoft.Maui.Graphics.Font("OpenSans-SemiBold");
    protected float Density = (float)DeviceDisplay.Current.MainDisplayInfo.Density;
    public abstract ProcessNode HitTest(PointF point);
    public NodeState State { get; set; } = NodeState.Idle;
}

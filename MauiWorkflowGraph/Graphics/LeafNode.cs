﻿using MauiWorkflowGraph.Models;

namespace MauiWorkflowGraph.Graphics;

/// <summary>
/// LeafNode class represents a leaf node in the process graph.
/// </summary>
public class LeafNode : ProcessNode
{
    private FlowProcess _processRule;
    public string Name { get { return _processRule.Name; } }

    public FlowProcess ProcessRule { get => _processRule; }

    /// <summary>
    /// 
    /// </summary>
    public void SelectDeselect()
    {
        if (FlowProcessManager.Instance.SelectedProcess == _processRule)
        {
            FlowProcessManager.Instance.SelectedProcess = null;
        }
        else
        {
            FlowProcessManager.Instance.SelectedProcess = _processRule;
        }
    }

    public LeafNode(string id)
    {
        _processRule = FlowProcessManager.Instance.FlowProcesses[id];
    }

    public override SizeF Measure(ICanvas canvas)
    {
        //canvas.DisplayScale = Density;
        canvas.Font = Font;
        canvas.FontSize = FontSize;
        canvas.FontColor = Colors.Black;

        var textSize = canvas.GetStringSize(Name, Font, FontSize);
#if WINDOWS
        return new SizeF(textSize.Width *Density + 16 , textSize.Height*Density + 24 );     
#else        
        return new SizeF(textSize.Width + 16, textSize.Height + 16);
#endif
    }

    public override void Draw(ICanvas canvas)
    {

        canvas.StrokeColor = Colors.LightGray;
        canvas.StrokeSize = 2;
        if (FlowProcessManager.Instance.SelectedProcess == _processRule)
        {

            canvas.StrokeDashPattern = new float[] { 4, 2 };
            canvas.StrokeDashOffset = 0;
            canvas.FillColor = Colors.LightBlue;
        }
        else
        {
            // draw no dash lines
            canvas.StrokeDashPattern = null;
            switch (State)
            {
                case NodeState.Executing:
                    canvas.FillColor = Colors.Orange;
                    break;
                case NodeState.Completed:
                    canvas.FillColor = Colors.Green;
                    break;
                default:
                    canvas.FillColor = Colors.White;
                    break;
            }
        }
        canvas.FillRoundedRectangle(Bounds, 4);
        canvas.DrawRoundedRectangle(Bounds, 4);
        canvas.StrokeDashPattern = null;

        // Center Text
        var textSize = canvas.GetStringSize(Name, Font, FontSize);
        float x = Bounds.X + (Bounds.Width - textSize.Width) / 2;
        float y = Bounds.Y + (Bounds.Height - textSize.Height) / 2;
#if WINDOWS
        canvas.DrawString(Name, x, y-4, textSize.Width * Density, textSize.Height * Density + 8  , HorizontalAlignment.Left, VerticalAlignment.Top);
#else        
        canvas.DrawString(Name, x, y, textSize.Width * Density, textSize.Height * Density, HorizontalAlignment.Left, VerticalAlignment.Top);
#endif
    }

    public override ProcessNode HitTest(PointF point)
    {
        return Bounds.Contains(point) ? this : null;
    }
}

using MauiWorkflowGraph.Graphics;
using MauiWorkflowGraph.Models;
// PointerRoutedEventArgs
#if WINDOWS
using Microsoft.UI.Xaml; // Correct namespace for UIElement in WinUI
using Microsoft.Graphics.Canvas.UI.Xaml; // Win2D CanvasControl
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Input;
using Windows.System;
using Windows.UI.Core;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
#endif



namespace MauiWorkflowGraph;

public partial class MainPage : ContentPage
{
    GraphRenderer _renderer;

    // Flags pour détecter pan / pinch / tap
    bool _isPinching;
    bool _isTap;
    float _initialDistance;
    float _initialScale;
    PointF _initialTapPoint;
    PointF _lastPanPoint;

    public MainPage()
    {
        InitializeComponent();

        // Between square bracket are sequential processes (rules), between parenthesis are parallel ones
        // The name is the dictionary index found in FlowProcessManager.cs
        string input = "[Rule1,(Rule2,Rule3),Rule4,Rule5,([Rule6,Rule7],[Rule8,(Rule9,[Rule10,Rule11])],[Rule12,Rule13,Rule14]),Rule15]";
        _renderer = new GraphRenderer();
        _renderer.UpdateGraph(input);
        myGraphicsView.Drawable = _renderer;

#if WINDOWS
        myGraphicsView.HandlerChanged += (s, e) =>
        {
            if (myGraphicsView.Handler?.PlatformView is UIElement native) // Added null check to avoid CS8602
            {
                native.PointerWheelChanged += OnPointerWheelChanged;
            }
        };
#endif

        // Activate Touch events
        myGraphicsView.StartInteraction += OnStartInteraction;
        myGraphicsView.DragInteraction += OnDragInteraction;
        myGraphicsView.EndInteraction += OnEndInteraction;
    }

    // Allow zooming with the mouse (ctrl + Wheel)
#if WINDOWS
    private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
    {
        // récupère le PointerPoint relatif au CanvasControl
        var pt = e.GetCurrentPoint((UIElement) sender);

        // 3) test “ctrl + molette”:
        if (InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control).HasFlag(CoreVirtualKeyStates.Down))
        {
            float delta = pt.Properties.MouseWheelDelta;
            var logical = _renderer.HitTest(new PointF((float)pt.Position.X, (float)pt.Position.Y));
            _renderer.Scale *= delta > 0 ? 1.1f : 0.9f;
            myGraphicsView.Invalidate();
        }
    }
#endif

    // Starting interaction
    void OnStartInteraction(object sender, TouchEventArgs e)
    {
        if (e.Touches.Length == 2)
        {
            _isPinching = true;
            _isTap = false;
            _initialDistance = Distance(e.Touches[0], e.Touches[1]);
            _initialScale = _renderer.Scale;
        }
        else if (e.Touches.Length == 1)
        {
            _isTap = true;
            _lastPanPoint = new PointF(e.Touches[0].X, e.Touches[0].Y);
            _initialTapPoint = _lastPanPoint;
        }
    }

    // Panning with finger down or mouse, and zooming with pinch
    void OnDragInteraction(object sender, TouchEventArgs e)
    {
        if (_isPinching && e.Touches.Length == 2)
        {
            // pinch → zoom
            var newDist = Distance(e.Touches[0], e.Touches[1]);
            _renderer.Scale = _initialScale * (newDist / _initialDistance);
            myGraphicsView.Invalidate();
        }
        else if (e.Touches.Length == 1 && !_isPinching)
        {
            var cur = new PointF(e.Touches[0].X, e.Touches[0].Y);
            // Moving too much is not tapping or clicking, it's panning
            if (_isTap && Distance(cur, _initialTapPoint) > 10)
                _isTap = false;
            // pan if it's not a tap
            if (!_isTap)
            {
                _renderer.Offset = new PointF(
                    _renderer.Offset.X + (cur.X - _lastPanPoint.X),
                    _renderer.Offset.Y + (cur.Y - _lastPanPoint.Y));
                _lastPanPoint = cur;
                myGraphicsView.Invalidate();
            }
        }
    }

    /// <summary>
    /// Handles the tap or click event on the graph.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>    
    void OnEndInteraction(object sender, TouchEventArgs e)
    {
        if (_isTap && e.Touches.Length == 1)
        {
            var p = e.Touches[0];
            var tapped = _renderer.HitTest(new PointF(p.X, p.Y));
            if (tapped is LeafNode leaf)
            {
                //Selects or deselects the node if tapped
                leaf.SelectDeselect();
                if (FlowProcessManager.Instance.SelectedProcess != null)
                {
                    // TODO: Expand the bottom sheet with the selected process details
                } else
                {
                    // TODO: Collapse the bottom sheet if no process is selected
                }

                myGraphicsView.Invalidate();
            }
            // Deselect current if tapped outside
            if(tapped == null && FlowProcessManager.Instance.SelectedProcess !=null)
            {
                FlowProcessManager.Instance.SelectedProcess = null;
                // TODO: Collapse the bottom sheet
                myGraphicsView.Invalidate();
            }
        }
        // reset des flags
        _isPinching = false;
        _isTap = false;
    }

    // utilitaire : distance euclidienne
    float Distance(PointF a, PointF b)
    {
        var dx = a.X - b.X;
        var dy = a.Y - b.Y;
        return MathF.Sqrt(dx * dx + dy * dy);
    }


    /// <summary>
    /// Start the simulation when the button is clicked.
    /// </summary>
    async void OnStartSimulationClicked(object sender, EventArgs e)
    {
        btnStart.IsEnabled = false;
        await SimulateNode(_renderer.Root);
        btnStart.IsEnabled = true;
        await Task.Delay(2000);
        _renderer.ResetStates();
        myGraphicsView.Invalidate();
    }


    /// <summary>
    /// Simulate node execution.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    async Task SimulateNode(ProcessNode node)
    {
        switch (node)
        {
            case LeafNode leaf:
                leaf.State = NodeState.Executing;
                myGraphicsView.Invalidate();
                var success  = await leaf.ProcessRule.Execute();
                if (!success)
                {
                    leaf.State = NodeState.Error;
                    myGraphicsView.Invalidate();
                    return;
                }
                leaf.State = NodeState.Completed;
                myGraphicsView.Invalidate();
                break;

            case SequenceNode seq:
                foreach (var child in seq.Children)
                    await SimulateNode(child);
                break;

            case ParallelNode par:
                // lance toutes les branches en même temps
                var tasks = par.Branches
                                .Select(branch => SimulateNode(branch));
                await Task.WhenAll(tasks);
                break;
        }
    }

}

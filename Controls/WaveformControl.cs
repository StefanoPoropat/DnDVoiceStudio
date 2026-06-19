using System.Windows;
using System.Windows.Media;

namespace DnDVoiceStudio.Controls;

public class WaveformControl : FrameworkElement
{
    private readonly List<float> _samples =
        new();

    public void AddLevel(
        float level)
    {
        _samples.Add(level);

        while (_samples.Count > 100)
            _samples.RemoveAt(0);

        InvalidateVisual();
    }

    protected override void OnRender(
        DrawingContext dc)
    {
        base.OnRender(dc);

        dc.DrawRectangle(
            Brushes.Black,
            null,
            new Rect(
                0,
                0,
                ActualWidth,
                ActualHeight));

        if (_samples.Count < 2)
            return;

        Pen pen =
            new(
                Brushes.LimeGreen,
                2);

        double step =
            ActualWidth /
            (double)_samples.Count;

        for (int i = 1;
             i < _samples.Count;
             i++)
        {
            double x1 =
                (i - 1) * step;

            double x2 =
                i * step;

            double y1 =
                ActualHeight -
                (_samples[i - 1]
                 / 100.0 *
                 ActualHeight);

            double y2 =
                ActualHeight -
                (_samples[i]
                 / 100.0 *
                 ActualHeight);

            dc.DrawLine(
                pen,
                new Point(x1, y1),
                new Point(x2, y2));
        }
    }
}
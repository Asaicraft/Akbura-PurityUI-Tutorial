using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Input.Platform;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Avalonia.VisualTree;

namespace PurityUIDashborad.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Used to quickly capture full-page UI previews while recording
        // the "Building a Responsive UI with Akbura" video.
        AddHandler(
            KeyDownEvent,
            OnKeyDown,
            RoutingStrategies.Tunnel);
    }

    private async void OnKeyDown(object? sender, KeyEventArgs e)
    {
        // Physical key: ' / "
        //
        // Captures the complete dashboard, including content outside
        // the visible viewport, and copies it to the clipboard.
        if (e.PhysicalKey != PhysicalKey.Quote)
        {
            return;
        }

        await CopyFullScreenshotToClipboardAsync();

        e.Handled = true;
    }

    private async Task CopyFullScreenshotToClipboardAsync()
    {
        if (Content is not Control target)
        {
            return;
        }

        // Find the ScrollViewers through the visual tree instead of
        // FindControl, because they live inside nested Akbura components
        // and therefore may belong to different name scopes.
        var mainScrollViewer = this
            .GetVisualDescendants()
            .OfType<ScrollViewer>()
            .FirstOrDefault(x => x.Name == "MainScrollViewer");

        var sidebarScrollViewer = this
            .GetVisualDescendants()
            .OfType<ScrollViewer>()
            .FirstOrDefault(x => x.Name == "SidebarScrollViewer");

        if (mainScrollViewer is null)
        {
            return;
        }

        target.UpdateLayout();

        var originalSize = target.Bounds.Size;

        var originalTargetHeight = target.Height;
        var originalMainHeight = mainScrollViewer.Height;

        var originalMainVerticalScrollBarVisibility =
            mainScrollViewer.VerticalScrollBarVisibility;

        var originalSidebarHeight =
            sidebarScrollViewer?.Height ?? double.NaN;

        try
        {
            // Extent represents the full scrollable content height,
            // including the part that is currently outside the viewport.
            var mainContentHeight =
                mainScrollViewer.Extent.Height;

            var sidebarContentHeight =
                sidebarScrollViewer?.Extent.Height ?? 0d;

            // Keep the current responsive width and only increase
            // the height enough to reveal the entire dashboard.
            var captureHeight = Math.Ceiling(
                Math.Max(
                    originalSize.Height,
                    Math.Max(
                        mainContentHeight,
                        sidebarContentHeight)));

            var captureWidth =
                Math.Ceiling(originalSize.Width);

            var captureSize = new Size(
                captureWidth,
                captureHeight);

            // Temporarily expand the UI vertically so the complete page
            // can be rendered into a single image.
            target.Height = captureHeight;
            mainScrollViewer.Height = captureHeight;

            if (sidebarScrollViewer is not null)
            {
                sidebarScrollViewer.Height = captureHeight;
            }

            mainScrollViewer.VerticalScrollBarVisibility =
                ScrollBarVisibility.Hidden;

            target.Measure(captureSize);

            target.Arrange(
                new Rect(
                    0,
                    0,
                    captureWidth,
                    captureHeight));

            target.UpdateLayout();

            var pixelSize = new PixelSize(
                (int)captureWidth,
                (int)captureHeight);

            using var bitmap = new RenderTargetBitmap(
                pixelSize,
                new Vector(96d, 96d));

            bitmap.Render(target);

            if (Clipboard is null)
            {
                return;
            }

            await Clipboard.SetBitmapAsync(bitmap);
            await Clipboard.FlushAsync();
        }
        finally
        {
            // Restore the normal interactive layout after the capture.
            target.Height = originalTargetHeight;

            mainScrollViewer.Height =
                originalMainHeight;

            if (sidebarScrollViewer is not null)
            {
                sidebarScrollViewer.Height =
                    originalSidebarHeight;
            }

            mainScrollViewer.VerticalScrollBarVisibility =
                originalMainVerticalScrollBarVisibility;

            target.Measure(originalSize);

            target.Arrange(
                new Rect(
                    0,
                    0,
                    originalSize.Width,
                    originalSize.Height));

            target.UpdateLayout();
        }
    }
}
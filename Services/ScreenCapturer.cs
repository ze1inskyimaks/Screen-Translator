using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ScreenTranslatorApp.Services;

public static class ScreenCapturer
{
    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    static extern bool BitBlt(
        IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight,
        IntPtr hdcSrc, int nXSrc, int nYSrc, CopyPixelOperation dwRop);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);
    
    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(SystemMetric smIndex);

    private enum SystemMetric
    {
        SM_CXSCREEN = 0,
        SM_CYSCREEN = 1
    }
    
    public static Bitmap CaptureScreenArea(int x, int y, int width, int height)
    {
        Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            IntPtr hdcDest = g.GetHdc();
            IntPtr hdcSrc = GetDC(IntPtr.Zero);

            BitBlt(hdcDest, 0, 0, width, height, hdcSrc, x, y, CopyPixelOperation.SourceCopy | CopyPixelOperation.CaptureBlt);

            ReleaseDC(IntPtr.Zero, hdcSrc);
            g.ReleaseHdc(hdcDest);
        }

        return bmp;
    }

    /// <summary>
    /// Захопити весь основний екран.
    /// </summary>
    /// <returns>Зображення Bitmap із всього екрана</returns>
    public static Bitmap CaptureFullScreen()
    {
        int screenWidth = GetSystemMetrics(SystemMetric.SM_CXSCREEN);
        int screenHeight = GetSystemMetrics(SystemMetric.SM_CYSCREEN);

        return CaptureScreenArea(0, 0, screenWidth, screenHeight);
    }
}
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace FractalApp
{
    public partial class MainWindow : Window
    {
        private CancellationTokenSource _ctsMandelbrot;
        private CancellationTokenSource _ctsJulia;

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Mandelbrot
        private async void MandelbrotBg_Click(object sender, RoutedEventArgs e)
        {
            _ctsMandelbrot = new CancellationTokenSource();
            ToggleMandelbrotButtons(false);

            var progress = new Progress<double>(p => progressMandelbrot.Value = p * 100);
            await Task.Run(() => CalculateMandelbrot(_ctsMandelbrot.Token, progress));

            ToggleMandelbrotButtons(true);
            progressMandelbrot.Value = 0;
        }

        private void MandelbrotUi_Click(object sender, RoutedEventArgs e)
        {
            _ctsMandelbrot = new CancellationTokenSource();
            ToggleMandelbrotButtons(false);

            CalculateMandelbrot(_ctsMandelbrot.Token, null);

            ToggleMandelbrotButtons(true);
            progressMandelbrot.Value = 0;
        }

        private void CancelMandelbrot_Click(object sender, RoutedEventArgs e)
        {
            _ctsMandelbrot?.Cancel();
        }

        private void ToggleMandelbrotButtons(bool isEnabled)
        {
            btnMandelbrotUi.IsEnabled = isEnabled;
            btnMandelbrotBg.IsEnabled = isEnabled;
            btnCancelMandelbrot.IsEnabled = !isEnabled;
        }
        #endregion

        #region Julia
        private async void JuliaBg_Click(object sender, RoutedEventArgs e)
        {
            _ctsJulia = new CancellationTokenSource();
            ToggleJuliaButtons(false);

            var progress = new Progress<double>(p => progressJulia.Value = p * 100);
            await Task.Run(() => CalculateJulia(_ctsJulia.Token, progress));

            ToggleJuliaButtons(true);
            progressJulia.Value = 0;
        }

        private void JuliaUi_Click(object sender, RoutedEventArgs e)
        {
            _ctsJulia = new CancellationTokenSource();
            ToggleJuliaButtons(false);

            CalculateJulia(_ctsJulia.Token, null);

            ToggleJuliaButtons(true);
            progressJulia.Value = 0;
        }

        private void CancelJulia_Click(object sender, RoutedEventArgs e)
        {
            _ctsJulia?.Cancel();
        }

        private void ToggleJuliaButtons(bool isEnabled)
        {
            btnJuliaUi.IsEnabled = isEnabled;
            btnJuliaBg.IsEnabled = isEnabled;
            btnCancelJulia.IsEnabled = !isEnabled;
        }
        #endregion

        private void CalculateMandelbrot(CancellationToken token, IProgress<double> progress)
        {
            int width = 800;
            int height = 600;
            int maxIterations = 1000;

            var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);

            Dispatcher.Invoke(() => imgMandelbrot.Source = bitmap);

            double xMin = -2.0;
            double xMax = 1.0;
            double yMin = -1.5;
            double yMax = 1.5;

            for (int y = 0; y < height; y++)
            {
                if (token.IsCancellationRequested) return;

                double imaginary = yMin + (y * (yMax - yMin) / height);

                for (int x = 0; x < width; x++)
                {
                    double real = xMin + (x * (xMax - xMin) / width);

                    int iteration = 0;
                    double zReal = 0;
                    double zImag = 0;

                    while (iteration < maxIterations && (zReal * zReal + zImag * zImag) < 4)
                    {
                        double temp = zReal * zReal - zImag * zImag + real;
                        zImag = 2 * zReal * zImag + imaginary;
                        zReal = temp;
                        iteration++;
                    }

                    byte color = (byte)(iteration == maxIterations ? 0 : (iteration % 255));
                    SetPixel(bitmap, x, y, color, color, color);
                }

                progress?.Report((double)y / height);
            }
        }

        private void CalculateJulia(CancellationToken token, IProgress<double> progress)
        {
            int width = 800;
            int height = 600;
            int maxIterations = 1000;

            var bitmap = new WriteableBitmap(width, height, 96, 96, PixelFormats.Rgb24, null);

            Dispatcher.Invoke(() => imgJulia.Source = bitmap);

            double cReal = double.Parse(txtReal.Text);
            double cImag = double.Parse(txtImag.Text);

            double xMin = -1.5;
            double xMax = 1.5;
            double yMin = -1.5;
            double yMax = 1.5;

            for (int y = 0; y < height; y++)
            {
                if (token.IsCancellationRequested) return;

                double imaginary = yMin + (y * (yMax - yMin) / height);

                for (int x = 0; x < width; x++)
                {
                    double real = xMin + (x * (xMax - xMin) / width);

                    int iteration = 0;
                    double zReal = real;
                    double zImag = imaginary;

                    while (iteration < maxIterations && (zReal * zReal + zImag * zImag) < 4)
                    {
                        double temp = zReal * zReal - zImag * zImag + cReal;
                        zImag = 2 * zReal * zImag + cImag;
                        zReal = temp;
                        iteration++;
                    }

                    byte color = (byte)(iteration == maxIterations ? 0 : (iteration % 255));
                    SetPixel(bitmap, x, y, color, color, color);
                }

                progress?.Report((double)y / height);
            }
        }

        private void SetPixel(WriteableBitmap bitmap, int x, int y, byte r, byte g, byte b)
        {
            try
            {
                bitmap.Lock();
                IntPtr backBuffer = bitmap.BackBuffer;
                int stride = bitmap.BackBufferStride;

                int pos = y * stride + x * 3;
                unsafe
                {
                    byte* buffer = (byte*)backBuffer.ToPointer();
                    buffer[pos] = r;
                    buffer[pos + 1] = g;
                    buffer[pos + 2] = b;
                }
                bitmap.AddDirtyRect(new Int32Rect(x, y, 1, 1));
            }
            finally
            {
                bitmap.Unlock();
            }
        }
    }
}
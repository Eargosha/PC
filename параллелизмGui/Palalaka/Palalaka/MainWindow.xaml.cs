using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Palalaka;

namespace MandelbrotApp
{
    /// <summary>
    /// Главное окно приложения для визуализации фракталов (Множество Мандельброта и Жулиа)
    /// </summary>
    public partial class MainWindow : Window
    {
        // Константы для размеров изображения и параметров вычислений
        private const int Width = 1263;          // Ширина области отрисовки фрактала
        private const int Height = 593;          // Высота области отрисовки фрактала
        private const int MaxIterations = 1000;  // Максимальное количество итераций для расчета точки

        // Калькуляторы для каждого типа фрактала
        private readonly FractalCalculator _mandelbrotCalculator;
        private readonly FractalCalculator _juliaCalculator;

        // Битмапы для хранения изображений фракталов
        private WriteableBitmap _mandelbrotBitmap;
        private WriteableBitmap _juliaBitmap;

        /// <summary>
        /// Конструктор главного окна
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            // Подписываемся на событие загрузки окна
            this.Loaded += MainWindow_Loaded;

            // Инициализируем калькуляторы фракталов
            _mandelbrotCalculator = new FractalCalculator();
            _juliaCalculator = new FractalCalculator();

            // Настраиваем обработчики исключений
            SetupExceptionHandlers();
        }

        /// <summary>
        /// Настройка глобальных обработчиков исключений
        /// </summary>
        private void SetupExceptionHandlers()
        {
            // Обработчик необработанных исключений в домене приложения
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                MessageBox.Show($"Произошла ошибка: {e.ExceptionObject}");

            // Обработчик исключений в UI-потоке
            Dispatcher.UnhandledException += (s, e) =>
            {
                MessageBox.Show($"Ошибка в UI-потоке: {e.Exception}");
                e.Handled = true; // Помечаем исключение как обработанное
            };
        }

        /// <summary>
        /// Обработчик события загрузки окна
        /// </summary>
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeFractalBitmaps();
        }

        /// <summary>
        /// Инициализация битмапов для отрисовки фракталов
        /// </summary>
        private void InitializeFractalBitmaps()
        {
            // Создаем битмапы с заданными размерами
            _mandelbrotBitmap = FractalCalculator.CreateFractalBitmap(Width, Height);
            _juliaBitmap = FractalCalculator.CreateFractalBitmap(Width, Height);

            // Устанавливаем битмапы как фон для канвасов
            FractalCanvasMandelbrot.Background = new ImageBrush(_mandelbrotBitmap);
            FractalCanvasJulia.Background = new ImageBrush(_juliaBitmap);
        }

        #region Обработчики для множества Мандельброта
        /// <summary>
        /// Обработчик нажатия кнопки "Start" для асинхронного расчета Мандельброта
        /// </summary>
        private async void StartMandelbrotButton_Click(object sender, RoutedEventArgs e)
        {
            await StartFractalCalculationAsync(
                _mandelbrotCalculator,
                StartMandelbrotButton,
                StartMandelbrotMainThread,
                StopMandelbrotButton,
                ProgressBarMandelbrot,
                (calc, progress) => FractalRenderer.DrawMandelbrotSet(
                    calc, progress, _mandelbrotBitmap, Width, Height, MaxIterations),
                "Мандельброта");
        }

        /// <summary>
        /// Обработчик нажатия кнопки для расчета Мандельброта в основном потоке
        /// </summary>
        private void StartMandelbrotMainThread_Click(object sender, RoutedEventArgs e)
        {
            // Если уже идет расчет - выходим
            if (_mandelbrotCalculator.IsCalculating) return;

            // Блокируем кнопки
            ToggleMandelbrotButtons(false);

            try
            {
                // Выполняем расчет в основном потоке
                FractalRenderer.DrawMandelbrotSet(
                    _mandelbrotCalculator,
                    p => ProgressBarMandelbrot.Value = p, // Callback для обновления прогресса
                    _mandelbrotBitmap,
                    Width, Height,
                    MaxIterations);
            }
            finally
            {
                // Восстанавливаем состояние кнопок
                ToggleMandelbrotButtons(true);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Stop" для остановки расчета Мандельброта
        /// </summary>
        private void StopMandelbrotButton_Click(object sender, RoutedEventArgs e)
        {
            _mandelbrotCalculator.Stop = true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Reset" для сброса Мандельброта
        /// </summary>
        private void ResetMandelbrotButton_Click(object sender, RoutedEventArgs e)
        {
            FractalOperations.ResetFractal(
                _mandelbrotCalculator,
                ProgressBarMandelbrot,
                ref _mandelbrotBitmap,
                FractalCanvasMandelbrot,
                Width, Height);
        }

        /// <summary>
        /// Переключение состояния кнопок управления для Мандельброта
        /// </summary>
        /// <param name="enable">Флаг активности кнопок</param>
        private void ToggleMandelbrotButtons(bool enable)
        {
            StartMandelbrotButton.IsEnabled = enable;
            StartMandelbrotMainThread.IsEnabled = enable;
            StopMandelbrotButton.IsEnabled = !enable;
        }
        #endregion

        #region Обработчики для множества Жулиа
        /// <summary>
        /// Обработчик нажатия кнопки "Start" для асинхронного расчета Жулиа
        /// </summary>
        private async void StartJuliaButton_Click(object sender, RoutedEventArgs e)
        {
            await StartFractalCalculationAsync(
                _juliaCalculator,
                StartJuliaButton,
                StartJuliaMainThread,
                StopJuliaButton,
                ProgressBarJulia,
                (calc, progress) => FractalRenderer.DrawJuliaSet(
                    calc, progress, _juliaBitmap, Width, Height, MaxIterations),
                "Жулиа");
        }

        /// <summary>
        /// Обработчик нажатия кнопки для расчета Жулиа в основном потоке
        /// </summary>
        private void StartJuliaMainThread_Click(object sender, RoutedEventArgs e)
        {
            if (_juliaCalculator.IsCalculating) return;

            ToggleJuliaButtons(false);

            try
            {
                FractalRenderer.DrawJuliaSet(
                    _juliaCalculator,
                    p => ProgressBarJulia.Value = p,
                    _juliaBitmap,
                    Width, Height,
                    MaxIterations);
            }
            finally
            {
                ToggleJuliaButtons(true);
            }
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Stop" для остановки расчета Жулиа
        /// </summary>
        private void StopJuliaButton_Click(object sender, RoutedEventArgs e)
        {
            _juliaCalculator.Stop = true;
        }

        /// <summary>
        /// Обработчик нажатия кнопки "Reset" для сброса Жулиа
        /// </summary>
        private void ResetJuliaButton_Click(object sender, RoutedEventArgs e)
        {
            FractalOperations.ResetFractal(
                _juliaCalculator,
                ProgressBarJulia,
                ref _juliaBitmap,
                FractalCanvasJulia,
                Width, Height);
        }

        /// <summary>
        /// Переключение состояния кнопок управления для Жулиа
        /// </summary>
        /// <param name="enable">Флаг активности кнопок</param>
        private void ToggleJuliaButtons(bool enable)
        {
            StartJuliaButton.IsEnabled = enable;
            StartJuliaMainThread.IsEnabled = enable;
            StopJuliaButton.IsEnabled = !enable;
        }
        #endregion

        /// <summary>
        /// Общий метод для запуска асинхронного расчета фрактала
        /// </summary>
        /// <param name="calculator">Калькулятор фрактала</param>
        /// <param name="startAsyncButton">Кнопка запуска асинхронного расчета</param>
        /// <param name="startMainButton">Кнопка запуска в основном потоке</param>
        /// <param name="stopButton">Кнопка остановки</param>
        /// <param name="progressBar">Прогресс-бар</param>
        /// <param name="calculationMethod">Метод расчета фрактала</param>
        /// <param name="fractalName">Название фрактала (для сообщений об ошибках)</param>
        private async Task StartFractalCalculationAsync(
            FractalCalculator calculator,
            Button startAsyncButton,
            Button startMainButton,
            Button stopButton,
            ProgressBar progressBar,
            Action<FractalCalculator, Action<double>> calculationMethod,
            string fractalName)
        {
            if (calculator.IsCalculating) return;

            // Блокируем кнопки управления
            ToggleButtons(startAsyncButton, startMainButton, stopButton, false);
            progressBar.Value = 0;
            calculator.Stop = false;

            try
            {
                calculator.IsCalculating = true;

                // Запускаем расчет в фоновом потоке
                await Task.Run(() =>
                {
                    calculationMethod(calculator, progress =>
                        // Обновляем прогресс через диспетчер
                        Dispatcher.BeginInvoke(() => progressBar.Value = progress));
                }).ConfigureAwait(false); // Оптимизация для продолжения в любом потоке
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при вычислении множества {fractalName}: {ex.Message}");
            }
            finally
            {
                calculator.IsCalculating = false;
                // Восстанавливаем кнопки через диспетчер
                Dispatcher.Invoke(() => ToggleButtons(startAsyncButton, startMainButton, stopButton, true));
            }
        }

        /// <summary>
        /// Переключение состояния группы кнопок
        /// </summary>
        /// <param name="startAsync">Кнопка асинхронного запуска</param>
        /// <param name="startMain">Кнопка запуска в основном потоке</param>
        /// <param name="stop">Кнопка остановки</param>
        /// <param name="enable">Флаг активности</param>
        private void ToggleButtons(Button startAsync, Button startMain, Button stop, bool enable)
        {
            startAsync.IsEnabled = enable;
            startMain.IsEnabled = enable;
            stop.IsEnabled = !enable;
        }
    }
}
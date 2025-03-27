using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MandelbrotApp;
using System.Windows.Media.Imaging;
using System.Windows;

namespace Palalaka
{
    /// <summary>
    /// Статический класс для отрисовки фракталов в bitmap-буфер
    /// </summary>
    public static class FractalRenderer
    {
        /// <summary>
        /// Рендерит множество Мандельброта в указанный bitmap
        /// </summary>
        /// <param name="calculator">Калькулятор с состоянием вычислений</param>
        /// <param name="progressCallback">Callback для обновления прогресса</param>
        /// <param name="bitmap">Целевой bitmap для отрисовки</param>
        /// <param name="width">Ширина области отрисовки</param>
        /// <param name="height">Высота области отрисовки</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <remarks>
        /// Алгоритм:
        /// 1. Создается массив пикселей
        /// 2. Для каждой точки вычисляется количество итераций
        /// 3. Количество итераций преобразуется в цвет
        /// 4. Каждые 10 строк обновляется прогресс
        /// 5. Результат записывается в bitmap
        /// </remarks>
        public static void DrawMandelbrotSet(FractalCalculator calculator, Action<double> progressCallback,
            WriteableBitmap bitmap, int width, int height, int maxIterations)
        {
            // Буфер для хранения ARGB-значений пикселей
            int[] pixels = new int[width * height];

            // Проход по всем строкам изображения
            for (int y = 0; y < height; y++)
            {
                // Проверка флага остановки
                if (calculator.Stop) return;

                // Проход по всем столбцам
                for (int x = 0; x < width; x++)
                {
                    // Преобразование координат пикселя в комплексную плоскость
                    double real = (x - width / 2.0) * 4.0 / width;
                    double imag = (y - height / 2.0) * 4.0 / height;

                    // Вычисление количества итераций для точки
                    int iteration = FractalCalculations.CalculateMandelbrotPoint(real, imag, maxIterations);
                    // Преобразование итераций в цвет
                    pixels[y * width + x] = FractalCalculations.GetColorFromIteration(iteration, maxIterations);
                }

                // Обновление прогресса каждые 10 строк (оптимизация производительности)
                if (y % 10 == 0)
                    progressCallback(y * 100.0 / height);
            }

            // Запись результата в bitmap
            UpdateBitmap(bitmap, pixels, width, height);
        }

        /// <summary>
        /// Рендерит множество Жулиа в указанный bitmap
        /// </summary>
        /// <param name="calculator">Калькулятор с состоянием вычислений</param>
        /// <param name="progressCallback">Callback для обновления прогресса</param>
        /// <param name="bitmap">Целевой bitmap для отрисовки</param>
        /// <param name="width">Ширина области отрисовки</param>
        /// <param name="height">Высота области отрисовки</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <remarks>
        /// Отличается от Мандельброта фиксированными параметрами cRe и cIm
        /// и преобразованием начальных координат
        /// </remarks>
        public static void DrawJuliaSet(FractalCalculator calculator, Action<double> progressCallback,
            WriteableBitmap bitmap, int width, int height, int maxIterations)
        {
            // Буфер для хранения ARGB-значений пикселей
            int[] pixels = new int[width * height];

            // Фиксированные параметры для множества Жулиа
            double cRe = -0.7;
            double cIm = 0.27015;

            // Проход по всем строкам изображения
            for (int y = 0; y < height; y++)
            {
                // Проверка флага остановки
                if (calculator.Stop) return;

                // Проход по всем столбцам
                for (int x = 0; x < width; x++)
                {
                    // Преобразование координат пикселя в начальные значения z
                    double zx = 1.5 * (x - width / 2.0) / (0.5 * width);
                    double zy = (y - height / 2.0) / (0.5 * height);

                    // Вычисление количества итераций для точки
                    int iteration = FractalCalculations.CalculateJuliaPoint(zx, zy, cRe, cIm, maxIterations);
                    // Преобразование итераций в цвет
                    pixels[y * width + x] = FractalCalculations.GetColorFromIteration(iteration, maxIterations);
                }

                // Обновление прогресса каждые 10 строк
                if (y % 10 == 0)
                    progressCallback(y * 100.0 / height);
            }

            // Запись результата в bitmap
            UpdateBitmap(bitmap, pixels, width, height);
        }

        /// <summary>
        /// Обновляет bitmap новыми пикселями через UI-диспетчер
        /// </summary>
        /// <param name="bitmap">Целевой bitmap</param>
        /// <param name="pixels">Массив пикселей в формате ARGB</param>
        /// <param name="width">Ширина изображения</param>
        /// <param name="height">Высота изображения</param>
        /// <remarks>
        /// Использует Dispatcher.Invoke для безопасного обновления UI из другого потока
        /// </remarks>
        private static void UpdateBitmap(WriteableBitmap bitmap, int[] pixels, int width, int height)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Запись пикселей в bitmap
                bitmap.WritePixels(
                    new Int32Rect(0, 0, width, height), // Область обновления
                    pixels,                             // Буфер пикселей
                    width * 4,                          // Шаг (4 байта на пиксель)
                    0);                                 // Смещение в буфере
            });
        }
    }
}
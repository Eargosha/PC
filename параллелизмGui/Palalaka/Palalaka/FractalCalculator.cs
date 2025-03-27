using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Palalaka
{
    /// <summary>
    /// Класс для управления состоянием вычислений фракталов и создания bitmap-буферов
    /// </summary>
    public class FractalCalculator
    {
        /// <summary>
        /// Флаг для остановки текущих вычислений
        /// </summary>
        /// <remarks>
        /// Устанавливается в true для досрочного прерывания расчетов.
        /// Должен проверяться в цикле вычислений.
        /// </remarks>
        public bool Stop { get; set; }

        /// <summary>
        /// Флаг, указывающий что вычисления в процессе выполнения
        /// </summary>
        /// <remarks>
        /// Используется для предотвращения одновременного запуска нескольких вычислений
        /// </remarks>
        public bool IsCalculating { get; set; }

        /// <summary>
        /// Создает новый WriteableBitmap для отрисовки фрактала
        /// </summary>
        /// <param name="width">Ширина изображения в пикселях</param>
        /// <param name="height">Высота изображения в пикселях</param>
        /// <returns>Новый WriteableBitmap с заданными параметрами</returns>
        /// <remarks>
        /// Параметры bitmap:
        /// - DPI: 96 (стандартное значение для Windows)
        /// - PixelFormat: Bgra32 (32 бита на пиксель - по 8 бит на канал Blue, Green, Red, Alpha)
        /// - Palette: null (используется прямой формат цвета)
        /// </remarks>
        public static WriteableBitmap CreateFractalBitmap(int width, int height)
        {
            return new WriteableBitmap(
                width, height,       // Размеры изображения
                96.0, 96.0,         // Горизонтальное и вертикальное DPI
                PixelFormats.Bgra32, // Формат пикселей (Blue, Green, Red, Alpha)
                null);               // Палитра (не используется)
        }
    }
}
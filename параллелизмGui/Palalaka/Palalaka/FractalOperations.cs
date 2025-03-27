using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Palalaka
{
    /// <summary>
    /// Статический класс, содержащий операции управления фракталами
    /// </summary>
    public static class FractalOperations
    {
        /// <summary>
        /// Сбрасывает состояние фрактала к начальному
        /// </summary>
        /// <param name="calculator">Калькулятор фрактала</param>
        /// <param name="progressBar">Прогресс-бар для сброса</param>
        /// <param name="bitmap">Ссылка на bitmap фрактала (будет заменен новым)</param>
        /// <param name="canvas">Канвас для отображения фрактала</param>
        /// <param name="width">Ширина нового bitmap</param>
        /// <param name="height">Высота нового bitmap</param>
        /// <remarks>
        /// Выполняет следующие действия:
        /// 1. Останавливает текущие вычисления
        /// 2. Сбрасывает прогресс
        /// 3. Создает новый чистый bitmap
        /// 4. Устанавливает bitmap как фон канваса
        /// 
        /// Параметр bitmap передается по ссылке (ref), так как требуется
        /// изменить саму ссылку, а не только содержимое объекта
        /// </remarks>
        public static void ResetFractal(
            FractalCalculator calculator,
            ProgressBar progressBar,
            ref WriteableBitmap bitmap,
            Canvas canvas,
            int width,
            int height)
        {
            // Остановка текущих вычислений
            calculator.Stop = true;

            // Сброс прогресс-бара
            progressBar.Value = 0;

            // Создание нового чистого bitmap
            bitmap = FractalCalculator.CreateFractalBitmap(width, height);

            // Установка bitmap как фона канваса
            canvas.Background = new ImageBrush(bitmap);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Palalaka
{
    /// <summary>
    /// Статический класс, содержащий математические вычисления для фракталов
    /// </summary>
    public static class FractalCalculations
    {
        /// <summary>
        /// Вычисляет количество итераций для точки в множестве Мандельброта
        /// </summary>
        /// <param name="real">Реальная часть комплексного числа</param>
        /// <param name="imag">Мнимая часть комплексного числа</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <returns>
        /// Количество итераций до расхождения или maxIterations, 
        /// если точка принадлежит множеству
        /// </returns>
        /// <remarks>
        /// Алгоритм:
        /// 1. Инициализируем z = 0
        /// 2. На каждой итерации вычисляем z = z^2 + c
        /// 3. Если |z| > 2 (zx^2 + zy^2 > 4) - последовательность расходится
        /// 4. Если достигли maxIterations - точка принадлежит множеству
        /// </remarks>
        public static int CalculateMandelbrotPoint(double real, double imag, int maxIterations)
        {
            int iteration = 0;
            double zx = 0.0;
            double zy = 0.0;

            // Вычисляем пока точка не выйдет за границы круга радиуса 2
            // или не достигнем максимального числа итераций
            while (zx * zx + zy * zy < 4.0 && iteration < maxIterations)
            {
                // z = z^2 + c
                // (a+bi)^2 = a^2 - b^2 + 2abi
                double temp = zx * zx - zy * zy + real; // действительная часть
                zy = 2.0 * zx * zy + imag;             // мнимая часть
                zx = temp;
                iteration++;
            }
            return iteration;
        }

        /// <summary>
        /// Вычисляет количество итераций для точки в множестве Жулиа
        /// </summary>
        /// <param name="zx">Начальная действительная часть z</param>
        /// <param name="zy">Начальная мнимая часть z</param>
        /// <param name="cRe">Действительная часть константы c</param>
        /// <param name="cIm">Мнимая часть константы c</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <returns>
        /// Количество итераций до расхождения или maxIterations, 
        /// если точка принадлежит множеству
        /// </returns>
        /// <remarks>
        /// Алгоритм аналогичен Мандельброту, но с фиксированным c
        /// и изменяющимся начальным z
        /// </remarks>
        public static int CalculateJuliaPoint(double zx, double zy, double cRe, double cIm, int maxIterations)
        {
            int iteration = 0;

            // Условие аналогично Мандельброту
            while (zx * zx + zy * zy < 4.0 && iteration < maxIterations)
            {
                double temp = zx * zx - zy * zy + cRe; // действительная часть
                zy = 2.0 * zx * zy + cIm;             // мнимая часть
                zx = temp;
                iteration++;
            }
            return iteration;
        }

        /// <summary>
        /// Преобразует количество итераций в цвет пикселя
        /// </summary>
        /// <param name="iteration">Количество итераций</param>
        /// <param name="maxIterations">Максимальное количество итераций</param>
        /// <returns>Цвет в формате ARGB (32 бита)</returns>
        /// <remarks>
        /// Если точка принадлежит множеству (iteration == maxIterations) - черный цвет
        /// Иначе - цвет зависит от количества итераций с использованием модуля
        /// для создания цветового градиента
        /// </remarks>
        public static int GetColorFromIteration(int iteration, int maxIterations)
        {
            Color color = iteration == maxIterations ?
                Colors.Black : // Точка в множестве - черный цвет
                Color.FromRgb(
                    (byte)(iteration % 256),         // Красный компонент
                    (byte)(iteration * 9 % 256),     // Зеленый компонент (x9 для разнообразия)
                    (byte)(iteration * 16 % 256));   // Синий компонент (x16 для разнообразия)

            // Упаковываем цвет в 32-битное целое (ARGB)
            return color.A << 24 | color.R << 16 | color.G << 8 | color.B;
        }
    }
}
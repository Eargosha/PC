using System;
using System.Threading;
using System.Diagnostics;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        // Создаем большой массив (100 миллионов элементов)
        int arraySize = 100_000_000; // Уменьшен размер для стабильности демонстрации
        int[] largeArray = new int[arraySize];

        // Заполняем массив случайными числами
        Random rand = new Random();
        for (int i = 0; i < arraySize; i++)
        {
            largeArray[i] = rand.Next(1, 1000);
        }

        Console.WriteLine($"Создан массив из {arraySize} элементов.");

        // Получаем количество логических процессоров
        int threadCount = Environment.ProcessorCount;
        Console.WriteLine($"\n--- Параллельная обработка с разделением на {threadCount} частей ---");

        // --- Параллельная сортировка с объединением ---
        Stopwatch sortSw = Stopwatch.StartNew();
        int[][] sortedChunks = Operations.ParallelSort(largeArray, threadCount);
        int[] fullySortedArray = Operations.MergeSortedChunks(sortedChunks);
        sortSw.Stop();
        long ellapsed = sortSw.ElapsedMilliseconds;
        Console.WriteLine($"Полная сортировка завершена за {sortSw.ElapsedMilliseconds} мс.");

        // --- Параллельные агрегатные операции ---
        Stopwatch parallelSw = Stopwatch.StartNew();

        // Для агрегатных операций создаем отдельные чанки (так как массив уже отсортирован)
        int chunkSize = arraySize / threadCount;
        List<int[]> chunks = new List<int[]>();
        for (int i = 0; i < threadCount; i++)
        {
            int start = i * chunkSize;
            int end = (i == threadCount - 1) ? arraySize : (i + 1) * chunkSize;
            chunks.Add(largeArray.Skip(start).Take(end - start).ToArray());
        }

        // Вычисляем сумму с объединением результатов
        long totalSum = Operations.ParallelSum(chunks);
        Console.WriteLine($"Общая сумма: {totalSum}");

        // Находим min/max с объединением результатов
        var (min, max) = Operations.ParallelMinMax(chunks);
        Console.WriteLine($"Global Min: {min}, Global Max: {max}");

        // Вычисляем среднее значение
        double average = (double)totalSum / arraySize;
        Console.WriteLine($"Среднее значение: {average:F2}");

        parallelSw.Stop();
        Console.WriteLine($"Все паралельные операции с разделением массива на {threadCount} частей завершены за {parallelSw.ElapsedMilliseconds + ellapsed} мс.");

        // --- Последовательная обработка для сравнения ---
        Console.WriteLine("\n--- Последовательное выполнение ---");
        Stopwatch sequentialSw = Stopwatch.StartNew();

        int[] seqArray = largeArray.Clone() as int[];
        Operations.SortArray(seqArray);
        Operations.CalculateSum(seqArray);
        Operations.FindMinMax(seqArray);
        Operations.CalculateAverage(seqArray);

        sequentialSw.Stop();
        Console.WriteLine($"Все операции последовательно завершены за {sequentialSw.ElapsedMilliseconds} мс.");

        // Выводим разницу в производительности
        Console.WriteLine("\n--- Результаты сравнения ---");
        Console.WriteLine($"Параллельная обработка быстрее в {(double)sequentialSw.ElapsedMilliseconds / (sortSw.ElapsedMilliseconds + parallelSw.ElapsedMilliseconds):F2} раз");

    }
}













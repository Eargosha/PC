using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


class Operations
{
    public static void SortArray(int[] array)
    {
        Stopwatch sw = Stopwatch.StartNew();

        MergeSort(array, 0, array.Length - 1);

        sw.Stop();
        Console.WriteLine($"Сортировка завершена за {sw.ElapsedMilliseconds} мс.");
    }

    private static void MergeSort(int[] array, int left, int right)
    {
        if (left < right)
        {
            int middle = left + (right - left) / 2;

            // Рекурсивно сортируем обе половины
            MergeSort(array, left, middle);
            MergeSort(array, middle + 1, right);

            // Сливаем отсортированные половины
            Merge(array, left, middle, right);
        }
    }

    private static void Merge(int[] array, int left, int middle, int right)
    {
        int n1 = middle - left + 1;
        int n2 = right - middle;

        // Создаем временные массивы
        int[] leftArray = new int[n1];
        int[] rightArray = new int[n2];

        // Копируем данные во временные массивы
        for (int i = 0; i < n1; ++i)
            leftArray[i] = array[left + i];
        for (int j = 0; j < n2; ++j)
            rightArray[j] = array[middle + 1 + j];

        // Сливаем временные массивы

        int k = left; // Индекс начала merged подмассива
        int x = 0, y = 0; // Индексы для leftArray и rightArray

        while (x < n1 && y < n2)
        {
            if (leftArray[x] <= rightArray[y])
            {
                array[k] = leftArray[x];
                x++;
            }
            else
            {
                array[k] = rightArray[y];
                y++;
            }
            k++;
        }

        // Копируем оставшиеся элементы leftArray, если есть
        while (x < n1)
        {
            array[k] = leftArray[x];
            x++;
            k++;
        }

        // Копируем оставшиеся элементы rightArray, если есть
        while (y < n2)
        {
            array[k] = rightArray[y];
            y++;
            k++;
        }
    }

    public static void CalculateSum(int[] array)
    {
        Stopwatch sw = Stopwatch.StartNew();

        long sum = 0;
        foreach (int num in array)
        {
            sum += num;
        }

        sw.Stop();
        Console.WriteLine($"Сумма элементов: {sum}. Вычислено за {sw.ElapsedMilliseconds} мс.");
    }

    public static void FindMinMax(int[] array)
    {
        Stopwatch sw = Stopwatch.StartNew();

        int min = array[0];
        int max = array[0];

        for (int i = 1; i < array.Length; i++)
        {
            if (array[i] < min) min = array[i];
            if (array[i] > max) max = array[i];
        }

        sw.Stop();
        Console.WriteLine($"Min: {min}, Max: {max}. Найдено за {sw.ElapsedMilliseconds} мс.");
    }

    public static void CalculateAverage(int[] array)
    {
        Stopwatch sw = Stopwatch.StartNew();

        long sum = 0;
        foreach (int num in array)
        {
            sum += num;
        }
        double average = (double)sum / array.Length;

        sw.Stop();
        Console.WriteLine($"Среднее значение: {average:F2}. Вычислено за {sw.ElapsedMilliseconds} мс.");
    }


    ////////INFO////////
    // Пул потоков -- шаблон проектирования параллельных алгоритмов, в котором задачи динамически распределяются между потоками.
    // При этом потоки, как правило, не завершаются после выполнению текущей задачи, а начинают выполнять следующую задачу или ожидают её поступления.
    // Это сокращает накладные расходы, по сравнению со случаем когда для каждой новой задачи или набора задач создаются новые потоки.

    // ThreadPool (пул потоков) - это механизм в .NET для управления набором рабочих потоков, которые могут выполнять несколько задач параллельно.
    // Он: уменьшает накладные расходы на создание/уничтожение потоков, автоматически управляет количеством потоков, оптимизирует использование системных ресурсов
    // Пул потоков создается при запуске приложения. По умолчанию содержит минимальное количество потоков (обычно равное количеству ядер процессора)
    // Может динамически создавать дополнительные потоки при нагрузке. Максимальное количество потоков ограничено (по умолчанию около 1000)

    // QueueUserWorkItem - это основной метод для постановки задач в очередь пула потоков.
    // Первый параметр = callBack - делегат WaitCallback, представляющий метод для выполнения. Эта задача (лямбда-функция) помещается в глобальную очередь пула потоков
    // Второй = state (опционально) - объект, содержащий данные для использования методом doneEvents[0] передаётся как параметр state (для синхронизации)
    // При вызове метода задача помещается в очередь пула потоков. Когда становится доступен
    //   свободный поток из пула, он извлекает задачу из очереди и выполняет.После выполнения поток
    //   возвращается в пул и может быть использован для других задач

    // Постоянно клонируем массив, чтобы избежать конфликтов
    // Каждая задача при завершении "сигналит" через (ManualResetEvent)state).Set()

    // Параллельная сортировка с разделением на чанки
    // Результат — массив отсортированных чанков.
    public static int[][] ParallelSort(int[] array, int chunksCount)
    {
        // 1. Делим массив на чанки
        int chunkSize = array.Length / chunksCount;
        // ManualResetEvent используется для синхронизации (ожидания завершения задач)
        ManualResetEvent[] doneEvents = new ManualResetEvent[chunksCount];
        int[][] chunks = new int[chunksCount][];

        // 2. Запускаем сортировку каждого чанка в отдельном потоке
        for (int i = 0; i < chunksCount; i++)
        {
            // ManualResetEvent используется для синхронизации (ожидания завершения задач)
            doneEvents[i] = new ManualResetEvent(false);
            int start = i * chunkSize;
            int end = (i == chunksCount - 1) ? array.Length : (i + 1) * chunkSize;
            int[] chunk = array.Skip(start).Take(end - start).ToArray();
            chunks[i] = chunk;

            // 3. Ставим задачу в ThreadPool
            // Каждый чанк сортируется в своём потоке через ThreadPool.QueueUserWorkItem
            ThreadPool.QueueUserWorkItem(state =>
            {
                var idx = (int)state;
                Operations.MergeSort(chunks[idx], 0, chunks[idx].Length - 1);
                doneEvents[idx].Set(); // Сигналим о завершении
            }, i); 
        }
        // Ждем выполнение всех задач, блокируя основной поток
        WaitHandle.WaitAll(doneEvents);
        // Результат — массив отсортированных чанков.
        return chunks;
    }

    // Слияние отсортированных чанков
    // Берём первый элемент каждого чанка. Выбираем наименьший из них и добавляем в итоговый массив. Повторяем, пока не объединим все чанки.
    public static int[] MergeSortedChunks(int[][] chunks)
    {
        // Объединяем отсортированные чанки в один массив
        if (chunks.Length == 1) return chunks[0];

        int totalLength = chunks.Sum(c => c.Length);
        int[] result = new int[totalLength];
        int[] indices = new int[chunks.Length];

        // Проходим по всем элементам и выбираем минимальный из доступных
        for (int i = 0; i < totalLength; i++)
        {
            int? minVal = null;
            int minChunk = -1;

            // Ищем минимальный элемент среди всех чанков
            for (int j = 0; j < chunks.Length; j++)
            {
                if (indices[j] < chunks[j].Length)
                {
                    if (!minVal.HasValue || chunks[j][indices[j]] < minVal)
                    {
                        minVal = chunks[j][indices[j]];
                        minChunk = j;
                    }
                }
            }

            result[i] = minVal.Value;
            indices[minChunk]++;
        }

        return result;
    }

    // Параллельное вычисление суммы списка чанков массива
    public static long ParallelSum(List<int[]> chunks)
    {
        ManualResetEvent[] doneEvents = new ManualResetEvent[chunks.Count];
        long[] partialSums = new long[chunks.Count];

        for (int i = 0; i < chunks.Count; i++)
        {
            doneEvents[i] = new ManualResetEvent(false);
            int idx = i;
            ThreadPool.QueueUserWorkItem(_ =>
            {
                // Считаем сумму для своего чанка
                partialSums[idx] = chunks[idx].Sum(x => (long)x);
                doneEvents[idx].Set();
            });
        }

        WaitHandle.WaitAll(doneEvents);
        return partialSums.Sum();
    }

    // Параллельное вычисление min/max списка чанков массива
    public static (int min, int max) ParallelMinMax(List<int[]> chunks)
    {
        ManualResetEvent[] doneEvents = new ManualResetEvent[chunks.Count];
        (int min, int max)[] partialResults = new (int, int)[chunks.Count];

        for (int i = 0; i < chunks.Count; i++)
        {
            doneEvents[i] = new ManualResetEvent(false);
            int idx = i;
            // Находим min/max для своего чанка
            ThreadPool.QueueUserWorkItem(_ =>
            {
                int chunkMin = chunks[idx][0];
                int chunkMax = chunks[idx][0];
                foreach (var num in chunks[idx])
                {
                    if (num < chunkMin) chunkMin = num;
                    if (num > chunkMax) chunkMax = num;
                }
                partialResults[idx] = (chunkMin, chunkMax);
                doneEvents[idx].Set();
            });
        }

        WaitHandle.WaitAll(doneEvents);

        // Находим глобальные min/max среди всех чанков
        int globalMin = partialResults[0].min;
        int globalMax = partialResults[0].max;
        for (int i = 1; i < partialResults.Length; i++)
        {
            if (partialResults[i].min < globalMin) globalMin = partialResults[i].min;
            if (partialResults[i].max > globalMax) globalMax = partialResults[i].max;
        }

        return (globalMin, globalMax);
    }
}


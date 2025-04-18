#pragma warning disable CA1416
using System;
using System.Collections.Concurrent; // Пространство имен содержит классы, которые обеспечивают потокобезопасные коллекции
using System.Threading;
using System.Threading.Tasks;

// Класс для хранения данных о температуре
public class TemperatureMarks
{
    public DateTime Timestamp { get; set; } // Временная метка
    public double Value { get; set; } // Значение температуры
}

// Статический класс для общей очереди
public static class SharedQueue
{
    // BlockingCollection<T> — это потокобезопасный класс коллекции, который используется для организации обмена данными между потоками
    // BlockingCollection автоматически управляет доступом к очереди из разных потоков
    // BlockingCollection<T> автоматически управляет доступом к данным из разных потоков, что исключает необходимость ручной синхронизации
    // boundedCapacity: - ограничение по размеру очережи
    public static BlockingCollection<TemperatureMarks> Queue { get; } = new BlockingCollection<TemperatureMarks>(boundedCapacity: 10);
}

class Program
{
    static void Main()
    {
        Console.WriteLine("Нажмите Ctrl+C, чтобы остановить мониторинг...");

        // Обработка события нажатия Ctrl+C для корректного завершения работы
        Console.CancelKeyPress += (s, e) =>
        {
            // s - Sender, Представляет объект, который инициировал событие. В данном случае это объект Console
            // e - EventArguments, Это объект типа ConsoleCancelEventArgs, который содержит информацию о событии
            // Cancel (bool): Если установить e.Cancel = true, программа не завершится сразу после нажатия Ctrl+C. Это позволяет выполнить дополнительные действия перед завершением

            // Сообщаем очереди, что больше не будет новых данных
            // CompleteAdding помечает BlockingCollection<T> как не допускающей добавления дополнительных элементов
            // После того как коллекция будет помечена как завершенная для добавления, добавление в коллекцию запрещено,
            // и попытки удаления из коллекции не будут ждать, когда коллекция пуста.
            SharedQueue.Queue.CompleteAdding(); // Сообщаем очереди, что больше не будет новых данных
            e.Cancel = true; // Отменяем стандартное поведение завершения программы
        };

        // Создаем задачу для производителя и запускаем на фоне в отдельном потоков
        Task producer = Task.Run(() => Producer());

        // Получаем количество ядер процессора
        int consumerCount = Environment.ProcessorCount;
        Console.WriteLine($"Количество ядер процессора: {consumerCount}");

        // Создаем массив ссылок на задачи потребителей кол-ва consumerCount
        Task[] consumers = new Task[consumerCount];
        for (int i = 0; i < consumerCount; i++)
        {
            int id = i + 1; // Номер потребителя
            // Каждая задача будет работать в фоновом режиме, выполнять переданную лямбда функцию
            // Task.Run - метод, что запускает ассинхронную задачу и запускает ее в пуле потоков
            consumers[i] = Task.Run(() => Consumer(id));
        }

        // Дожидаемся завершения всех задач потребителей, блокируя основной поток и не завершая основной поток
        Task.WaitAll(consumers);

        Console.WriteLine("Мониторинг за температурой завершен.");
    }

    static void Producer()
    {
        Random random = new Random();
        // IsAddingCompleted помечает что очередь завершена и помещать в нее больше нельзя
        // Возвращает значение, указывающее, помечена ли данная коллекция BlockingCollection<T> как закрытая для добавления элементов.
        while (!SharedQueue.Queue.IsAddingCompleted)
        {
            try
            {
                // Создаем новый объект данных о температуре
                var data = new TemperatureMarks
                {
                    Timestamp = DateTime.UtcNow, // Текущее время UTC
                    Value = random.NextDouble() * 100 // Случайное значение от 0 до 100
                };

                // Добавляем данные в общую очередь
                SharedQueue.Queue.Add(data);
                Console.WriteLine($"Produced: Temperature = {data.Value:F2}, Time = {data.Timestamp:HH:mm:ss}");

                // Имитация задержки между генерацией данных (500 мс)
                Thread.Sleep(500);
            }
            catch (InvalidOperationException)
            {
                // Исключение возникает, если очередь завершена (CompleteAdding)
                // Если не перехватить InvalidOperationException, метод Producer() завершится аварийно, и программа может не корректно обработать завершение работы 
                // InvalidOperationException - Это стандартное исключение в .NET, которое возникает, когда операция недопустима в текущем состоянии объекта.
                // Вызов Add() после CompleteAdding().
                break;
            }
        }

        Console.WriteLine("[Producer] Завершение работы.");
    }

    static void Consumer(int id)
    {
        //GetConsumingEnumerable() возвращает перечисляемую коллекцию, которая блокирует поток, пока не будут добавлены новые элементы или очередь не будет завершена.
        //Если был получен сигнал окончания - GetConsumingEnumerable вытащит все оставшиеся элементы из очереди
        foreach (var data in SharedQueue.Queue.GetConsumingEnumerable())
        {
            // Проверяем, является ли значение температуры аномальным (> 80)
            if (data.Value > 80)
            {
                Console.WriteLine($"[Consumer #{id}] ВНИМАНИЕ! Высокая температура: {data.Value:F2} зафиксирована в {data.Timestamp:HH:mm:ss}");
            }
            else
            {
                Console.WriteLine($"[Consumer #{id}] Consumed: Temperature = {data.Value:F2}, Time = {data.Timestamp:HH:mm:ss}");
            }

            // Имитация времени обработки данных (300 мс)
            Thread.Sleep(300);
        }

        Console.WriteLine($"[Consumer #{id}] Завершение работы.");
    }
}

using System;
using System.Collections.Concurrent;
using System.Threading;

// Класс-потребитель данных о температуре
public static class TemperatureConsumer
{
    // Потребитель - обрабатывает данные произведенные Producer
    public static void Consume(int id, BlockingCollection<TemperatureMarks> queue)
    {
        //GetConsumingEnumerable() возвращает перечисляемую коллекцию, которая блокирует поток, пока не будут добавлены новые элементы или очередь не будет завершена.
        //Если был получен сигнал окончания - GetConsumingEnumerable вытащит все оставшиеся элементы из очереди
        foreach (var data in queue.GetConsumingEnumerable())
        {
            // Проверяем, является ли значение температуры аномальным (> 80)
            if (TemperatureUtils.IsTemperatureAnomaly(data.Value))
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
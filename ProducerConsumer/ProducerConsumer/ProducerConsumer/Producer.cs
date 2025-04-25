using System;
using System.Collections.Concurrent;
using System.Threading;

// Класс-производитель данных о температуре
public static class TemperatureProducer
{
    // Производитель - создает объекты TemperatureMarks и помещает в очередь для будущей обработки
    public static void Produce(BlockingCollection<TemperatureMarks> queue)
    {
        Random random = new Random();
        // IsAddingCompleted помечает что очередь завершена и помещать в нее больше нельзя
        // Возвращает значение, указывающее, помечена ли данная коллекция BlockingCollection<T> как закрытая для добавления элементов.
        while (!queue.IsAddingCompleted)
        { 
            try
            {
                // Создаем новый объект данных о температуре
                var data = new TemperatureMarks
                {
                    Timestamp = DateTime.UtcNow, // Текущее время UTC
                    Value = random.NextDouble() * 100 // Случайное значение от 0 до 100
                };

                // Добавляем данные в очередь
                queue.Add(data);
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
}
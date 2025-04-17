using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace ProducerConsumer
{
    // Класс для хранения данных о температуре
    public class TemperatureMarks
    {
        // Временная метка 
        public DateTime Timestamp { get; set; }

        // Значение температуры
        public double Value { get; set; }
    }

    // Статический класс для общей очереди
    public static class SharedQueue
    {
        // BlockingCollection<T> — это потокобезопасный класс коллекции, который используется для организации обмена данными между потоками
        // BlockingCollection автоматически управляет доступом к очереди из разных потоков
        // BlockingCollection<T> автоматически управляет доступом к данным из разных потоков, что исключает необходимость ручной синхронизации
        public static BlockingCollection<TemperatureMarks> Queue { get; } = new BlockingCollection<TemperatureMarks>();
    }

    // Класс Producer
    public class Producer
    {
        // Генератор случайных чисел для имитации значений температуры
        private readonly Random _random = new Random();

        // Метод для генерации данных, возвращает Task, чтобы после его можно было выполнить паралельно с другими Task
        public async Task GenerateDataAsync()
        {
            // Бесконечный цикл для постоянной генерации данных
            while (true)
            {
                // Создаем новый объект данных о температуре
                var data = new TemperatureMarks
                {
                    Timestamp = DateTime.UtcNow, // Текущее время UTC
                    Value = _random.NextDouble() * 100 // Случайное значение от 0 до 100
                };

                // Добавляем данные в общую очередь
                SharedQueue.Queue.Add(data);

                // Выводим сообщение о создании данных
                Console.WriteLine($"Produced at {data.Timestamp:HH:mm:ss} - Temperature: {data.Value:F2}");

                // Имитация задержки между генерацией данных (500 мс)
                await Task.Delay(500);
            }
        }
    }

    // Класс Consumer
    public class Consumer
    {
        // Метод для обработки данных, возвращает Task, чтобы после его можно было выполнить паралельно с другими Task
        public async Task ProcessDataAsync()
        {
            // Перебираем все элементы из очереди
            // Поток блокируется, если очередь пуста
            foreach (var data in SharedQueue.Queue.GetConsumingEnumerable())
            {
                // Проверяем, является ли значение температуры аномальным (> 80)
                if (data.Value > 80)
                {
                    // Выводим предупреждение о высокой температуре
                    Console.WriteLine($"ВНИМАНИЕ! Высокая температура: {data.Value:F2} зафиксирована в {data.Timestamp:HH:mm:ss}");
                }
                else
                {
                    // Выводим сообщение об успешной обработке данных
                    Console.WriteLine($"Consumed at {data.Timestamp:HH:mm:ss} - Temperature: {data.Value:F2}");
                }

                // Имитация времени обработки данных (300 мс)
                await Task.Delay(300);
            }
        }
    }

    // Главный класс программы
    class Program
    {
        // Главный метод приложения
        static async Task Main(string[] args)
        {
            // Информируем пользователя о том, как завершить программу
            Console.WriteLine("Нажмите Ctrl+C, чтобы остановить мониторинг...");

            // Обработка события нажатия Ctrl+C для корректного завершения работы
            Console.CancelKeyPress += (s, e) =>
            {
                // s - Sender, Представляет объект, который инициировал событие. В данном случае это объект Console
                // e - EventArguments, Это объект типа ConsoleCancelEventArgs, который содержит информацию о событии
                // Cancel (bool): Если установить e.Cancel = true, программа не завершится сразу после нажатия Ctrl+C. Это позволяет выполнить дополнительные действия перед завершением

                // Сообщаем очереди, что больше не будет новых данных
                // CompleteAdding помечает экземпляры BlockingCollection<T> как не допускающие добавления дополнительных элементов
                SharedQueue.Queue.CompleteAdding();

                // Отменяем стандартное поведение завершения программы
                e.Cancel = true;
            };

            // Создаем экземпляры производителя и потребителя
            var producer = new Producer();
            var consumer = new Consumer();

            // Запускаем задачи для производителя и потребителя
            var producerTask = producer.GenerateDataAsync();
            var consumerTask = consumer.ProcessDataAsync();

            // Ждем завершения работы потребителя
            await consumerTask;

            Console.WriteLine("Мониторинг за температурой завершен");
        }
    }
}
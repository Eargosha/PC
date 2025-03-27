#include <iostream>
#include <vector>
#include <thread>
#include <chrono>

using namespace std;

// Функция для проведения тестирования численного интегрирования
// с разным количеством потоков. 
// a Начало интервала интегрирования
// b Конец интервала интегрирования
// total_steps Общее количество шагов интегрирования
// threads_nums Вектор с количествами потоков для тестирования
void run_integration_test(double a, double b, int total_steps, const vector<int>& threads_nums) {
    vector<double> execution_times;  // Вектор для хранения времени выполнения каждого теста

    // Вывод информации о параметрах тестирования
    cout << "\nTesting integration from " << a << " to " << b 
         << " with " << total_steps << " steps\n";
    cout << "Thread counts to test: [";
    for (size_t i = 0; i < threads_nums.size(); ++i) {
        cout << threads_nums[i];
        if (i != threads_nums.size() - 1) cout << ", ";
    }
    cout << "]\n\n";

    // Основной цикл тестирования для каждого количества потоков
    for (int threads_num : threads_nums) {
        vector<thread> threads;       // Вектор для хранения рабочих потоков
        vector<double> results(threads_num, 0.0);  // Результаты каждого потока
        vector<char> symbols = {'|', '.', '-', '+', '*', '#', '@', '~', '^', '&'};  // Символы для визуализации прогресса

        // Засекаем время начала выполнения
        auto start_time = chrono::high_resolution_clock::now();

        // Разделяем интервал интегрирования на части для каждого потока
        double chunk_size = (b - a) / threads_num;
        double current = a;  // Текущая граница интервала

        // Создаем и запускаем потоки
        for (int i = 0; i < threads_num; ++i) {
            double end = current + chunk_size;
            // Последний поток получает оставшуюся часть интервала
            if (i == threads_num - 1) end = b;

            // Вычисляем количество шагов для текущего потока
            int steps_per_thread = total_steps / threads_num;
            // Последний поток получает оставшиеся шаги
            if (i == threads_num - 1) {
                steps_per_thread = total_steps - (threads_num - 1) * steps_per_thread;
            }

            // Выбираем символ для визуализации прогресса этого потока
            char symbol = symbols[i % symbols.size()];
            
            // Создаем поток для интегрирования своей части интервала
            threads.emplace_back(integrate_part, current, end, 
                               steps_per_thread, symbol, &results[i]);
            current = end;  // Сдвигаем границу для следующего потока
        }

        // Ожидаем завершения всех потоков
        for (auto& t : threads) {
            if (t.joinable()) t.join();
        }

        // Суммируем результаты всех потоков
        double total_integral = 0;
        for (int i = 0; i < threads_num; ++i) {
            result_callback(results[i], i);  // Выводим результат каждого потока
            total_integral += results[i];   // Суммируем частичные результаты
        }

        // Засекаем время окончания и вычисляем длительность
        auto end_time = chrono::high_resolution_clock::now();
        auto duration = chrono::duration_cast<chrono::milliseconds>(end_time - start_time);
        execution_times.push_back(duration.count());  // Сохраняем время выполнения

        // Выводим информацию о текущем тесте
        cout << "Threads: " << threads_num << ", Time: " << duration.count() << " ms\n";
        cout << "Total integral: " << total_integral 
             << " (expected: ~" << (1 - cos(b)) << ")\n\n";
    }

    // Вывод результатов в формате для построения графиков
    cout << "\nResults for plots:\n";
    cout << "Threads = [";
    for (size_t i = 0; i < threads_nums.size(); ++i) {
        cout << threads_nums[i];
        if (i != threads_nums.size() - 1) cout << ", ";
    }
    cout << "]\n";
    
    cout << "Times = [";
    for (size_t i = 0; i < execution_times.size(); ++i) {
        cout << execution_times[i];
        if (i != execution_times.size() - 1) cout << ", ";
    }
    cout << "]\n";
}
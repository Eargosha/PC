#include <cmath>         
#include <iostream>      
#include <vector>        
#include <thread>        // Работа с потоками
#include <chrono>        // Замер времени

using namespace std;

// Функция для численного интегрирования методом прямоугольников
double integrate_part(double a, double b, int steps, char symbol, double* result) {
    double sum = 0;
    double step_size = (b - a) / steps; // Шаг интегрирования

    for (size_t i = 0; i < steps; ++i) {
        double x = a + i * step_size + step_size / 2; // Середина прямоугольника
        sum += sin(x) * step_size; // Площадь прямоугольника

        // Вывод символа прогресса через равные интервалы
        if (i % (steps / 10) == 0) {
            cout << symbol << flush; // flush для немедленного вывода
        }
    }

    *result = sum; // Сохранение результата в переданную переменную
    return sum;
}

// Callback-функция для обработки результатов потока
void result_callback(double result, int thread_id) {
    cout << "\nThread " << thread_id << " result: " << result << endl;
}

int main() {
    double a, b;              // Границы интегрирования
    int total_steps;          // Общее количество шагов
    int threads_num;          // Количество потоков

    // Ввод параметров
    cout << "Enter integration interval [a, b]: ";
    cin >> a >> b;
    cout << "Enter total number of steps: ";
    cin >> total_steps;
    cout << "Enter number of threads: ";
    cin >> threads_num;

    // Проверка корректности введенных данных
    if (a >= b || total_steps < 1 || threads_num < 1) {
        cerr << "Invalid parameters!" << endl;
        return 1;
    }

    // Символы для отображения прогресса разных потоков
    vector<char> symbols = { '|', '.', '-', '+', '*', '#', '@', '~', '^', '&' };

    // Вектор для хранения потоков
    vector<thread> threads;

    // Вектор для хранения результатов каждого потока
    vector<double> results(threads_num, 0.0);

    // Старт замера времени выполнения
    const auto start_time = chrono::high_resolution_clock::now();

    // Распределение работы между потоками (Декомпозиция по данным)
    double chunk_size = (b - a) / threads_num; // Размер интервала для одного потока
    double current = a;                       // Текущая позиция

    for (size_t i = 0; i < threads_num; ++i) {
        double end = current + chunk_size; // Конец интервала для текущего потока
        if (i == threads_num - 1) { // Последний поток
            end = b; // Последний поток берет остаток интервала
        }

        int steps_per_thread = total_steps / threads_num; // Количество шагов для потока
        if (i == threads_num - 1) {
            steps_per_thread = total_steps - (threads_num - 1) * steps_per_thread; // Остаток шагов
        }

        char symbol = symbols[i % symbols.size()]; // Выбор символа

        // Создание потока
        threads.emplace_back(integrate_part, current, end, steps_per_thread, symbol, &results[i]);

        current = end; // Обновление текущей позиции
    }

    // Ожидание завершения всех потоков
    for (auto& t : threads) {
        if (t.joinable()) {
            t.join();
        }
    }

    // Сбор и обработка результатов
    double total_integral = 0;
    for (int i = 0; i < threads_num; ++i) {
        result_callback(results[i], i);
        /// ????
        total_integral += results[i];
    }

    // Фиксация времени окончания и расчет длительности
    const auto end_time = chrono::high_resolution_clock::now();
    const auto duration = chrono::duration_cast<chrono::milliseconds>(end_time - start_time);

    // Вывод финальных результатов
    cout << "\nTotal integral: " << total_integral << endl;
    cout << "Execution time: " << duration.count() << " ms" << endl;

    return 0;
}

// НА МОДУЛИ 
// фигня с суммой  на нуле выдало не нуль
// 
// 
// Задание.Несколько потоков
// Создать параллельный алгоритм решения некоторой задачи(см.задание про отдельный поток в приложении с GUI), в котором отдельные потоки используют общую память.
// Организовать синхронизацию потоков с помощью встроенных средств языка программирования(мьютексы).Параметры задачи(число подзадач) и число потоков задаются пользователем.
// Оцените время выполнения программы для разного числа потоков.Постройте график.Определите оптимальное число потоков.
// График 

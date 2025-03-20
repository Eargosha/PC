#include <cmath>         
#include <iostream>      
#include <vector>        
#include <future>        // Асинхронные операции
#include <thread>        // Работа с потоками
#include <chrono>        // Замер времени
#include <functional>    

using namespace std;

// Функция для вычисления части суммы с выводом прогресса
// start - начальный индекс, end - конечный индекс
// symbol - символ для отображения прогресса
double calculate_part(unsigned long long start,
    unsigned long long end,
    char symbol)
{
    const unsigned long long range = end - start;
    const unsigned long long step = max(1ULL, range / 10); // Шаг для прогресса (10% от диапазона)
    double sum = 0;

    // Основной цикл вычислений
    for (unsigned long long i = start; i < end; ++i) {
        sum += sin(i); // Накопление суммы синусов

        // Вывод символа прогресса через равные интервалы
        if ((i - start) % step == 0) {
            cout << symbol << flush; // flush для немедленного вывода
        }
    }
    return sum;
}

// Callback-функция для обработки результатов потока
// result - вычисленная сумма
// thread_id - идентификатор потока
void result_callback(double result, int thread_id) {
    cout << "\nThread " << thread_id << " result: " << result << endl;
}

int main() {
    unsigned long long N;         // Общее количество итераций
    int threads_num;              // Количество потоков

    // Ввод параметров
    cout << "Enter iterations count: ";
    cin >> N;
    cout << "Enter threads number: ";
    cin >> threads_num;

    // Проверка корректности введенных данных
    if (N < 1 || threads_num < 1) {
        cerr << "Invalid parameters!" << endl;
        return 1;
    }

    // Символы для отображения прогресса разных потоков
    vector<char> symbols = { '|', '.', '-', '+', '*', '#', '@', '~', '^', '&' };

    // Вектор для хранения будущих результатов
    // future говорит о том, что значения будут записаны в будущем
    vector<future<double>> futures;

    // Старт замера времени выполнения
    const auto start_time = chrono::high_resolution_clock::now();

    // Распределение работы между потоками (Декомпозиция по данным)
    unsigned long long chunk = N / threads_num;  // Базовый размер блока
    unsigned long long extra = N % threads_num;  // Остаток для распределения (число данных % кол-во потоков)
    unsigned long long current = 0;              // Текущая позиция

    // Создание асинхронных задач
    for (int i = 0; i < threads_num; ++i) {
        // Расчет границ для текущего потока
        const unsigned long long end = current + chunk + (i < extra ? 1 : 0);
        const char symbol = symbols[i % symbols.size()]; // Выбор символа

        // Запуск асинхронной задачи:
        // - launch::async гарантирует выполнение в отдельном потоке
        // - calculate_part - целевая функция
        // - current, end, symbol - аргументы функции
        futures.push_back(async(launch::async, calculate_part, current, end, symbol));

        current = end; // Обновление текущей позиции
    }

    // Сбор и обработка результатов
    double total = 0;
    for (int i = 0; auto & f : futures) {
        // Ожидание результата и получение значения
        double result = f.get();

        result_callback(result, i++);

        total += result;
    }

    // Фиксация времени окончания и расчет длительности
    const auto end_time = chrono::high_resolution_clock::now();
    const auto duration = chrono::duration_cast<chrono::milliseconds>(end_time - start_time);

    // Вывод финальных результатов
    cout << "\nTotal sum: " << total << endl;
    cout << "Execution time: " << duration.count() << " ms" << endl;

    return 0;
}
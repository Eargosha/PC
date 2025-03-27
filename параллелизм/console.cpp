#include <iostream>
#include <vector>
#include "integration.cpp"
#include "threadTest.cpp"

using namespace std;

int main() {
    double a, b;            // Границы интегрирования
    int total_steps;        // Общее количество шагов
    vector<int> threads_nums; // Вектор для хранения чисел потоков для тестирования
    bool custom_input;      // Флаг выбора режима ввода

     // Запрос режима работы программы
    cout << "Use custom parameters? (0 - default, 1 - custom): ";
    cin >> custom_input;

    if (custom_input) {
        // Ручной ввод параметров
        cout << "Enter integration interval [a, b]: ";
        cin >> a >> b;
        cout << "Enter total number of steps: ";
        cin >> total_steps;
        
        // Ввод количества потоков (0 - завершение ввода)
        int num_threads;
        cout << "Enter number of threads to test (enter 0 to finish): ";
        threads_nums.clear();
        while (cin >> num_threads && num_threads != 0) {
            threads_nums.push_back(num_threads);
        }
    } else {
        // Параметры по умолчанию
        a = 0;                     // Начало интервала
        b = 3.14;                  // Конец интервала
        total_steps = 100'000'000;  // 100 миллионов шагов
        // Стандартные значения потоков для тестирования
        threads_nums = {1, 2, 4, 8, 16, 32, 64, 512, 1024, 2048};
    }

    // Запускаем само тестирование с полученными параметрами
    run_integration_test(a, b, total_steps, threads_nums);

    return 0;
}
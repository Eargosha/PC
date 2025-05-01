// 178а Даны натуральные числа n, a 1...an. 
// Определить количество членов ak последовательности a1,...,an:
// a) являющихся нечетными числами;
// https://ivtipm.github.io/Programming/Glava07/index07.htm#z178


#include <iostream>
#include <omp.h>
#include <vector>
#include <cstdlib>
#include <chrono>
// g++ -fopenmp main.cpp -o main === -fopenmp -- флаг: подключить OpenMP и другие нужные OpenMP библиотеки
int main()
{
    using namespace std::chrono;
    int n;
    std::cout << "arrrray size? n: ";
    std::cin >> n;
    // Последовательный код

    // Генерация массива случайных натуральных чисел
    std::vector<int> a(n);
// распараллеливание цикла
// Итерации цикла делятся между потоками (например, поток 1 обрабатывает 1000 эл, второй след)
#pragma omp parallel for
    for (int i = 0; i < n; ++i)
    {                             // fork (создание функции на основе блока кода, создание нескольких потоков, запуск потоков с функцией выполнения)
        a[i] = rand() % 1000 + 1; // числа от 1 до 1000
    } // join (ожидание завершения всех потоков.)

    // Подсчёт нечётных элементов с использованием OpenMP
    int odd_count = 0;

// Метод 1: С использованием reduction
// распараллеливание цикла - omp parallel for
// reduction(+:odd_count) - автоматическая сумма результатов odd_count из всех потоков.
// schedule(static)  - равномерное распределение итераций по потокам
auto reduction_start = high_resolution_clock::now();
#pragma omp parallel for reduction(+ : odd_count) schedule(static)
    for (int i = 0; i < n; ++i)
    {
        if (a[i] % 2 != 0)
        {
            // Увеличиваем локальную копию переменной
            odd_count++;
        }
    } // После завершения всех потоков локальные значения суммируются в общую переменную (+:odd_count)

    auto reduction_end = high_resolution_clock::now();
    std::cout << "REDUCTION: count of odd elements: " << odd_count << std::endl << "DURATION: " << duration_cast<microseconds>(reduction_end-reduction_start).count() << std::endl;


    // Метод 2: С использованием critical
    //  schedule - Стратегии планирования, тут schedule(guided) уменьшающиеся блоки для лучшего баланса
    odd_count = 0;
    auto critical_start = high_resolution_clock::now();
#pragma omp parallel for schedule(guided)
    for (int i = 0; i < n; ++i)
    {
        if (a[i] % 2 != 0)
        {
#pragma omp critical
            odd_count++;
            // critical Cоздаёт критическую секцию, гарантируя,
            // что только один поток выполняет код внутри блока в любой
            // момент времени
        }
    }
    auto critical_end = high_resolution_clock::now();
    std::cout << "CRITICAL: " << odd_count << " count of odd elements\n" << "DURATION: " << duration_cast<microseconds>(critical_end - critical_start).count() << std::endl;




    // Метод 2: С использованием atomic
    // Гарантирует атомарное обновление одной переменной без необходимости захвата 
    // полноценной критической секции. Особенно эффективна для простых операций, 
    // таких как инкремент или декремент, поскольку многие архитектуры поддерживают 
    // аппаратные атомарные команды
    odd_count = 0;
    // schedule(dynamic, 1000) - динамическое распределение блоками по 1000
    auto atomic_start = high_resolution_clock::now();
#pragma omp parallel for schedule(dynamic, 1000)
    for (int i = 0; i < n; ++i)
    {
        if (a[i] % 2 != 0)
        {
#pragma omp atomic
            odd_count++;
        }
    }
    auto atomic_end = high_resolution_clock::now();
    std::cout << "ATOMIC: " << odd_count << " count of odd elements\n" << "DURATION: " << duration_cast<microseconds>(atomic_end - atomic_start).count() << std::endl;

    return 0;
}

// omp_get_thread_num(); - получить номер потокка в блоке паралллельного кода
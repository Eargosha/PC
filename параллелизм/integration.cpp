#include <iostream>
#include <vector>
#include <thread>
#include <chrono>
#include "integration.h"

using namespace std;

// Функция для численного интегрирования методом прямоугольников
// a - начало интервала, b - конец интервала
// steps - количество шагов интегрирования
// symbol - символ для визуализации прогресса
// result - указатель для сохранения результата
// func - указатель на функцию по которой будет производиться интегрирование 
double integrate_part(double (*func)(double), double a, double b, int steps, char symbol, double* result) {
    double sum = 0;
    double step_size = (b - a) / steps; // Размер одного шага

    // Основной цикл интегрирования
    for (int i = 0; i < steps; ++i) {
        // Вычисляем середину текущего интервала (метод средних прямоугольников)
        double x = a + i * step_size + step_size / 2;
        sum += func(x) * step_size; // Добавляем площадь текущего прямоугольника

        // Визуализация прогресса (выводим символ каждые 10% выполнения)
        if (i % (steps / 10) == 0) {
            cout << symbol << flush; // flush для немедленного вывода
        }
    }

    *result = sum; // Сохраняем результат по переданному указателю
    return sum;
}

// Функция для вывода результата работы одного потока
void result_callback(double result, int thread_id) {
    cout << "Thread " << thread_id << " result: " << result << endl;
}

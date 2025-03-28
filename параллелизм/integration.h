#pragma once

#ifndef HEADER_H
#define HEADER_H

#include <vector>


// Функция для численного интегрирования методом прямоугольников
// a - начало интервала, b - конец интервала
// steps - количество шагов интегрирования
// symbol - символ для визуализации прогресса
// result - указатель для сохранения результата
// func - указатель на функцию по которой будет производиться интегрирование 
double integrate_part(double (*func)(double), double a, double b, int steps, char symbol, double* result);
// Функция для вывода результата работы одного потока
void result_callback(double result, int thread_id);
// Функция для проведения тестирования численного интегрирования
// с разным количеством потоков. 
// a Начало интервала интегрирования
// b Конец интервала интегрирования
// total_steps Общее количество шагов интегрирования
// threads_nums Вектор с количествами потоков для тестирования
void run_integration_test(double (*func_to_int)(double), double a, double b, int total_steps, const std::vector<int>& threads_nums);

#endif //


// Кастомная задача:
// Найти все уникальные пары элементов массива, 
// сумма которых равна заданному числу K, и вывести их количество.

#include <iostream>  
#include <vector>    
#include <unordered_set> 
#include <omp.h>       
#include <cstdlib>    
#include <chrono>     // Для измерения времени

int main() {
    using namespace std::chrono;
    
    int n, K;
    std::cout << "array size: ";
    std::cin >> n;  // Размер массива
    std::cout << "what K: ";
    std::cin >> K;   // Целевая сумма пар
    
    // 1. Генерация массива случайных чисел
    auto gen_start = high_resolution_clock::now();
    std::vector<int> arr(n);  // Создаем массив размера n
    
    // Параллельная генерация элементов массива
    #pragma omp parallel for  
    for (int i = 0; i < n; ++i) {
        arr[i] = rand() % 10 + 1;  // Генерируем числа от 1 до 10
    }
    auto gen_end = high_resolution_clock::now();

    std::cout << "Array: ";
    for (int num : arr) {
        std::cout << num << " ";  // Выводим элементы массива
    }
    std::cout << "\n";

    // 3. Поиск пар с суммой K
    auto search_start = high_resolution_clock::now();
    int pair_count = 0;  // Счетчик найденных пар
    std::unordered_set<int> seen;  // Множество для хранения просмотренных элементов

    // Параллельный поиск пар с использованием OpenMP
    #pragma omp parallel for reduction(+:pair_count) schedule(dynamic, 100)
    for (int i = 0; i < n; ++i) {
        int target = K - arr[i];  // Вычисляем искомое число для текущего элемента
        
        if (seen.count(target)) {
            #pragma omp critical
            {
                std::cout << "Pair: (" << target << ", " << arr[i] << ")\n";
            }
            pair_count++;
        }
        
        #pragma omp critical
        {
            seen.insert(arr[i]);
        }
    }
    auto search_end = high_resolution_clock::now();

    std::cout << "Count of the pairs " << K << ": " << pair_count << "\n";
    
    
    std::cout << "\nExecution times:\n";
    std::cout << "Generation time: " 
              << duration_cast<microseconds>(gen_end - gen_start).count() 
              << " nanos\n";
    std::cout << "Search time:     " 
              << duration_cast<microseconds>(search_end - search_start).count() 
              << " nanos\n";
    std::cout << "Total time:      " 
              << duration_cast<microseconds>((gen_end - gen_start) + (search_end - search_start)).count() 
              << " nanos\n";
    
    return 0;
}
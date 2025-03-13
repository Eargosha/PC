import argparse

import numpy as np

# Импортируем все задачи из модуля
from tasks import *


def get_matrix_from_user():
    """Функция для получения матрицы от пользователя."""
    rows = int(input("Введите количество строк матрицы: "))
    cols = int(input("Введите количество столбцов матрицы: "))
    matrix = []
    print("Введите элементы матрицы построчно (через пробел):")
    for i in range(rows):
        row = list(map(float, input(f"Строка {i + 1}: ").split()))
        if len(row) != cols:
            raise ValueError("Количество элементов в строке не соответствует количеству столбцов.")
        matrix.append(row)
    return np.array(matrix)


def parse_matrix(matrix_str):
    """Преобразует строку матрицы (из аргументов) в массив NumPy."""
    rows = [list(map(float, row.split(','))) for row in matrix_str.split(';')]
    return np.array(rows)


def main(args=None):
    # Создаем парсер аргументов командной строки
    parser = argparse.ArgumentParser(description="Программа для выполнения различных математических задач.")

    # Добавляем аргумент для выбора задачи
    parser.add_argument("--task", type=str, choices=["136", "178", "335", "677"],
                        help="Выбор задачи: 136, 178, 335 или 677.")

    # Аргументы для задачи 136
    parser.add_argument("--n", type=int, help="Натуральное число n (длина последовательности).")
    parser.add_argument("--a", type=str, help="Список чисел через запятую для задачи 136.")

    # Аргументы для задачи 178
    parser.add_argument("--a178", type=str, help="Список целых чисел через запятую для задачи 178.")

    # Аргументы для задачи 335
    parser.add_argument("--n335", type=int, help="Натуральное число n для задачи 335.")

    # Аргументы для задачи 677
    parser.add_argument("--matrix", type=str,
                        help="Матрица в формате 'row1;row2;...', где строки разделены ';', а элементы строки - ','.")
    parser.add_argument("--operation", type=str, choices=["sum", "prod", "max", "min"],
                        help="Операция для задачи 677: 'sum', 'prod', 'max' или 'min'.")

    # Парсим аргументы командной строки
    args = parser.parse_args(args)

    try:
        # Если переданы аргументы командной строки
        if args.task:
            if args.task == "136":
                if args.n is None or args.a is None:
                    raise ValueError("Для задачи 136 необходимо указать --n и --a.")
                n = args.n
                a = list(map(float, args.a.split(',')))
                if len(a) != n:
                    raise ValueError("Количество чисел в --a не соответствует --n.")
                result = task136(n, a)
                print(f"Результат задачи 136: {result}")

            elif args.task == "178":
                if args.n is None or args.a178 is None:
                    raise ValueError("Для задачи 178 необходимо указать --n и --a178.")
                n = args.n
                a = list(map(int, args.a178.split(',')))
                if len(a) != n:
                    raise ValueError("Количество чисел в --a178 не соответствует --n.")
                result = task178(n, a)
                print(f"Результат задачи 178: {result}")

            elif args.task == "335":
                if args.n335 is None:
                    raise ValueError("Для задачи 335 необходимо указать --n335.")
                n = args.n335
                result = task335(n)
                print(f"Результат задачи 335: {result}")

            elif args.task == "677":
                if args.matrix is None or args.operation is None:
                    raise ValueError("Для задачи 677 необходимо указать --matrix и --operation.")
                matrix = parse_matrix(args.matrix)
                operation = args.operation
                result = task677(matrix, operation)
                if operation in ["max", "min"]:
                    print(f"Результат задачи 677 ({operation}): {result}")
                else:
                    print(f"Результат задачи 677 ({operation}):\n{result}")

        # Если аргументы командной строки не переданы, используем текстовый интерфейс
        else:
            print("Доступные задачи:")
            print("1. Задача 136: Вычисление суммы a1/1! + a2/2! + ... + an/n!")
            print("2. Задача 178: Подсчет количества элементов, кратных 3 и не кратных 5")
            print("3. Задача 335: Вычисление суммы Sum(1/(k^2)!) from 1 to n")
            print("4. Задача 677: Обработка матрицы с выбором операции ('sum', 'prod', 'max', 'min')")
            choice = input("Выберите задачу (введите номер): ")

            if choice == "1":
                n = int(input("Введите натуральное число n: "))
                if n <= 0:
                    raise ValueError("n должно быть натуральным числом (n ≥ 1)")
                print(f"Введите {n} действительных чисел через пробел:")
                a = list(map(float, input().split()))
                if len(a) != n:
                    raise ValueError("Количество введенных чисел не соответствует n")
                result = task136(n, a)
                print(f"Результат: {result}")

            elif choice == "2":
                n = int(input("Введите натуральное число n: "))
                if n <= 0:
                    raise ValueError("n должно быть натуральным числом (n ≥ 1)")
                print(f"Введите {n} целых чисел через пробел:")
                a = list(map(int, input().split()))
                if len(a) != n:
                    raise ValueError("Количество введенных чисел не соответствует n")
                result = task178(n, a)
                print(f"Количество элементов, кратных 3 и не кратных 5: {result}")

            elif choice == "3":
                n = int(input("Введите натуральное число n: "))
                if n <= 0:
                    raise ValueError("n должно быть натуральным числом (n ≥ 1)")
                result = task335(n)
                print(f"Результат: {result}")

            elif choice == "4":
                print("Введите матрицу:")
                matrix = get_matrix_from_user()
                print("Выберите операцию: 'sum', 'prod', 'max', 'min'")
                operation = input("Операция: ").lower()
                if operation not in ['sum', 'prod', 'max', 'min']:
                    raise ValueError("Недопустимое значение операции. Выберите из 'sum', 'prod', 'max', 'min'.")
                result = task677(matrix, operation)
                if operation in ['max', 'min']:
                    print(f"Результат ({operation}): {result}")
                else:
                    print(f"Результат ({operation}):\n{result}")

            else:
                print("Неверный выбор задачи.")

    except ValueError as e:
        print(f"Ошибка: {e}")
    except Exception as e:
        print(f"Произошла ошибка: {e}")


if __name__ == "__main__":
    # Если программа запущена без аргументов командной строки, вызываем main() без параметров
    import sys
    if len(sys.argv) > 1:
        # Если переданы аргументы командной строки, используем их
        main()
    else:
        # Если аргументы командной строки отсутствуют, используем текстовый интерфейс
        main(args=[])
import numpy as np
import unittest

# Дана действительная матрица [aij]i, i = 1, ..., n. Получить действительную матрицу [bij]i, i = 1, ..., n
# элемент bij которой равен сумме элементов данной матрицы расположенных в области, определяемой индексами i,j так,
# как показано на рисунке (область заштрихована). Сходным образом можно рассмотреть вместо суммы элементов их
# произведение, наибольшее значение, наименьшее значение.
def task677(a):
    """
    :param a: Исходная матрица a. Это объект типа ndarray.
    :param operation: Тип операции ('sum', 'prod', 'max', 'min').
    :return: Матрица b с результатами выбранной операции.
    """
    a = np.array(a)
    n = a.shape[0]
    res = np.zeros_like(a, dtype=float)
    for i in range(n):
        for j in range(n):
            sub = a[i:, :j + 1]
            res[i, j] = np.sum(sub)
    return res



# Класс для тестирования функции obfuscated_task
class TestObfuscatedTask(unittest.TestCase):
    def test_2x2_matrix(self):
        input_matrix = np.array([[1, 2], [3, 4]])
        expected_output = np.array([[4, 10], [3, 7]])
        result = task677(input_matrix)
        self.assertTrue(np.array_equal(result, expected_output), f"Тест 1 провален: {result} != {expected_output}")

    def test_3x3_matrix(self):
        input_matrix = np.array([[1, 2, 3], [4, 5, 6], [7, 8, 9]])
        expected_output = np.array([[12, 27, 45], [11, 24, 39], [7, 15, 24]])
        result = task677(input_matrix)
        self.assertTrue(np.array_equal(result, expected_output), f"Тест 2 провален: {result} != {expected_output}")

    def test_1x1_matrix(self):
        input_matrix = np.array([[5]])
        expected_output = np.array([[5]])
        result = task677(input_matrix)
        self.assertTrue(np.array_equal(result, expected_output), f"Тест 3 провален: {result} != {expected_output}")

    def test_zero_matrix(self):
        input_matrix = np.array([[0, 0], [0, 0]])
        expected_output = np.array([[0, 0], [0, 0]])
        result = task677(input_matrix)
        self.assertTrue(np.array_equal(result, expected_output), f"Тест 4 провален: {result} != {expected_output}")


# Основная функция для запуска тестов
def main():
    example_matrix = np.array([[1, 2, 3], [4, 5, 6], [7, 8, 9]])
    print("Исходная матрица:")
    print(example_matrix)

    result_matrix = task677(example_matrix)
    print("\nРезультирующая матрица:")
    print(result_matrix)

    # Запуск всех тестов
    print("\nЗапуск тестов...")
    unittest.main()

if __name__ == "__main__":
    main()

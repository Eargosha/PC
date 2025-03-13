__author__ = "Eargosha"
import unittest
from tasks import *
from myMath import *


class TestTask136(unittest.TestCase):
    def test_basic(self):
        self.assertAlmostEqual(task136(3, [1.0, 1.0, 1.0]), 1.6666666666666667)

    def test_single_element(self):
        self.assertAlmostEqual(task136(1, [5.0]), 5.0)

    def test_factorial_growth(self):
        self.assertAlmostEqual(task136(3, [1.0, 2.0, 3.0]), 2.5)

    def test_invalid_input(self):
        with self.assertRaises(ValueError):
            task136(0, [1.0])  # Неверное n
        with self.assertRaises(ValueError):
            task136(2, [1.0])  # Длина списка не совпадает с n



class TestCountElements(unittest.TestCase):
    def test_valid_case(self):
        n = 10
        a = [3, 5, 9, 12, 15, 18, 20, 21, 24, 30]
        self.assertEqual(task178(n, a), 6)

    def test_empty_list(self):
        n = 0
        a = []
        with self.assertRaises(ValueError):
            task178(n, a)

    def test_invalid_n(self):
        n = -5
        a = [3, 9, 12]
        with self.assertRaises(ValueError):
            task178(n, a)

    def test_mismatched_length(self):
        n = 5
        a = [3, 9, 12]
        with self.assertRaises(ValueError):
            task178(n, a)

    def test_no_matches(self):
        n = 5
        a = [5, 10, 20, 25, 35]
        self.assertEqual(task178(n, a), 0)

    def test_all_matches(self):
        n = 4
        a = [3, 9, 21, 24]
        self.assertEqual(task178(n, a), 4)

    def test_negative_numbers(self):
        n = 6
        a = [-3, -9, -15, -21, -30, 12]
        self.assertEqual(task178(n, a), 4)



class TestComputeSum(unittest.TestCase):
    def test_valid_case(self):
        n = 5
        expected_result = sum(1 / factorial(k ** 2) for k in range(1, n + 1))
        self.assertAlmostEqual(task335(n), expected_result, places=10)

    def test_zero_case(self):
        n = 0
        self.assertEqual(task335(n), 0.0)

    def test_large_n(self):
        n = 10
        expected_result = sum(1 / factorial(k ** 2) for k in range(1, n + 1))
        self.assertAlmostEqual(task335(n), expected_result, places=10)


class TestMatrixOperations(unittest.TestCase):
    def setUp(self):
        # Создаем тестовые матрицы
        self.matrix_4x4 = np.array([
            [1, 2, 3, 4],
            [5, 6, 7, 8],
            [9, 10, 11, 12],
            [13, 14, 15, 16]
        ])
        self.matrix_3x5 = np.array([
            [1, 2, 3, 4, 5],
            [6, 7, 8, 9, 10],
            [11, 12, 13, 14, 15]
        ])

    def test_task677_sum(self):
        result = task677(self.matrix_4x4, OPERATIONSINTASK677.Sum)
        expected = np.array([
            [9, 19],
            [22, 46]
        ])
        np.testing.assert_array_equal(result, expected)

    def test_task677_prod(self):
        result = task677(self.matrix_4x4, OPERATIONSINTASK677.Prod)
        expected = np.array([
            [9, 90],
            [117, 16380]
        ])
        np.testing.assert_array_equal(result, expected)

    def test_task677_max(self):
        result = task677(self.matrix_4x4, OPERATIONSINTASK677.Max)
        self.assertEqual(result, 14)

    def test_task677_min(self):
        result = task677(self.matrix_4x4, OPERATIONSINTASK677.Min)
        self.assertEqual(result, 9)

    def test_invalid_operation(self):
        with self.assertRaises(ValueError):
            task677(self.matrix_4x4, "invalid_operation")


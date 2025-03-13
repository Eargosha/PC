import math
# Тестовый фреймворк, встроенный в Python для написания и запуска тестов, автоматизирует процесс тестирования
import unittest
# Для красивого вывода формул. Sympy это бесплатная библиотека для символьных вычислений на языке Python.
# В символьных вычислениях компьютер работает с уравнениями и выражениями как с последовательностью символов,
# тогда как в численных оперирует приближёнными числовыми значениями.
import sympy as sp


def generate_sequence(n, formula):
    '''
    Функция для генерации последовательности
    :param n: кол-во элементов последовательности
    :param formula: формула последовательности
    '''
    for i in range(1, n + 1):
        # yield - ключевое слово, которое используется для создания генераторов.
        # Генераторы позволяют итерировать через последовательность данных,
        # не загружая всю последовательность в память сразу.
        yield formula(i)


# Функции формул (от a to и) с задачи 136, везде i является элементом последовательности
def a(i):
    return i


def b(i):
    return i ** 2


def c(i):
    return 2 ** (i + 1)


def d(i):
    return 2 ** i + 3 ** (i + 1)


def e(i):
    return (2 ** i) / math.factorial(i)


def f(i):
    return sum(1 / k for k in range(1, i + 1))


def g(i):
    return sum((-1) ** (k + 1) / k for k in range(1, i + 1))


def h(i):
    return i * sum(1 / math.factorial(k) for k in range(1, i + 1))


# Класс для тестирования
# TestCase - это класс, который содержит набор тестов. Каждый тест — это метод, начинающийся с test_.
# Утверждение self.assertEqual(a, b) — проверяет, что a равно b.

# TestSequenceGeneration есть тутошний тестовый случай (TestCase). Он нужен для того, чтобы проверить,
# что функции, которые генерируют последовательности, работают правильно.
class TestSequenceGeneration(unittest.TestCase):
    def test_a(self):
        expected = [1, 2, 3, 4]
        result = list(generate_sequence(4, a))
        self.assertEqual(result, expected)

    def test_b(self):
        expected = [1, 4, 9, 16]
        result = list(generate_sequence(4, b))
        self.assertEqual(result, expected)

    def test_c(self):
        expected = [4, 8, 16, 32]
        result = list(generate_sequence(4, c))
        self.assertEqual(result, expected)

    def test_d(self):
        expected = [11, 31, 89, 259]
        result = list(generate_sequence(4, d))
        self.assertEqual(result, expected)

    def test_e(self):
        expected = [2.0, 2.0, 1.3333333333333333, 0.6666666666666666]
        result = list(generate_sequence(4, e))
        self.assertEqual(result, expected)

    def test_f(self):
        expected = [1.0, 1.5, 1.8333333333333333, 2.0833333333333335]
        result = list(generate_sequence(4, f))
        self.assertEqual(result, expected)

    def test_g(self):
        expected = [1.0, 0.5, 0.8333333333333333, 0.5833333333333333]
        result = list(generate_sequence(4, g))
        self.assertEqual(result, expected)

    def test_h(self):
        expected = [1.0, 3.0, 5.0, 6.833333333333333]
        result = list(generate_sequence(4, h))
        self.assertEqual(result, expected)

if __name__ == '__main__':
    # unittest.main()  # Запуск тестов. Закомментировано, чтобы программа продолжала выполнение
    # Использование sympy для вывода формул
    i, k = sp.symbols('i k', integer=True, positive=True)  # Определяем i и k как целые положительные числа

    # Функция sympy.simplify возвращает символьное выражение (объект типа sympy.Expr),
    # которое представляет собой математическое выражение в символьной форме.
    # Это выражение можно дальше использовать для вычислений, подстановок, дифференцирования, интегрирования
    # и других математических операций.
    # Тут используется для вывода
    print("Формула а:", sp.simplify(a(i)))
    print("Формула б:", sp.simplify(b(i)))
    print("Формула в:", sp.simplify(c(i)))
    print("Формула г:", sp.simplify(d(i)))
    print("Формула д:", sp.simplify((2 ** i) / sp.factorial(i)))
    print("Формула е:", sp.simplify(sp.Sum(1/k, (k, 1, i))))
    print("Формула ж:", sp.simplify(sp.Sum((-1)**(k+1)/k, (k, 1, i))))
    print("Формула з:", sp.simplify(i * sp.Sum(1/sp.factorial(k), (k, 1, i))))

    n = int(input("Введите n: "))

    print("Результат а:", list(generate_sequence(n, a)))
    print("Результат б:", list(generate_sequence(n, b)))
    print("Результат в:", list(generate_sequence(n, c)))
    print("Результат г:", list(generate_sequence(n, d)))
    print("Результат д:", [f"{x:.3f}" for x in generate_sequence(n, e)])
    print("Результат е:", [f"{x:.3f}" for x in generate_sequence(n, f)])
    print("Результат ж:", [f"{x:.3f}" for x in generate_sequence(n, g)])
    print("Результат з:", [f"{x:.3f}" for x in generate_sequence(n, h)])
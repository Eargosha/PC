__author__ = "Eargosha"
# Модуль, который будем тестировать
from modules.harmonicSumm import *

"""
    Тестирует функцию calculate_harmonic_sum.
    Проверяет различные случаи:
    1. n = 1: сумма должна быть равна 1.
    2. n = 2: сумма должна быть равна 1 + 1/2.
    3. n = 3: сумма должна быть равна 1 + 1/2 + 1/3.
    4. n = 10: проверка для большего значения.
    5. n <= 0: должно вызывать ValueError.
"""


def testHarmonicSumm():
    # Тест 1: n = 1 (сумма должна быть равна 1)
    n = 1
    expected_result = 1.0  # Сумма ряда для n=1
    result = calculateHarmonicSum(n)
    assert abs(result - expected_result) < 1e-9, f"Ошибка в тесте 1: {result} != {expected_result}"

    # Тест 2: n = 2 (сумма должна быть равна 1 + 1/2)
    n = 2
    expected_result = 1 + 1 / 2  # Сумма ряда для n=2
    result = calculateHarmonicSum(n)
    assert abs(result - expected_result) < 1e-9, f"Ошибка в тесте 2: {result} != {expected_result}"

    # Тест 3: n = 3 (сумма должна быть равна 1 + 1/2 + 1/3)
    n = 3
    expected_result = 1 + 1 / 2 + 1 / 3  # Сумма ряда для n=3
    result = calculateHarmonicSum(n)
    assert abs(result - expected_result) < 1e-9, f"Ошибка в тесте 3: {result} != {expected_result}"

    # Тест 4: n = 10 (проверка для большего значения)
    n = 10
    expected_result = sum(1 / k for k in range(1, n + 1))  # Вычисляем сумму гармонического ряда
    result = calculateHarmonicSum(n)
    assert abs(result - expected_result) < 1e-9, f"Ошибка в тесте 4: {result} != {expected_result}"

    # Тест 5: n <= 0 (должно вызывать ValueError)
    try:
        calculateHarmonicSum(0)
        assert False, "Ошибка в тесте 5: ожидалось исключение ValueError"
    except ValueError:
        pass  # Исключение успешно поймано

    # Если все тесты прошли успешно
    return True
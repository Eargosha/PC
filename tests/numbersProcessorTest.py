__author__ = "Eargosha"

# Модуль, который будем тестировать
from modules.numbersProcessor import processNumbers

"""
    Тестирует функцию processNumbers.

    Проверяет различные случаи:
    1. a >= b >= c: числа должны быть удвоены.
    2. a < b или b < c: числа должны быть заменены их абсолютными значениями.
    """
def testProcessNumbers():

    # Тест 1: a >= b >= c (числа должны быть удвоены)
    a, b, c = 6, 4, 2
    expected_result = (12, 8, 4)  # Удвоенные значения
    result = processNumbers(a, b, c)
    assert result == expected_result, f"Ошибка в тесте 1: {result} != {expected_result}"

    # Тест 2: a >= b >= c (числа должны быть удвоены, случай с отрицательным числом)
    a, b, c = 0, -3, -6
    expected_result = (0, -6, -12)  # Удвоенные значения
    result = processNumbers(a, b, c)
    assert result == expected_result, f"Ошибка в тесте 2: {result} != {expected_result}"

    # Тест 3: a < b или b < c (числа должны быть заменены абсолютными значениями)
    a, b, c = 1, 3, 2
    expected_result = (1, 3, 2)  # Абсолютные значения
    result = processNumbers(a, b, c)
    assert result == expected_result, f"Ошибка в тесте 3: {result} != {expected_result}"

    # Тест 4
    a, b, c = -5, -10, -15
    expected_result = (-10, -20, -30)  # Абсолютные значения
    result = processNumbers(a, b, c)
    assert result == expected_result, f"Ошибка в тесте 4: {result} != {expected_result}"

    # Тест 5: Все числа равны (a == b == c, числа должны быть удвоены)
    a, b, c = 7, 7, 7
    expected_result = (14, 14, 14)  # Удвоенные значения
    result = processNumbers(a, b, c)
    assert result == expected_result, f"Ошибка в тесте 5: {result} != {expected_result}"

    # Если все тесты прошли успешно
    return True
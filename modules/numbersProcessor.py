__author__ = "Eargosha"

from modules.myMath import calculate_abs

"""
Обрабатывает числа a, b, c:
- Если a >= b >= c, удваивает их.
- Иначе заменяет их абсолютными значениями.

:param a: Первое число (float или int).
:param b: Второе число (float или int).
:param c: Третье число (float или int).
:return: Кортеж из обработанных чисел (a, b, c).
"""
# 37. Даны действительные числа a, b, c. Удвоить эти числа, если a ≥ b ≥ c, и заменить их абсолютными значениями, если это не так.
def processNumbers(a, b, c):
    if a >= b >= c:
        # Удваиваем числа
        a *= 2
        b *= 2
        c *= 2
    else:
        # Заменяем числа их абсолютными значениями
        a = calculate_abs(a)
        b = calculate_abs(b)
        c = calculate_abs(c)

    return a, b, c
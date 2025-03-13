__author__ = "Eargosha"

# Подключаем модули
from  modules.myMath import *

# 8. Определить периметр правильного n-угольника, описанного около окружности радиуса r.
"""Вычисление поставленной задачи #8 
Вычисление периметра правильного n-угольника, описанного около окружности радиуса r
"""

def findPerimetrOfRegularPolygon(n, r):
    # Проверка на кол-во сторон
    if n < 3:
        # ValueError - это исключение, что возникает при передаче невалидного значения - Value
        raise ValueError("Кол-во сторон многоугольника начинается с 3")

    # Проверка на радиус
    if r <= 0:
        raise ValueError("Радиус окружности должен быть положительным числом.")

    # Вычисляем угол в радинаха
    angle = PI / n

    # Находим синус и косинус угла
    sin_angle = calculate_sin(angle)
    cos_angle = calculate_cos(angle)

    # Вот и сам периметр
    perimeter = 2 * r * n * sin_angle * cos_angle

    return perimeter

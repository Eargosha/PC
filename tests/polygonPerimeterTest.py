__author__ = "Eargosha"

# Модуль, что будем тестировать
from modules.polygonPerimeter import findPerimetrOfRegularPolygon

# Тестирование функции findPerimetrOfRegularPolygon
def testPolygonPerimeter():
    # Тест 1: Правильный треугольник (n=3) с радиусом r=1
    expected_perimeter = (3 * (3 ** 0.5)) / 2  # (3 * sqrt(3))/2 ≈ 2,598
    calculated_perimeter = findPerimetrOfRegularPolygon(3, 1)
    assert abs(
        calculated_perimeter - expected_perimeter) < 1e-6, f"Ошибка в тесте 1: {calculated_perimeter} != {expected_perimeter}"

    # Тест 2: Правильный квадрат (n=4) с радиусом r=1
    # P = 2 * 4 * 1 * sin(pi/4) * cos(pi/4) = 4 * 2 * (sqrt(2)/2) * (sqrt(2)/2) = 4 * 1 = 4
    expected_perimeter = 4
    calculated_perimeter = findPerimetrOfRegularPolygon(4, 1)
    assert abs(
        calculated_perimeter - expected_perimeter) < 1e-6, f"Ошибка в тесте 2: {calculated_perimeter} != {expected_perimeter}"

    # Тест 3: Правильный шестиугольник (n=6) с радиусом r=2
    expected_perimeter = (12 * (3 ** 0.5)) / 2  # (12 * sqrt(3))/2 ≈ 5,196
    calculated_perimeter = findPerimetrOfRegularPolygon(6, 2)
    assert abs(
        calculated_perimeter - expected_perimeter) < 1e-6, f"Ошибка в тесте 3: {calculated_perimeter} != {expected_perimeter}"

    # Тест 4: Исключение
    try:
        findPerimetrOfRegularPolygon(0, 22)
        assert False, "Ошибка в тесте 6: ожидалось исключение ValueError"
    except ValueError:
        pass  # Исключение успешно поймано

    return True
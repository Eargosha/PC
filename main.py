import argparse

__author__ = "Eargosha"

from modules.chees import *
from modules.harmonicSumm import *
from modules.numbersProcessor import *
from modules.polygonPerimeter import *
from tests.cheesTest import testIsSameColor
from tests.harmonicSummTest import testHarmonicSumm
from tests.numbersProcessorTest import testProcessNumbers
from tests.polygonPerimeterTest import testPolygonPerimeter

TESTERROR = 1100011

# Функция для тестов
def tests():
    try:
        if testPolygonPerimeter() and testProcessNumbers() and testIsSameColor() and testHarmonicSumm():
            print("Все тесты пройдены успешно!")
        else:
            assert False
    except AssertionError as e:
        print(f"Ошибка: тестирование модулей не прошло успешно!\n{e}")
        exit(TESTERROR)

# Главная функция
def main(args):
    # Если аргументы не переданы, запрашиваем данные у пользователя
    if not args.task:
        while True:
            try:
                task_number = int(input("Введите номер задачи (1-4): "))
                if 1 <= task_number <= 4:
                    break
                else:
                    print("Ошибка: введите число от 1 до 4.")
            except ValueError:
                print("Ошибка: введите целое число.")
    else:
        task_number = args.task

    # Выполнение выбранной задачи
    if task_number == 1:
        # Задача 1
        # 8. Определить периметр правильного n-угольника, описанного около окружности радиуса r.
        if args.n and args.r:
            n = args.n
            r = args.r
        else:
            n = int(input("Введите количество сторон многоугольника (n): "))
            r = float(input("Введите радиус окружности (r): "))
        perimeter = findPerimetrOfRegularPolygon(n, r)
        print(f"Периметр правильного {n}-угольника, описанного около окружности радиуса {r}, равен {perimeter:.2f}")

    elif task_number == 2:
        # Задача 2
        # 37. Даны действительные числа a, b, c. Удвоить эти числа, если a ≥ b ≥ c, и заменить их абсолютными значениями, если это не так.
        if args.numbers:
            n1, n2, n3 = args.numbers
        else:
            n1 = float(input("Введите 1 действительное число: "))
            n2 = float(input("Введите 2 действительное число: "))
            n3 = float(input("Введите 3 действительное число: "))
        n1, n2, n3 = processNumbers(n1, n2, n3)
        print(f"Получившиеся числа: {n1} {n2} {n3}")

    elif task_number == 3:
        # Задача 3
        ''' 
        76a. Поле шахматной доски определяется парой натуральных чисел, каждое 
        из которых не превосходит восьми: первое число — номер вертикали 
        (при счете слева направо), второе — номер горизонтали (при счете снизу вверх). 
        Даны натуральные числа k, l, m, n, каждое из которых не превосходит восьми. Требуется:
        а) Выяснить, являются ли поля (k, l) и (m, n) полями одного цвета.
        '''
        if args.coords:
            k, l, m, n = args.coords
        else:
            try:
                k = int(input("Введите натуральное число k от 1 до 8: "))
                l = int(input("Введите натуральное число l от 1 до 8: "))
                m = int(input("Введите натуральное число m от 1 до 8: "))
                n = int(input("Введите натуральное число n от 1 до 8: "))
            except ValueError:
                raise ValueError("Все входные данные должны быть целыми числами.")
        if isSameColor(k, l, m, n):
            print(f"Поля ({k},{l}) и ({m},{n}) являются полями одного цвета")
        else:
            print(f"Поля ({k},{l}) и ({m},{n}) НЕ являются полями одного цвета")

    elif task_number == 4:
        # Задача 4
        # 115a. Дано натуральное число n. Вычислить: https://ivtipm.github.io/Programming/Glava04/index04.htm#z115#a
        if args.n:
            n = args.n
        else:
            try:
                n = int(input("Введите натуральное число n: "))
            except ValueError:
                raise ValueError("Число должно быть целым")
        result = calculateHarmonicSum(n)
        print(f"Результат вычисления гармонического ряда по числу {n} = {result:.3}")

if __name__ == "__main__":
    # Парсинг аргументов командной строки
    parser = argparse.ArgumentParser(description="Выполнение различных задач.")
    parser.add_argument("--task", type=int, choices=range(1, 5), help="Номер задачи (1-4)")
    parser.add_argument("--n", type=int, help="Количество сторон многоугольника или число для гармонического ряда")
    parser.add_argument("--r", type=float, help="Радиус окружности")
    parser.add_argument("--numbers", nargs=3, type=float, help="Три действительных числа для задачи 2")
    parser.add_argument("--coords", nargs=4, type=int, help="Четыре координаты для задачи 3 (k, l, m, n)")

    args = parser.parse_args()

    # Запуск тестов
    tests()

    # Запуск основной программы
    main(args)
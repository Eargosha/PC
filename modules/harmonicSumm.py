__author__ = "Eargosha"
"""
Вычисляет сумму гармонического ряда до n-го члена.
param:
    n (int): Натуральное число, до которого вычисляется сумма.
return:
    float: Сумма гармонического ряда.
"""
def calculateHarmonicSum(n):
    if n <= 0:
        raise ValueError("n должно быть больше 0.")

    total_sum = 0.0
    for k in range(1, n + 1):
        total_sum += 1 / k
    return total_sum
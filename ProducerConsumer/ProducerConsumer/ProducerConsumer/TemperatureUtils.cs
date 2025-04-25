// Класс для работы с температурой
public static class TemperatureUtils
{
    // Проверка на аномальную температуру
    public static bool IsTemperatureAnomaly(double temperature, double threshold = 80.0)
    {
        return temperature > threshold;
    }
}
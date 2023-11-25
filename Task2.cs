using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// Интерфейс для функции сложения чисел
public interface ICalculator
{
    double Add(double a, double b);
}

// Интерфейс для логгера
public interface ILogger
{
    void LogEvent(string message);
    void LogError(string message);
}

// Реализация интерфейса логгера
public class ConsoleLogger : ILogger
{
    public void LogEvent(string message)
    {
        using (new ConsoleColorScope(ConsoleColor.Blue))
        {
            Console.WriteLine(message);
        }
    }

    public void LogError(string message)
    {
        using (new ConsoleColorScope(ConsoleColor.Red))
        {
            Console.WriteLine(message);
        }
    }
}

// Реализация интерфейса калькулятора
public class SimpleCalculator : ICalculator
{
    private readonly ILogger logger;

    // Внедрение зависимости через конструктор
    public SimpleCalculator(ILogger logger)
    {
        this.logger = logger;
    }

    public double Add(double a, double b)
    {
        // Логгирование события
        logger.LogEvent($"Выполняется сложение: {a} + {b}");

        double result = a + b;

        // Логгирование результата
        logger.LogEvent($"Результат сложения: {result}");

        return result;
    }
}

class ConsoleColorScope : IDisposable
{
    private readonly ConsoleColor originalColor;

    public ConsoleColorScope(ConsoleColor color)
    {
        originalColor = Console.ForegroundColor;
        Console.ForegroundColor = color;
    }

    public void Dispose()
    {
        Console.ForegroundColor = originalColor;
    }
}

class Task2
{
    static void Main()
    {
        // Создаем контейнер для внедрения зависимостей
        var serviceProvider = new ServiceCollection()
            // Регистрируем калькулятор
            .AddTransient<ICalculator, SimpleCalculator>()
            // Регистрируем логгер
            .AddTransient<ILogger, ConsoleLogger>()
            .BuildServiceProvider();

        // Получаем экземпляры из контейнера
        var calculator = serviceProvider.GetService<ICalculator>();
        var logger = serviceProvider.GetService<ILogger>();

        try
        {
            // Вводим первое число
            Console.Write("Введите первое число: ");
            double num1 = GetValidInput();

            // Вводим второе число
            Console.Write("Введите второе число: ");
            double num2 = GetValidInput();

            // Вычисляем сумму
            double result = calculator.Add(num1, num2);
            Console.WriteLine($"Сумма чисел {num1} и {num2} равна {result}");
        }
        catch (FormatException ex)
        {
            // Логгирование ошибки ввода
            logger.LogError($"Ошибка ввода: {ex.Message}");
        }
        catch (Exception ex)
        {
            // Логгирование других ошибок
            logger.LogError($"Ошибка: {ex.Message}");
        }
        finally
        {
            Console.ReadLine();
        }
    }

    // Метод для получения корректного числового ввода от пользователя
    static double GetValidInput()
    {
        string input = Console.ReadLine();
        if (!double.TryParse(input, out double result))
        {
            throw new FormatException("Некорректный ввод. Введите число.");
        }

        return result;
    }
}
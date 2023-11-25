using System;

// Интерфейс для функции сложения чисел
public interface ICalculator
{
    double Add(double a, double b);
}

// Реализация интерфейса
public class SimpleCalculator : ICalculator
{
    public double Add(double a, double b)
    {
        return a + b;
    }
}

class Program
{
    static void Main()
    {
        // Создаем экземпляр калькулятора
        ICalculator calculator = new SimpleCalculator();

        try
        {
            // Вводим первое число
            Console.Write("Введите первое число: ");
            double num1 = GetValidInput();

            // Вводим второе число
            Console.Write("Введите второе число: ");
            double num2 = GetValidInput();

            // Вычисляем сумму и выводим результат
            double result = calculator.Add(num1, num2);
            Console.WriteLine($"Сумма чисел {num1} и {num2} равна {result}");
        }
        catch (FormatException)
        {
            Console.WriteLine("Ошибка: Введено некорректное число.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
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

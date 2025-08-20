using System;
using System.Collections.Generic;

namespace CalculatorApp
{
    public static class Calculator
    {
        public static double Add(double x, double y)
        {
            return x + y;
        }

        public static double Subtract(double x, double y)
        {
            return x - y;
        }

        public static double Multiply(double x, double y)
        {
            return x * y;
        }

        public static double Divide(double x, double y)
        {
            if (y == 0) throw new DivideByZeroException("Cannot divide by zero.");
            return x / y;
        }

        public static string Operation(string prompt)
        {
            List<string> operators = new List<string> { "+", "-", "*", "/" };

            while (true)
            {
                Console.Write(prompt);
                string term = Console.ReadLine() ?? string.Empty;
                if (!operators.Contains(term) && !term.Equals("exit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please enter in a valid operator (+, -, *, /)");
                }
                else
                {
                    return term;
                }
            }
        }

        public static double Calculate(double x, double y, string operation)
        {
            switch (operation)
            {
                case "+":
                    return Calculator.Add(x, y);
                case "-":
                    return Calculator.Subtract(x, y);
                case "*":
                    return Calculator.Multiply(x, y);
                case "/":
                    return Calculator.Divide(x, y);
                default:
                    return 0.0;
            }
        }
    }

    class Program
    {
        static void Main()
        {
            Console.WriteLine("------------Calculator------------\nType 'exit' to close program.");
            while (true)
            {
                double result = 0;
                string inputX = GetUserInput("Please enter the first number: ");
                if (inputX.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                double x = ParseToDouble(inputX);
                string operation = Calculator.Operation("Please type in one of the following math operators (+, -, *, /): ");
                if (operation.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                string inputY = GetUserInput("Please enter the second number: ");
                if (inputY.Equals("exit", StringComparison.OrdinalIgnoreCase)) break;
                double y = ParseToDouble(inputY);
                try
                {
                    result = Calculator.Calculate(x, y, operation);
                    Console.WriteLine($"Result: {result}");
                }
                catch (DivideByZeroException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static string GetUserInput(string prompt)
        {
            Console.Write(prompt);
            string term = Console.ReadLine() ?? string.Empty;
            return term;
        }


        public static double ParseToDouble(string term)
        {
            while (true)
            {
                if (double.TryParse((term ?? string.Empty).Trim(), out double value))
                    return value;

                Console.Write("Please enter a valid number (integers or decimals): ");
                term = Console.ReadLine();
            }
        }
    }
}

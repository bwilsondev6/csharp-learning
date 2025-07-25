using System;

class Program
{
    static void Main()
    {
        Random rand = new Random();
        int age = rand.Next(18, 41);

        bool guess = false;
        int maxAttempts = 5;
        int attempts = 0;

        Console.WriteLine("Guess my age, it's between 18 and 41: ");

        while (!guess && maxAttempts > 0)
        {
            string userGuess = Console.ReadLine();

            if (!int.TryParse(userGuess, out int userGuessValid))
            {
                Console.WriteLine("Invalid format. Please enter numbers.");
            }
            else
            {
                attempts++;
                maxAttempts--;

                if (userGuessValid == age)
                {
                    Console.WriteLine($"You got it! I am {age} years old. You got it in {attempts} attempts.");
                    guess = true;
                }
                else
                {
                    if (maxAttempts > 0)
                    {
                        Console.WriteLine($"Try again. You have {maxAttempts} attempts left.");
                        Console.WriteLine(userGuessValid > age ? "*HINT* it's lower" : "*HINT* it's higher");
                    }
                    else
                    {
                        Console.WriteLine($"Game over. I am {age} years old.");
                    }
                }
            }
        }
    }
}

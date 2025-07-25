using System;

class Program
{
    static void Main()
    {
        List<string> items = new List<string>();

        while (true)
        {
            Console.WriteLine("------------------");
            Console.WriteLine("1. Add item\n2.View items\n3.Remove item\n4.Exit");
            Console.WriteLine("------------------");
            if (!int.TryParse(GetUserInput("(Choose an option)"), out int userinput))
            {
                Console.WriteLine("Invalid format. Please enter numbers.");
            }
            else
            {
                switch (userinput)
                {
                    case 1:
                        AddItems(items);
                        break;
                    case 2:
                        ViewItems(items);
                        break;
                    case 3:
                        RemoveItems(items);
                        break;
                    case 4:
                        Console.WriteLine("Goodbye!");
                        return;
                    default:
                    Console.WriteLine("Invalid option. Please choose 1-4.");
                    break;

                }
            }
        }
    }

    static string GetUserInput(string prompt)
    {
        Console.WriteLine(prompt);
        return Console.ReadLine();
    }

    static void AddItems(List<string> items)
    {
        string item = GetUserInput("What would you like to add to the list?");
        items.Add(item);
        Console.WriteLine($"{item} added to list");
    }

    static void ViewItems(List<string> items)
    {
        if (items.Count == 0)
        {
            Console.WriteLine("There are no items in the list.");
        }
        else
        {
            Console.WriteLine("------------------");
            Console.WriteLine("Items in list: ");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i]}");
            }
        }
    }

    static void RemoveItems(List<string> items)
    {
        Console.WriteLine("Enter the number of the item to remove:");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index > 0 && index <= items.Count)
        {
            items.RemoveAt(index - 1);
            Console.WriteLine("Item removed!");
        }
        else
        {
            Console.WriteLine("Invalid selection.");
        }
    }

}

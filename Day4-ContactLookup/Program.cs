using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.IO;


namespace ContactLookup
{

    public class Contact
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }

    class Program
    {
        static readonly string DataFolder =
            Path.Combine(AppContext.BaseDirectory, "Data");
        static readonly string ContactsFile =
            Path.Combine(DataFolder, "contacts.json");
        static void Main()
        {
            if (!Directory.Exists(DataFolder))
            {
                Directory.CreateDirectory(DataFolder);
            }


            List<Contact> contactsList = new List<Contact> {
                new Contact { Name = "Brad",  Phone = "123-456-7890" },
                new Contact { Name = "Joe",   Phone = "123-456-9348" },
                new Contact { Name = "Julia", Phone = "123-456-1223" },
                new Contact { Name = "Jesus", Phone = "123-456-9937" },
                new Contact { Name = "Bryan", Phone = "123-456-3967" }
            };

            if (!File.Exists(ContactsFile))
            {
                var json = JsonSerializer.Serialize(contactsList, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(ContactsFile, json);
            }

            var contacts = LoadContacts(ContactsFile);

            while (true)
            {
                Console.WriteLine("------------------");
                Console.WriteLine("1. Lookup Contact");
                Console.WriteLine("2. Add Contact");
                Console.WriteLine("3. Update Contact");
                Console.WriteLine("4. Delete Contact");
                Console.WriteLine("5. Exit");
                Console.WriteLine("------------------");

                if (!int.TryParse(GetUserInput("(Choose an option)"), out int choice))
                {
                    Console.WriteLine("Invalid format. Please enter numbers.");
                    continue;
                }

                switch (choice)
                {
                    case 1:
                        {
                            var term = GetUserInput("Enter name to lookup:");
                            var results = LookupContact(term, contacts);
                            if (!results.Any())
                            {
                                Console.WriteLine("No matching contacts found.");
                            }
                            else
                            {
                                Console.WriteLine($"Found {results.Count()} contact(s):");
                                PrintContacts(results);
                            }
                            break;
                        }

                    case 2:
                        {
                            var name = GetUserInput("Enter contact name:");
                            var number = GetUserInput("Enter contact number:");

                            AddContact(contacts, name, number);
                            break;
                        }

                    case 3:
                        {
                            var number = GetUserInput("Enter the CURRENT number of the contact to update:");
                            UpdateContact(contacts, number);
                            break;
                        }

                    case 4:
                        {
                            var number = GetUserInput("Enter the number of the contact to remove:");
                            RemoveContact(contacts, number);
                            break;
                        }

                    case 5:
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option. Please choose 1-5.");
                        break;
                }
            }
        }

        static string GetUserInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine() ?? string.Empty;
        }

        /* ===================== Lookup / Print ===================== */

        static IEnumerable<Contact> LookupContact(string input, List<Contact> contacts)
        {
            var query = (input ?? "").Trim();
            if (query.Length == 0) return Enumerable.Empty<Contact>();

            return contacts
                .Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .OrderBy(c => c.Name, StringComparer.OrdinalIgnoreCase);
        }

        static void PrintContacts(IEnumerable<Contact> contacts)
        {
            foreach (var c in contacts)
            {
                Console.WriteLine("----------------------");
                Console.WriteLine($"Name : {c.Name}");
                Console.WriteLine($"Phone: {c.Phone}");
            }
        }

        /* ===================== Add ===================== */

        static void AddContact(List<Contact> contacts, string nameInput, string numberInput)
        {
            string name = ValidateNameTerm(nameInput);
            if (name == "Error") return;

            string formattedPhone = ValidatePhoneTerm(numberInput);
            if (formattedPhone == "Error") return;

            // Compare duplicates by digits, not formatting
            var newDigits = Digits(formattedPhone);
            if (contacts.Any(c => Digits(c.Phone) == newDigits))
            {
                Console.WriteLine("That number already exists.");
                return;
            }

            if (contacts.Any(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase)))
            {
                Console.WriteLine("That name already exists. Choose Update instead.");
                return;
            }

            contacts.Add(new Contact { Name = name, Phone = formattedPhone });
            SaveContacts(contacts, ContactsFile);
            Console.WriteLine($"{name} added to contacts.");
        }

        /* ===================== Update ===================== */

        static void UpdateContact(List<Contact> contacts, string contactNumber)
        {
            var keyDigits = Digits(contactNumber);
            if (keyDigits.Length != 10)
            {
                Console.WriteLine("Enter an existing number with exactly 10 digits.");
                return;
            }

            var contactToUpdate = contacts.Find(c => Digits(c.Phone) == keyDigits);
            if (contactToUpdate == null)
            {
                Console.WriteLine("Contact not found.");
                return;
            }

            var newNumberInput = GetUserInput($"Enter a NEW number for {contactToUpdate.Name}:");
            var newFormatted = ValidatePhoneTerm(newNumberInput);
            if (newFormatted == "Error") return;

            var newDigits = Digits(newFormatted);
            if (contacts.Any(c => Digits(c.Phone) == newDigits && !ReferenceEquals(c, contactToUpdate)))
            {
                Console.WriteLine("That number already exists.");
                return;
            }

            contactToUpdate.Phone = newFormatted;
            SaveContacts(contacts, ContactsFile);
            Console.WriteLine($"{contactToUpdate.Name}'s contact updated.");
        }

        /* ===================== Remove ===================== */

        static void RemoveContact(List<Contact> contacts, string number)
        {
            var keyDigits = Digits(number);
            if (keyDigits.Length != 10)
            {
                Console.WriteLine("Please enter a valid 10-digit number.");
                return;
            }

            var removed = contacts.RemoveAll(c => Digits(c.Phone) == keyDigits);
            if (removed > 0)
            {
                SaveContacts(contacts, ContactsFile);
                Console.WriteLine("Contact removed.");
            }
            else
            {
                Console.WriteLine("Contact not found.");
            }
        }

        /* ===================== Validation Helpers ===================== */

        // Allow letters, spaces, apostrophes, hyphens. Trim + collapse spaces.
        static string ValidateNameTerm(string userInput)
        {
            var raw = (userInput ?? "").Trim();
            if (raw.Length < 2)
            {
                Console.WriteLine("Please enter at least 2 characters for the name.");
                return "Error";
            }

            if (!raw.All(IsValidNameChar))
            {
                Console.WriteLine("Use letters, spaces, apostrophes, or hyphens only.");
                return "Error";
            }

            // Collapse multiple spaces
            var name = string.Join(" ", raw.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
            return name;
        }

        static bool IsValidNameChar(char ch)
            => char.IsLetter(ch) || ch == ' ' || ch == '\'' || ch == '-';

        // Require exactly 10 digits; format as ###-###-####. Reject letters.
        static string ValidatePhoneTerm(string userInput)
        {
            var input = (userInput ?? "").Trim();
            if (input.Length == 0)
            {
                Console.WriteLine("Please enter a phone number.");
                return "Error";
            }

            if (input.Any(char.IsLetter))
            {
                Console.WriteLine("Please enter digits only (you can include spaces/dashes/parentheses).");
                return "Error";
            }

            string digits = Digits(input);
            if (digits.Length != 10)
            {
                Console.WriteLine("Please enter exactly 10 digits.");
                return "Error";
            }

            var d1 = digits.Substring(0, 3);
            var d2 = digits.Substring(3, 3);
            var d3 = digits.Substring(6, 4);
            return $"{d1}-{d2}-{d3}";
        }

        // Keep digits only
        static string Digits(string number)
            => new string((number ?? "").Where(char.IsDigit).ToArray());

        static void SaveContacts(List<Contact> contacts, string filePath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true }; // pretty-print
            string json = JsonSerializer.Serialize(contacts, options);
            File.WriteAllText(filePath, json);
        }

        static List<Contact> LoadContacts(string filePath)
        {
            try
            {
                string json = File.ReadAllText(filePath);
                return JsonSerializer.Deserialize<List<Contact>>(json) ?? new List<Contact>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading contacts: {ex.Message}");
                return new List<Contact>(); // fallback so program keeps running
            }
        }
    }


}
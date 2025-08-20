# Calculator App

A simple console-based calculator built in **C#**. This project was created as a learning exercise to practice working with input validation, loops, and basic arithmetic operations.  

---

## Features  
- Supports the four basic operators: **+**, **-**, ***, /**  
- Handles both **integers** and **decimals**  
- Validates user input to ensure only valid numbers are processed  
- Loops until the user chooses to **exit**  
- Graceful error handling for invalid inputs  

---

## How It Works  
1. The program prompts the user for two numbers.  
2. The user chooses an operator (`+`, `-`, `*`, `/`).  
3. The result is displayed in the console.  
4. The app repeats until the user types **exit**.  

---

## Code Highlights  
- **Input Parsing:**  
  Uses `double.TryParse` inside a loop to ensure numbers are valid:  
  ```csharp
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
  ```

- **Loop Control:**  
  A `while` loop runs continuously until the user enters `"exit"` as the prompt.  

---

## 🚀 Getting Started  

### Prerequisites  
- [.NET SDK](https://dotnet.microsoft.com/en-us/download) (6.0 or later recommended)  

### Running the Program  
1. Clone this repo:  
   ```bash
   git clone https://github.com/yourusername/calculator-app.git
   cd calculator-app
   ```
2. Build and run:  
   ```bash
   dotnet run
   ```

---

## Lessons Learned  
- Difference between recursion and loops for handling repeated input  
- How variables persist and update inside a loop  
- Using input validation to make programs user-friendly  

---

##  Future Improvements  
- Add support for more operators (power, modulus, etc.)  
- Implement error handling for division by zero  
- Build a GUI version with **WinForms** or **WPF** in the future  

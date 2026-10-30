// MyBudget
// Declare variables;
// using System.Net.Http.Headers;
using System.Globalization;

// string expenses;
List<decimal> expenses = new();
// decimal amount;
decimal amount = 0m;

// string category;
string category = "";

// DateOnly date;
DateOnly date = default;

// DateTime dateTime;
// string product;
// string description;
string description = "";

// string budget;
// null means that the user has not set a budget.
decimal? budget = null;

// Running totals required by Feature 1.
decimal overallTotal = 0m;
int expenseCount = 0;
decimal highestSingleExpense = 0m;
const decimal maximumExpense = 10000m;

// Arrays for categories.
string[] categories =
{
    "Food",
    "Transport",
    "Utilities",
    "Entertainment",
    // "Healthcare",
    // "Education",
    // "Shopping",
    // "Housing",
    // "Insurance",
    // "Travel",
    "Other" 
};

// Store a running total for each allowed category.
Dictionary<string, decimal> categoryTotals =
    new(StringComparer.OrdinalIgnoreCase);

foreach (string allowedCategory in categories)
{
    categoryTotals[allowedCategory] = 0m;
}

Console.WriteLine("""
    =========================
             MyBudget
    =========================
    """);

// Range and index-from-end demonstrations.
var firstTwoCategories = categories[..2];

Console.WriteLine(
    $"First two categories: {string.Join(", ", firstTwoCategories)}");

Console.WriteLine($"Last category: {categories[^1]}");

// features must be inside a menu loop.
bool running = true;

while (running)
{
    Console.WriteLine("""
        
        1 — Add an expense
        2 — View summary
        3 — Set monthly budget
        4 — Exit
        """);

    Console.Write("Choose an option: ");
    string? menuInput = Console.ReadLine();

    // End cleanly if the input stream closes.
    if (menuInput is null)
    {
        break;
    }

    //  Parse the menu choice as an int.
    if (!int.TryParse(menuInput, out int menuChoice))
    {
        Console.WriteLine("Enter a number from 1 to 4.");
        continue;
    }

    // Required switch statement.
    switch (menuChoice)
    {
        // Part 01 — Feature 1: Add an expense.
        case 1:
        {
            bool expenseSaved = false;

            // Meaningful try/catch/finally for the add-expense flow.
            try
            {
                // Console.WriteLine("Add an expense: ");
                // Product = Console.ReadLine();
                // The assignment asks for a description instead.
                Console.WriteLine("Add an expense:");

                Console.WriteLine( "List of categories available:");

                // Console.WriteLine(categories[0]);
                // for (i = 0; i < categories.Length; i++)
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {categories[i]}");
                }

                // Console.WriteLine(
                // "Choose how many categories would you like: " + categories);
                // categories = Console.ReadLine() ?? "";
                // A string cannot be assigned to the string[] categories.
                // The category input is handled below.

                // mounts = Parse.ReadLine() ?? "";
                // The amount input is handled below using decimal.TryParse.

                // if (mount <= 0)
                // {
                //     Console.WriteLine("Buy again");
                // }
                // else
                //     Console.WriteLine("Continue!");
                // The amount loop below handles this.

                do
                {
                    // console.WriteLine("Enter description");
                    // Description = console.ReadLine() ?? "";
                    Console.Write("Enter description: ");
                    description = ReadRequiredInput().Trim();
                }
                // while (string.isNullOderSpace(description));
                while (string.IsNullOrWhiteSpace(description));
                // Amount.
                // console.WriteLine("Enter mount: ");
                // while(!decimal.TryParse(conssole.ReadLine(), out mount)
                //       || mount <= 0 || mount > 10000);
                // {
                //     console.WriteLine("Invalid mount and try again!");
                // }
                while (true)
                {
                    Console.Write(
                        $"Enter amount (greater than 0, maximum {maximumExpense}): ");

                    if (!decimal.TryParse(
                            ReadRequiredInput(),
                            out amount))
                    {
                        Console.WriteLine(
                            "Invalid amount. Enter a number.");
                        continue;
                    }

                    try
                    {
                        // the assignment's validation method.
                        amount = BudgetRules.ValidateAmount(amount);

                        // Preserve the positive/maximum checks from your code.
                        if (amount <= 0m || amount > maximumExpense)
                        {
                            throw new InvalidExpenseException(
                                $"Amount must be greater than zero " +
                                $"and no more than {maximumExpense}.");
                        }

                        break;
                    }
                    catch (InvalidExpenseException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Please try again.");
                    }
                }

                // console.WriteLine($"Valid mount {mount}");
                Console.WriteLine(
                    $"Valid amount: {BudgetRules.FormatCurrency(amount)}");

                // Category.
                // console.WriteLine("Enter category: ");
                // category = console.ReadLine() ?? "";
                //
                // while (
                //     !category.Equals("Food", stringComparison.OrdinalIgnoreCase) &&
                //     !category.Equals("Transport", stringComparison.OrdinalIgnoreCase) &&
                //     !category.Equals("Utilities", stringComparison.OrdinalIgnoreCase) &&
                //     !category.Equals("Entertainment", stringComparison.OrdinalIgnoreCase) &&
                //     !category.Equals("Other", stringComparison.OrdinalIgnoreCase)
                // )
                // {
                //     console.WriteLine("Invalid category. Try again please!");
                // }

                while (true)
                {
                    Console.Write("Enter category: ");

                    string? normalizedCategory =
                        BudgetRules.NormalizeCategory(
                            ReadRequiredInput().Trim());

                    if (normalizedCategory is not null)
                    {
                        category = normalizedCategory;
                        break;
                    }

                    Console.WriteLine(
                        "Invalid category. Enter Food, Transport, " +
                        "Utilities, Entertainment, or Other.");
                }

                Console.WriteLine($"Valid category: {category}");

                // Validate date.
                // console.WriteLine("Enter date (yyyy-mm-dd): ");
                // while(!dateTime.TryParseExact(
                //     console.ReadLine(), "yyyy-mm-dd",
                //     CultureInfo.InvariantCulture,
                //     dateTimeStyles.none, out date));
                // {
                //     console.WriteLine("Invalid date. Try gain (yyyy-mm-dd)!");
                // }

                while (true)
                {
                    Console.Write(
                        "Enter date (yyyy-mm-dd), or leave empty for today: ");

                    string dateInput = ReadRequiredInput().Trim();
                    DateOnly today = DateOnly.FromDateTime(DateTime.Today);

                    if (dateInput.Length == 0)
                    {
                        date = today;
                        break;
                    }

                    if (!DateOnly.TryParseExact(
                            dateInput,
                            "yyyy-MM-dd",
                            CultureInfo.InvariantCulture,
                            DateTimeStyles.None,
                            out date))
                    {
                        Console.WriteLine(
                            "Invalid date. Try again using yyyy-mm-dd.");
                        continue;
                    }

                    if (date > today)
                    {
                        Console.WriteLine(
                            "A future date is not allowed. Try again.");
                        continue;
                    }

                    break;
                }

                // Optional note with nullable reference types and ?.
                Console.Write("Optional note: ");
                string? note = Console.ReadLine()?.Trim();

                if (String.IsNullOrWhiteSpace(note))
                {
                    note = null;
                }

                // ?? handles an absent note without printing an empty label.
                string noteOutput = note ?? "";

                // Required amount classification.
                string sizeBand = BudgetRules.ClassifyAmount(amount);

                // Calculate totals before updating the stored state.
                // Decimal arithmetic can overflow, so the outer catch handles it.
                decimal newOverallTotal = overallTotal + amount;
                decimal newCategoryTotal = categoryTotals[category] + amount;

                expenses.Add(amount);
                overallTotal = newOverallTotal;
                categoryTotals[category] = newCategoryTotal;
                expenseCount++;
                highestSingleExpense =
                    Math.Max(highestSingleExpense, amount);

                expenseSaved = true;

                // Display information.
                // console.WriteLine($"Description {description}");
                Console.WriteLine($"Description: {description}");

                // console.WriteLine($"The expenses are: {expenses}");
                Console.WriteLine($"Number of expenses: {expenseCount}");
                // console.WriteLine($"The mounts are: {mount}");
                Console.WriteLine(
                    $"Amount: {BudgetRules.FormatCurrency(amount)}");

                // console.WriteLine($"The category are: {category}");
                Console.WriteLine($"Category: {category}");

                // console.WriteLine($"The date is: {date}");
                Console.WriteLine($"Date: {date:yyyy-MM-dd}");

                // console.WriteLine($"The time are: {dateTime}");
                // console.WriteLine($"The products are: {product}");

                Console.WriteLine($"Size band: {sizeBand}");

                if (noteOutput.Length > 0)
                {
                    Console.WriteLine($"Note: {noteOutput}");
                }
                
            }
            
            catch (InvalidExpenseException ex)
            {
                Console.WriteLine($"Expense rejected: {ex.Message}");
            }
            catch (OverflowException)
            {
                Console.WriteLine(
                    "The expense would make the totals too large.");
            }
            catch (EndOfStreamException)
            {
                Console.WriteLine("Input ended.");
                running = false;
            }
            finally
            {
                // Reports the outcome on both success and failure.
                Console.WriteLine(
                    expenseSaved
                        ? "Expense saved."
                        : "No expense was saved.");
            }

            break;
        }

        // Part 02 — Feature 2: View summary.
        case 2:
        {
            // 90.00m;
            if (expenses.Count > 0)
            {
                // int numberOfExpenses = expenses.Count;
                int numberOfExpenses = expenseCount;

                // decimal total = expenses.Sum();
                decimal total = overallTotal;

                // decimal average = expenses.Average();
                decimal average = total / numberOfExpenses;

                // decimal highest = expenses.Max();
                decimal highest = highestSingleExpense;

                // console.WriteLine($"The number of expenses are: {numberOfExpenses}");
                Console.WriteLine(
                    $"Number of expenses: {numberOfExpenses}");

                // console.WriteLine($"The total are: {total:F2}");
               // Console.WriteLine(
                  //  $"Total: {BudgetRules.FormatCurrency(total)}");
                   Console.WriteLine($"Total: {total:C2}");
                // console.WriteLine($"The average are: {average:F2}");
                //Console.WriteLine(
                  //  $"Average: {BudgetRules.FormatCurrency(average)}");
                    Console.WriteLine($"Average: {average:C2}");
                // console.WriteLine($"The highest: {highest:F2}");
               // Console.WriteLine(
                  //  $"Highest: {BudgetRules.FormatCurrency(highest)}");
                  Console.WriteLine($"Highest: {highest:C2}");
            }
            else
            {
                // console.WriteLine("No expenses recorded!");
                Console.WriteLine("No expenses recorded yet.");
            }

            // Show the total spent per category.
            // foreach (string category in categories);
            foreach (string categoryName in categories)
            {
                // decimal total = expenses.Where(
                //     expenses => expensesCategory.Equals(
                //         category, stringComparison.OrdinalIgnoreCase))
                //     .Sum(expenses.amount);
                decimal categoryTotal = categoryTotals[categoryName];

                // console.WriteLine($"{category}: ${total:F2}");
               // Console.WriteLine(
                   // $"{categoryName}: " +
                   // BudgetRules.FormatCurrency(categoryTotal));
                    Console.WriteLine($"{categoryName}: {categoryTotal:C2}");
            }

            // Check whether a budget is set.
            if (budget.HasValue)
            {
                // decimal totalExpenses =
                //     expenses.Sum(expenses => expenses.amount);
                decimal totalExpenses = overallTotal;

                decimal remaining = budget.Value - totalExpenses;

                // console.WriteLine($"Remaining: ${remaining:F2}");
                //Console.WriteLine(
                  //  $"Remaining: {BudgetRules.FormatCurrency(remaining)}");
                    Console.WriteLine($"Remaining: {remaining:C2}");
                // if (remaining > 0)
                // {
                //     console.WriteLine("Status: within budget!");
                // }
                // else if (remaining == 0)
                //     console.WriteLine("Status: budget reached!");
                // else
                //     console.WriteLine("Status: over budget!");
                // inside the scope where it was declared.
               // Console.WriteLine(
                 //   $"Status: {BudgetRules.BudgetStatus(
                  //      remaining, budget.Value)}");
                   if (remaining < 0m)
               {
                    Console.WriteLine("Status: Over budget");
                }
                    else if (remaining < budget.Value * 0.10m)
               {
                   Console.WriteLine("Status: Almost out");
               }
              else
              {
                  Console.WriteLine("Status: On track");
               }
            }

            // if (expenses.Count == 0);
            // console.WriteLine("No expense exit yet!");
            // else
            // {
            //     decimal total = expenses.Sum(expenses => expenses.amount);
            //     decimal average = total / expenses.Count;
            // }
            // console.WriteLine($"Average expenses are: ${average}");
            break;
        }

        // Feature 3 — Set monthly budget.
        case 3:
        {
            // console.WriteLine("Enter your budget: ");
            // while(!decimal.TryParse(
            //     console.ReadLine(), out budget || budget <= 0))
            // {
            //     console.WriteLine("Invalid budget. Try again!");
            // }

            while (true)
            {
                Console.Write("Enter your budget: ");
                string? budgetInput = Console.ReadLine();

                if (budgetInput is null)
                {
                    running = false;
                    break;
                }

                if (!decimal.TryParse(
                        budgetInput, out decimal enteredBudget)
                    || enteredBudget <= 0m)
                {
                    Console.WriteLine(
                        "Invalid budget. Enter a positive number.");
                    continue;
                }

                budget = enteredBudget;

                // console.WriteLine("Your budget is: " + budget);
               // Console.WriteLine(
                //    $"Your budget is: " +
                  //  BudgetRules.FormatCurrency(budget.Value));
                    Console.WriteLine($"Your budget is: {budget.Value:C2}");
                decimal remaining = budget.Value - overallTotal;

                 //string BudgetStatus;
                //
                // Declare a variable to store the status.
                 string budgetStatus;

                 if (remaining < 0)
                {
                 budgetStatus = "Over budget";

                Console.WriteLine("Over budget: " + budgetStatus);
                }
               else if (remaining < budget.Value * 0.10m)
              {
                 budgetStatus = "Almost out";
                 Console.WriteLine("Almost out: " + budgetStatus);
             }
          else
            {
                budgetStatus = "On track";
                Console.WriteLine("on track: " + budgetStatus);
             }

               Console.WriteLine($"Budget status: {budgetStatus}");

                //------------------------
             // string budgetStatus =
                 //   BudgetRules.BudgetStatus(remaining, budget.Value);
                // Console.WriteLine($"Budget status: {budgetStatus}");

                break;
            }

            break;
        }

        // Feature 4 — Exit.
        case 4:
        {
            // while (true)
            // {
            //     console.WriteLine("Do you want to exit? (Yes/No)");
            //     string choice = Console.ReadLine() ?? "";
            // }
            // if(choice.Equals("Yes", StringComparison.OrdinalIgnoreCase));
            // {
            //     break;
            // }
            // console.WriteLine("Continue the program, let's go!");
            running = false;
            break;
        }

        default:
        {
            Console.WriteLine("Choose an option from 1 to 4.");
            break;
        }
    }
}

// Console.WriteLine("Thank you for using the expensess traker! Goodbye!");
Console.WriteLine("Thank you for using the expense tracker! Goodbye!");

static string ReadRequiredInput()
{
    return Console.ReadLine()
        ?? throw new EndOfStreamException("Input is no longer available.");
}

// These definitions make the existing calls work when running this file on its own.
public static class BudgetRules
{
    public static decimal ValidateAmount(decimal amount)
    {
        // Use the same realistic maximum already specified in the program.
        if (amount <= 0m || amount > 10000m)
        {
            throw new InvalidExpenseException(
                "Amount must be greater than zero and no more than 10000.");
        }

        return amount;
    }

    public static string FormatCurrency(decimal amount)
    {
        return amount.ToString("C2", CultureInfo.GetCultureInfo("en-CA"));
    }

    // Match the nullable input signature and safely handle null.
    public static string? NormalizeCategory(string? input)
    {
        return input?.Trim().ToLowerInvariant() switch
        {
            "food" => "Food",
            "transport" => "Transport",
            "utilities" => "Utilities",
            "entertainment" => "Entertainment",
            "other" => "Other",
            _ => null
        };
    }

    public static string ClassifyAmount(decimal amount)
    {
        // Small: below 50; Medium: 50 to below 200; Large: 200 or more.
        return amount switch
        {
            < 50m => "Small",
            < 200m => "Medium",
            _ => "Large"
        };
    }

    public static string BudgetStatus(decimal remaining, decimal monthlyLimit)
    {
        if (remaining < 0m)
        {
            return "Over budget";
        }

        if (remaining < monthlyLimit * 0.10m)
        {
            return "Almost out";
        }

        return "On track";
    }
}

public class InvalidExpenseException : Exception
{
    public InvalidExpenseException(string message) : base(message)
    {
    }
}

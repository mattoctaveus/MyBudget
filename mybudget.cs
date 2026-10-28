// MyBudget
// Declare variables;

// using System.Net.Http.Headers;
// CORRECTION: HTTP headers are not needed here.
using System.Globalization;

// string expenses;
// CORRECTION: A string cannot store expense amounts for Count/Sum/Average/Max.
List<decimal> expenses = new();

// decimal amount;
decimal amount = 0m;

// string category;
string category = "";

// DateOnly date;
DateOnly date = default;

// DateTime dateTime;
// CORRECTION: The assignment needs a date only, so DateOnly is sufficient.

// string product;
// CORRECTION: A separate product is not required; use description.

// string description;
string description = "";

// string budget;
// CORRECTION: A budget is numeric and optional.
// null means that the user has not set a budget.
decimal? budget = null;

// ADDED: Running totals required by Feature 1.
decimal overallTotal = 0m;
int expenseCount = 0;
decimal highestSingleExpense = 0m;

// ADDED: Keeps the maximum from your original validation.
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
    // CORRECTION: These categories are not permitted by the assignment.

    "Other" // CORRECTION: This required category was missing.
};

// ADDED: Store a running total for each allowed category.
Dictionary<string, decimal> categoryTotals =
    new(StringComparer.OrdinalIgnoreCase);

foreach (string allowedCategory in categories)
{
    categoryTotals[allowedCategory] = 0m;
}

// ADDED: Raw string literal.
Console.WriteLine("""
    =========================
             MyBudget
    =========================
    """);

// ADDED: Range and index-from-end demonstrations.
var firstTwoCategories = categories[..2];

Console.WriteLine(
    $"First two categories: {string.Join(", ", firstTwoCategories)}");

Console.WriteLine($"Last category: {categories[^1]}");

// ADDED: All features must be inside a menu loop.
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

    // ADDED: End cleanly if the input stream closes.
    if (menuInput is null)
    {
        break;
    }

    // ADDED: Parse the menu choice as an int.
    if (!int.TryParse(menuInput, out int menuChoice))
    {
        Console.WriteLine("Enter a number from 1 to 4.");
        continue;
    }

    // ADDED: Required switch statement.
    switch (menuChoice)
    {
        // Part 01 — Feature 1: Add an expense.
        case 1:
        {
            bool expenseSaved = false;

            // ADDED: Meaningful try/catch/finally for the add-expense flow.
            try
            {
                // Console.WriteLine("Add an expense: ");
                // Product = Console.ReadLine();
                // CORRECTION: Product and product are different identifiers.
                // The assignment asks for a description instead.
                Console.WriteLine("Add an expense:");

                Console.WriteLine(
                    "List of categories available:");

                // Console.WriteLine(categories[0]);
                // CORRECTION: The loop below already prints every category.

                // for (i = 0; i < categories.Length; i++)
                // CORRECTION: Declare i.
                for (int i = 0; i < categories.Length; i++)
                {
                    Console.WriteLine($"{i + 1}: {categories[i]}");
                }

                // Console.WriteLine(
                //     "Choose how many categories would you like: " + categories);
                // categories = Console.ReadLine() ?? "";
                // CORRECTION: Choose one category, not a number of categories.
                // A string cannot be assigned to the string[] categories.
                // The category input is handled below.

                // mounts = Parse.ReadLine() ?? "";
                // CORRECTION: mounts and Parse.ReadLine do not exist.
                // The amount input is handled below using decimal.TryParse.

                // if (mount <= 0)
                // {
                //     Console.WriteLine("Buy again");
                // }
                // else
                //     Console.WriteLine("Continue!");
                // CORRECTION: Validation must allow retrying.
                // The amount loop below handles this.

                do
                {
                    // console.WriteLine("Enter description");
                    // Description = console.ReadLine() ?? "";
                    // CORRECTION: C# is case-sensitive.
                    Console.Write("Enter description: ");
                    description = ReadRequiredInput().Trim();
                }
                // while (string.isNullOderSpace(description));
                // CORRECTION: Use the actual method name.
                while (string.IsNullOrWhiteSpace(description));

                // Amount.

                // console.WriteLine("Enter mount: ");
                // while(!decimal.TryParse(conssole.ReadLine(), out mount)
                //       || mount <= 0 || mount > 10000);
                // {
                //     console.WriteLine("Invalid mount and try again!");
                // }
                // CORRECTION:
                // - Use Console and amount consistently.
                // - Do not put a semicolon after while (...).
                // - Read input again after invalid input.
                // - Catch the custom validation exception and allow retrying.

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
                        // ADDED: Call the assignment's validation method.
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
                // CORRECTION:
                // Your loop never reads a new category, so it can loop forever.
                // StringComparison also needs a capital S.
                // Use BudgetRules.NormalizeCategory to centralize the rule.

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
                // CORRECTION:
                // - Use DateOnly.TryParseExact because date is DateOnly.
                // - MM means month; mm means minutes in date/time formats.
                // - Use DateTimeStyles.None with the correct capitalization.
                // - Remove the semicolon after while (...).
                // - Handle an empty entry and reject future dates.

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

                // ADDED: Optional note with nullable reference types and ?.
                Console.Write("Optional note: ");
                string? note = Console.ReadLine()?.Trim();

                if (String.IsNullOrWhiteSpace(note))
                {
                    note = null;
                }

                // ADDED: ?? handles an absent note without printing an empty label.
                string noteOutput = note ?? "";

                // ADDED: Required amount classification.
                string sizeBand = BudgetRules.ClassifyAmount(amount);

                // ADDED: Calculate totals before updating the stored state.
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
                // CORRECTION: Display the expense count rather than the collection.
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
                // CORRECTION: Time and product are not required fields.

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
                // ADDED: Reports the outcome on both success and failure.
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
            // CORRECTION: A standalone numeric literal is not a valid statement.

            if (expenses.Count > 0)
            {
                // int numberOfExpenses = expenses.Count;
                int numberOfExpenses = expenseCount;

                // decimal total = expenses.Sum();
                // CORRECTION: Sum now works, but use the required running total.
                decimal total = overallTotal;

                // decimal average = expenses.Average();
                decimal average = total / numberOfExpenses;

                // decimal highest = expenses.Max();
                decimal highest = highestSingleExpense;

                // console.WriteLine($"The number of expenses are: {numberOfExpenses}");
                Console.WriteLine(
                    $"Number of expenses: {numberOfExpenses}");

                // console.WriteLine($"The total are: {total:F2}");
                Console.WriteLine(
                    $"Total: {BudgetRules.FormatCurrency(total)}");

                // console.WriteLine($"The average are: {average:F2}");
                Console.WriteLine(
                    $"Average: {BudgetRules.FormatCurrency(average)}");

                // console.WriteLine($"The highest: {highest:F2}");
                Console.WriteLine(
                    $"Highest: {BudgetRules.FormatCurrency(highest)}");
            }
            else
            {
                // console.WriteLine("No expenses recorded!");
                Console.WriteLine("No expenses recorded yet.");
            }

            // Show the total spent per category.

            // foreach (string category in categories);
            // CORRECTION:
            // - Remove the semicolon.
            // - Use a different variable name because category already exists.
            foreach (string categoryName in categories)
            {
                // decimal total = expenses.Where(
                //     expenses => expensesCategory.Equals(
                //         category, stringComparison.OrdinalIgnoreCase))
                //     .Sum(expenses.amount);
                // CORRECTION: expenses contains decimals, not objects with
                // Amount and Category properties. Use the category totals.
                decimal categoryTotal = categoryTotals[categoryName];

                // console.WriteLine($"{category}: ${total:F2}");
                Console.WriteLine(
                    $"{categoryName}: " +
                    BudgetRules.FormatCurrency(categoryTotal));
            }

            // Check whether a budget is set.
            if (budget.HasValue)
            {
                // decimal totalExpenses =
                //     expenses.Sum(expenses => expenses.amount);
                decimal totalExpenses = overallTotal;

                decimal remaining = budget.Value - totalExpenses;

                // console.WriteLine($"Remaining: ${remaining:F2}");
                Console.WriteLine(
                    $"Remaining: {BudgetRules.FormatCurrency(remaining)}");

                // if (remaining > 0)
                // {
                //     console.WriteLine("Status: within budget!");
                // }
                // else if (remaining == 0)
                //     console.WriteLine("Status: budget reached!");
                // else
                //     console.WriteLine("Status: over budget!");
                // CORRECTION: Use the required method and keep remaining
                // inside the scope where it was declared.
                Console.WriteLine(
                    $"Status: {BudgetRules.BudgetStatus(
                        remaining, budget.Value)}");
            }

            // if (expenses.Count == 0);
            // console.WriteLine("No expense exit yet!");
            // else
            // {
            //     decimal total = expenses.Sum(expenses => expenses.amount);
            //     decimal average = total / expenses.Count;
            // }
            // console.WriteLine($"Average expenses are: ${average}");
            // CORRECTION:
            // - The semicolon breaks the if/else.
            // - average was outside its scope.
            // - The earlier if/else already handles no expenses safely.

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
            // CORRECTION:
            // - The || condition belongs outside TryParse.
            // - Parse into a decimal, then assign it to decimal? budget.
            // - Re-prompt on every attempt.

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
                Console.WriteLine(
                    $"Your budget is: " +
                    BudgetRules.FormatCurrency(budget.Value));

                decimal remaining = budget.Value - overallTotal;

                // string BudgetStatus;
                //
                // if (remaining < 0)
                // {
                //     console.WriteLine("Over budget: " + BudgetStatus);
                // }
                // else if (remaining < budget * 0.10m);
                // {
                //     console.WriteLine("Almost out: " + BudgetStatus);
                // }
                // else
                // {
                //     Console.WriteLine("on track: " + BudgetStatus);
                // }
                // Console.WriteLine($"Budget status: ${BudgetStatus}");
                // CORRECTION:
                // - BudgetStatus was never assigned.
                // - The semicolon breaks the else-if structure.
                // - Call the required method.
                // - A status string does not need a currency symbol.

                string budgetStatus =
                    BudgetRules.BudgetStatus(remaining, budget.Value);

                Console.WriteLine($"Budget status: {budgetStatus}");

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
            // CORRECTION:
            // - The old loop never ended.
            // - choice was outside its scope.
            // - The if had an incorrect trailing semicolon.
            // - break was outside a loop or switch in your original code.
            // The menu now provides the exit option.

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

// ADDED: A local input helper.
// Empty input is returned normally so each field can validate it.
// A closed input stream stops the add-expense flow instead of looping forever.
static string ReadRequiredInput()
{
    return Console.ReadLine()
        ?? throw new EndOfStreamException("Input is no longer available.");
}
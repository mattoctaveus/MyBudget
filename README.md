# MyBudget — Personal Budget Management System

Welcome to **MyBudget**! This C# console application helps users manage their income, expenses, savings, and personal budgets. The project includes a main application and an xUnit testing project.

## Project Structure

```text
MyBudget.sln
├── MyBudget/                  Main C# application
│   ├── mybudget.cs            Budget management functionality
│   ├── Program.cs             Console application entry point (if used)
│   └── MyBudget.csproj        C# project configuration
│
├── MyBudget.Tests/            Unit testing project
│   ├── UnitTest1.cs           Initial xUnit test file
│   └── MyBudget.Tests.csproj  Test project configuration
│
├── .gitignore                 Files excluded from Git
└── README.md                  Project documentation
```

*The structure above is the intended layout. Adjust the file names to match the files actually present in your repository.*

## How to Work

1. Open `MyBudget.sln` in Visual Studio or open the project folder in Visual Studio Code.
2. Restore the project's dependencies and build the application.
3. Develop the budget management features in `mybudget.cs`.
4. Implement unit tests in the `MyBudget.Tests` project to verify the application's functionality.
5. Run the tests regularly to identify and fix errors.
6. Commit your changes to GitHub as you complete each feature.

## Useful Commands

```bash
# Restore project dependencies
dotnet restore

# Compile the application
dotnet build

# Run unit tests
dotnet test

# Run the budget application
dotnet run --project MyBudget/MyBudget.csproj

# Check modified files
git status

# Stage project changes
git add .

# Create a commit
git commit -m "Update MyBudget application"

# Upload changes to GitHub
git push
```

## Development Guidelines

- Keep the application organized and readable.
- Use meaningful names for variables, methods, and classes.
- Implement input validation to handle incorrect user entries.
- Keep existing functionality intact when adding features.
- Test budget calculations and input-handling logic.
- Add comments where necessary to explain important logic.
- Maintain a clear GitHub commit history.

## Project Objectives

- Create a functional C# console-based budgeting application.
- Manage income and expenses.
- Perform budget calculations.
- Provide an interactive console menu.
- Validate user inputs.
- Improve code reliability through unit testing.
- Practice C#, .NET, Git, and GitHub development.

## Technologies

- **C#** — Programming language
- **.NET** — Application framework
- **Visual Studio Code / Visual Studio** — Development environment
- **xUnit** — Unit testing framework
- **Git** — Version control
- **GitHub** — Repository hosting

## Repository

[MyBudget on GitHub](https://github.com/mattoctaveus/MyBudget)

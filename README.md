# cli-menu

Simple C# cli menu. Includes a table class for displaying data in a table format.

## Installation

Use the package manager [nuget](https://www.nuget.org/packages/cli-menu/) to install cli-menu.

```bash
dotnet add package cli-menu
```

## Usage

### Basic Usage

```csharp
using cli_menu;

// Create a new menu
Menu menu = new Menu("Main Menu");

// Add a menu option
menu.AddOption("Option 1", () => Console.WriteLine("Option 1 selected"));

// Display the menu
menu.Start();
```

### Adding multiple options

```csharp
// Add multiple options
menu.AddOptions(
        new Option("Option 1", () => Console.WriteLine("Option 1 was selected!")),
        new Option("Option 2", () => Console.WriteLine("Option 2 was selected!")),
        new Option("Option 3", () => Console.WriteLine("Option 3 was selected!"))
    );
```

### Add an option that will change if you change the value of a variable

```csharp
// Create a variable
string option1 = "Option 1";

// Add a menu option
menu.AddOption(() => option1, () => {
    Console.WriteLine("Option 1 selected");
    option1 = "Option 1 selected";
});
```

### Add a header

```csharp
// Add a header
menu.Header = () => $"Current time is {DateTime.Now}";
```

## In development

- Adding a Table builder and allowing to integrate it in the menu

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)

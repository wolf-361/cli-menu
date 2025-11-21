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


## The ConsoleTable class

### Basic Usage

```csharp
using cli_menu;

// Create a new table
ConsoleTable table = new ConsoleTable();

// Add columns
table.AddColumn("Column 1")
    .Append("Row 1")
    .Append("Row 2");

// Display the table
table.Display();
```

### Adding multiple columns

```csharp
// Add multiple columns
table.AddColumns("Column 1", "Column 2", "Column 3");
```

### Adding multiple items to a row
```csharp
// Add multiple items to a row
table.AddRow("Item 1", "Item 1", "Item 1");
```

### Setting the table title

#### After creating the table

```csharp
// Set the table title
table.Title = "Table Title";
```

#### When creating the table

```csharp
// Set the table title
ConsoleTable table = new ConsoleTable("Table Title");
```

## Localisation

### Changing the language

```csharp
// Change the language to French
Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("fr-CA");
```

## Contributing

Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License

[MIT](https://choosealicense.com/licenses/mit/)

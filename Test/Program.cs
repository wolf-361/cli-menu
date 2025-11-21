using cli_menu;
using System.Globalization;

Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-CA");

ConsoleTable table = new()
{
    Title = "Test"
};

table.AddColumns("Test 1", "Test 2")
    .AddColumns("Test 3")
    .Append("Test 1", "Test 2")
    .Append("Test 3", "Test 4", null)
    .Append(43.999999, 1, true)
    .Append(43.999999, 1, true)
    .Append(1, 2, 3)
    .Append<int>([4, 5, 6]);


var menu = new Menu("Test Menu")
{
    // Add the delegate to the menu header. that will change the header text to the current time.
    Header = () => $"Test Menu - {DateTime.Now}"
};

string test = "Option 1 (Test Table)";

menu.AddOptions(
        new Option(test.ToString, () =>
        {
            test = "Test Table was selected!";
            table.Display();
            Console.WriteLine(test);
        }),
        new Option("Option 2", () => Console.WriteLine("Option 2 was selected!")),
        new Option("Option 3", () => Console.WriteLine("Option 3 was selected!"))
    );

// Add options to the menu.
menu.AddOption("Option 4", () => Console.WriteLine("Option 4 was selected!"))
    .AddOption("Option 5", () => Console.WriteLine("Option 5 was selected!"))
    .AddOption("Option 6", () => Console.WriteLine("Option 6 was selected!"))
    .AddOption("Option 7", () => Console.WriteLine("Option 7 was selected!"))
    .Start();


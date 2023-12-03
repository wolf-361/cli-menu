using cli_menu;
using System.Globalization;

Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-CA");

var menu = new Menu("Test Menu");

string test = "Option 1";

menu.AddOptions(
        new Option(() => test, () =>
        {
            test = "Test as été selectionner";
            Console.WriteLine(test);
        }),
        new Option("Option 2", () => Console.WriteLine("Option 2 was selected!")),
        new Option("Option 3", () => Console.WriteLine("Option 3 was selected!"))
    );

// Add options to the menu.
menu.AddOption("Option 4", () => Console.WriteLine("Option 4 was selected!"));
menu.AddOption("Option 5", () => Console.WriteLine("Option 5 was selected!"));
menu.AddOption("Option 6", () => Console.WriteLine("Option 6 was selected!"));
menu.AddOption("Option 7", () => Console.WriteLine("Option 7 was selected!"));
menu.AddOption("Option 8", () => Console.WriteLine("Option 8 was selected!"));
menu.AddOption("Option 9", () => Console.WriteLine("Option 9 was selected!"));
menu.AddOption("Option 10", () => Console.WriteLine("Option 10 was selected!"));

// Add the delegate to the menu header. that will change the header text to the current time.
menu.Header = () => $"Test Menu - {DateTime.Now}";


//menu.Start();

// Console table test
ConsoleTable table = new("Test Table");

table.AddColumns("Test 1", "Test 2", "Test 3");
table.Title = "Test";

table.Append("Test 1", "Test 2");
table.Append("Test 3", "Test 4", null);
table.Append(43.999999, 1, true);
table.Append(1, 2, 3);
table.Append(new List<int> { 4, 5, 6 });
table.Append(new[] { 7, 8, 9 });

table.Display();


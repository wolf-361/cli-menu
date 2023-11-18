using cli_menu;

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
ConsoleTable table = new();

table.SetTitles("Test Table", "Test Table 2", "Test 3");

table.Append("Test 1", "Test 2");
table.Append("Test 3", "Test 4");
table.Append("Test 5", "Test 6");
table.Append("Test 7", "Test 8");
table.Append("Test 9", "Test 10", "Test 11111111");

table.Display();


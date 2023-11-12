// See https://aka.ms/new-console-template for more information


// Create a new menu.

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

menu.Start();


namespace cli_menu;

public class Menu
{
    private readonly List<Option> _options;
    private Func<string> Title { get; set; }
    private int SelectedOptionIndex { get; set; } = 0; // Default to the first option.
    public Func<string>? Header { get; set; }

    public Menu(string title)
    {
        Title = () => title;
        _options = new List<Option>();
    }
    
    public Menu(Func<string> title)
    {
        Title = title;
        _options = new List<Option>();
    }
    
    public Menu(string title, params Option[] options)
    {
        Title = () => title;
        _options = options.ToList();
    }
    
    public Menu(Func<string> title, params Option[] options)
    {
        Title = title;
        _options = options.ToList();
    }
    
    // --- Option management ---
    
    /// <summary>
    /// Adds an option to this menu.
    /// </summary>
    /// <param name="option">The option object to add to the menu</param>
    public void AddOption(Option option)
    {
        _options.Add(option);
    }
    
    /// <summary>
    /// Adds an option to this menu.
    /// </summary>
    /// <param name="name">The name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = false) If we should wait for the user on completion</param>
    public void AddOption(string name, Delegate action, bool waitForUser = true)
    {
        _options.Add(new Option(name, action, waitForUser));
    }
    
    /// <summary>
    /// Adds multiple options to this menu.
    /// </summary>
    /// <param name="options">Thes options to add to the menu</param>
    public void AddOptions(params Option[] options)
    {
        _options.AddRange(options);
    }
    
    // --- Menu management ---

    /// <summary>
    /// Displays the menu and waits for the user to select an option.
    /// </summary>
    public void Start()
    {
        Option selectedOption;
        do
        {
            selectedOption = NextChoice();
            selectedOption.Invoke();
        } while (selectedOption.Action != null);
    }

    private Option NextChoice()
    {
        var done = false;

        do
        {
            Show();

            var input = Console.ReadKey(true);

            switch (input.Key)
            {
                case ConsoleKey.UpArrow:
                    MoveUp();
                    break;
                case ConsoleKey.DownArrow:
                    MoveDown();
                    break;
                case ConsoleKey.Enter:
                    done = true;
                    break;
            }
        } while (!done);
        
        return _options[SelectedOptionIndex];
    }
    
    private void MoveUp() => SelectedOptionIndex = Math.Max(0, SelectedOptionIndex - 1);
    
    private void MoveDown() => SelectedOptionIndex = Math.Min(_options.Count - 1, SelectedOptionIndex + 1);

    private void Show()
    {
        Console.Clear();
        
        // Write the title.
        Console.WriteLine($"--- {Title()} ---\n");
        
        // Write the header if there are any header func in the menu.
        if (Header != null)
            Console.WriteLine($"\n{Header()}\n");
        
        // Write the options.
        for (var i = 0; i < _options.Count; i++)
        {
            var option = _options[i];
            var prefix = i == SelectedOptionIndex ? "> " : $"  ";
            // Change the color of the selected option.
            Console.ForegroundColor = i == SelectedOptionIndex ? ConsoleColor.Magenta : ConsoleColor.Gray;
            Console.WriteLine($"{prefix}{option}");
        }
        
        // Reset the color.
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine("Use the arrow keys to navigate the menu. Select an option by pressing enter.");
        
    }
}
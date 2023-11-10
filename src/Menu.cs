namespace cli_menu;

public class Menu
{
    private readonly List<Option> _options;
    private readonly string _title;
    private int SelectedOptionIndex { get; set; } = 0; // Default to the first option.
    private Delegate? HeaderDelegate { get; set; }
    
    public Menu(string title)
    {
        _title = title;
        _options = new List<Option>();
    }
    
    public Menu(Func<string> title)
    {
        _title = title();
        _options = new List<Option>();
    }
    
    public Menu(string title, params Option[] options)
    {
        _title = title;
        _options = options.ToList();
    }
    
    public Menu(Func<string> title, params Option[] options)
    {
        _title = title();
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
    
    /// <summary>
    /// Sets the header of the menu.
    /// </summary>
    /// <param name="headerDelegate">A delegate that returns the header of the menu</param>
    public void SetHeader(Func<string> headerDelegate)
    {
        HeaderDelegate = headerDelegate;
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
        
        Console.WriteLine(Header());
        
        // Write the Delegate header if there are any delegates in the menu.
        Console.WriteLine(HeaderDelegate?.DynamicInvoke());
        
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

    private string Header()
    {
        return $"--- {_title} ---";
    }
    
    public override string ToString()
    {
        return _title;
    }

}
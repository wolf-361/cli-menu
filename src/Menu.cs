using cli_menu.Properties;

namespace cli_menu;

public class Menu
{
    private readonly List<Option> _options;
    public Func<string> Title { get; set; }
    public Func<string>? Header { get; set; }

    private int _selectedOptionIndex; // Default to the first option.
    private Option SelectedOption
    {
        get { return _options[_selectedOptionIndex]; }
        set { _selectedOptionIndex = _options.IndexOf(value); }
    }

    public Menu(Func<string> title, List<Option> list)
    {
        Title = title;
        _options = list;
    }

    public Menu(string title, params Option[] options)
        : this(title.ToString, options.ToList()) { }

    // --- Option management ---

    /// <summary>
    /// Adds an option to this menu.
    /// </summary>
    /// <param name="option">The option object to add to the menu</param>
    public Menu AddOption(Option option)
    {
        _options.Add(option);

        return this;
    }

    /// <summary>
    /// Adds an option to this menu.
    /// </summary>
    /// <param name="name">The name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = false) If we should wait for the user on completion</param>
    public Menu AddOption(string name, Action action, bool waitForUser = true)
        => AddOption(new Option(name, action, waitForUser));

    /// <summary>
    /// Adds multiple options to this menu.
    /// </summary>
    /// <param name="options">Thes options to add to the menu</param>
    public Menu AddOptions(params Option[] options)
    {
        _options.AddRange(options);

        return this;
    }

    // --- Menu management ---

    /// <summary>
    /// Displays the menu and waits for the user to select an option.
    /// </summary>
    public void Start()
    {
        Console.CursorVisible = false;

        _options.Add(Option.ExitOption);
        bool stop = false;

        while(!stop)
        {
            Show();

            // Blocking UI Thread until next ConsoleKey input
            if (NextChoiceSelected(waitForInput: true))
            {
                if (SelectedOption == Option.ExitOption)
                    stop = true;
                else
                    SelectedOption.Invoke();
            }
        }

        Console.CursorVisible = true;
        ClearConsole();
    }

    /// <summary>
    /// Check if the next option has been selected
    /// </summary>
    private bool NextChoiceSelected(bool waitForInput = false)
    {
        if (!Console.KeyAvailable && !waitForInput)
            return false;

        var input = Console.ReadKey(true);

        switch (input.Key)
        {
            case ConsoleKey.UpArrow:
                MoveUp();
                break;
            case ConsoleKey.DownArrow:
                MoveDown();
                break;
            case ConsoleKey.Home:
                MoveFirst();
                break;
            case ConsoleKey.End:
                MoveLast();
                break;
            case ConsoleKey.Enter:
                return true;
        }
        return false;
    }

    private void MoveUp() => _selectedOptionIndex = Math.Max(0, _selectedOptionIndex - 1);

    private void MoveDown() => _selectedOptionIndex = Math.Min(_options.Count - 1, _selectedOptionIndex + 1);

    private void MoveFirst() => _selectedOptionIndex = 0;

    private void MoveLast() => _selectedOptionIndex = _options.Count - 1;

    private void Show()
    {
        var currentForegroudColor = Console.ForegroundColor;

        ClearConsole();
        Console.WriteLine($"--- {Title()} ---");
        Console.WriteLine();

        if (Header != null)
        {
            Console.WriteLine();
            Console.WriteLine(Header());
            Console.WriteLine();
        }

        for (int i = 0; i < _options.Count; i++)
        {
            if (i == _selectedOptionIndex)
            {
                Console.ForegroundColor = ConsoleColor.Magenta; // Change the color of the selected option.
                Console.WriteLine('>' + SelectedOption.ToString());
                Console.ForegroundColor = currentForegroudColor; // Reset color
            }
            else
                Console.WriteLine($" {_options[i]}");
        }

        Console.WriteLine();
        Console.Write(strings.UseArrowKeysToNavigate + ' ');
        Console.WriteLine(strings.PressEnterToSelectOption);
    }

    private void ClearConsole()
    {
        Console.Clear();

        // \x1b[3J
        // erase entire screen + scrollback
        // Ref: https://alligatr.co.uk/ansi-codes/
        Console.WriteLine("\x1b[3J");
    }
}
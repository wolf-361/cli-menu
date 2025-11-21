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
        _options.Add(Option.ExitOption);

        do
        {
            NextChoice();
            SelectedOption.Invoke();
        } while (SelectedOption != Option.ExitOption);
    }

    /// <summary>
    /// Select the next option
    /// </summary>
    /// <returns>Option: Kept return in case of future change</returns>
    private Option NextChoice()
    {
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
                    return SelectedOption;
            }
        } while (true);
    }

    private void MoveUp() => _selectedOptionIndex = Math.Max(0, _selectedOptionIndex - 1);

    private void MoveDown() => _selectedOptionIndex = Math.Min(_options.Count - 1, _selectedOptionIndex + 1);

    private void Show()
    {
        Console.Clear();

        // Write the title.
        Console.WriteLine($"--- {Title()} ---\n");

        // Write the header if there are any header func in the menu.
        if (Header != null)
            Console.WriteLine($"\n{Header()}\n");

        // Write the options.
        foreach (Option option in _options)
        {
            var isSelected = option == SelectedOption;

            // Change the color of the selected option.
            Console.ForegroundColor = isSelected ? ConsoleColor.Magenta : ConsoleColor.Gray;
            Console.WriteLine($"{(isSelected ? ">" : " ")} {option}");
        }

        // Reset the color.
        Console.ForegroundColor = ConsoleColor.Gray;
        Console.WriteLine();

        Console.Write(strings.UseArrowKeysToNavigate + ' ');
        Console.WriteLine(strings.PressEnterToSelectOption);
    }
}
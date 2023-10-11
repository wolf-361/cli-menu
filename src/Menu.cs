namespace cli_menu;

public class Menu
{
    private readonly List<Option> _options;
    private readonly string _title;
    
    public Menu(string title)
    {
        _title = title;
        _options = new List<Option>();
    }
    
    public Menu(string title, params Option[] options)
    {
        _title = title;
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
    public void AddOption(string name, Delegate action, bool waitForUser = false)
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

}
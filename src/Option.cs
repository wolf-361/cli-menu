using cli_menu.Properties;

namespace cli_menu;

/// <summary>
/// A menu option
/// </summary>
public class Option
{
    private readonly Func<string> _name;
    private readonly Action _action;
    private readonly bool _waitForUser;

    public static readonly Option ExitOption = new(strings.Exit, () => { }, false);

    /// <summary>
    /// Creates a new option.
    /// </summary>
    /// <param name="name">The name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = true) If we should wait for the user on completion</param>
    public Option(string name, Action action, bool waitForUser = true)
        : this(() => name, action, waitForUser) { }

    /// <summary>
    /// Creates a new option.
    /// </summary>
    /// <param name="name">A function that returns the name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = true) If we should wait for the user on completion</param>
    public Option(Func<string> name, Action action, bool waitForUser = true)
    {
        _name = name;
        _action = action;
        _waitForUser = waitForUser;
    }

    /// <summary>
    /// Waits for the user to press a key.
    /// </summary>
    private static void WaitForUserInput()
    {
        Console.WriteLine(strings.PressKeyToContinue);
        Console.ReadKey(true);
    }

    /// <summary>
    /// Invokes the action of this option.
    /// </summary>
    public void Invoke()
    {
        Console.Clear(); // Clear the console before invoking the action.

        _action.DynamicInvoke();

        // If the option is set to wait for user input, wait for the user to press enter.
        if (_waitForUser)
            WaitForUserInput();
    }

    public override string ToString()
    {
        return _name();
    }
}
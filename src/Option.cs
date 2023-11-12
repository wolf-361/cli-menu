namespace cli_menu;

public class Option
{
    public Func<string> Name { get; }
    public Delegate? Action { get; }
    public bool WaitForUser { get; }
    
    /// <summary>
    /// Creates a new option.
    /// </summary>
    /// <param name="name">The name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = false) If we should wait for the user on completion</param>
    public Option(string name, Delegate action, bool waitForUser = true)
    {
        Name = () => name;
        Action = action;
        WaitForUser = waitForUser;
    }

    /// <summary>
    /// Creates a new option.
    /// </summary>
    /// <param name="name">A function that returns the name of the option</param>
    /// <param name="action">The action the option will do when selected</param>
    /// <param name="waitForUser">(optional, default = false) If we should wait for the user on completion</param>
    public Option(Func<string> name, Delegate action, bool waitForUser = true)
    {
        Name = name;
        Action = action;
        WaitForUser = waitForUser;
    }
    
    /// <summary>
    /// Waits for the user to press a key.
    /// </summary>
    private static void WaitForUserInput()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }
    
    /// <summary>
    /// Invokes the action of this option.
    /// </summary>
    public void Invoke()
    {
        Console.Clear(); // Clear the console before invoking the action.

        // Refresh the option name

        
        Action?.DynamicInvoke();
        
        // If the option is set to wait for user input, wait for the user to press enter.
        if (WaitForUser)
            WaitForUserInput();
    }
    
    public override string ToString()
    {
        return Name();
    }
}
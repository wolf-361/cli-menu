namespace cli_menu;

public class ConsoleTable
{
    // Constants

    // Instance Fields
    private readonly List<string[]> _rows = new();
    private readonly List<int> _columnWidths = new();
    private readonly List<string> _columnNames = new();
    private int _columnCount = 0;
}
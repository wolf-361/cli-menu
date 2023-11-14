namespace cli_menu;

public class ConsoleTable
{
    // Constants
    private static readonly string TOP_LEFT = "┌";
    private static readonly string TOP_RIGHT = "┐";
    private static readonly string BOTTOM_LEFT = "└";
    private static readonly string BOTTOM_RIGHT = "┘";
    private static readonly string HORIZONTAL_LINE = "-";
    private static readonly string VERTICAL_LINE = "|";
    private static readonly string CROSS = "┼";
    private static readonly string LEFT_CROSS = "├";
    private static readonly string RIGHT_CROSS = "┤";
    private static readonly string TOP_CROSS = "┬";
    private static readonly string BOTTOM_CROSS = "┴";
    private static readonly string SPACE = " ";
    private static readonly string NEW_LINE = "\n";
    private static readonly string EMPTY = "";
    private static readonly string ND = "N/D";

    // Instance Fields
    private readonly List<string[]> _rows = new();
    private readonly List<int> _columnWidths = new();
    private readonly List<string> _columnNames = new();
    private int _columnCount = 0;

    // ----- Private Methods -----

    /// <summary>
    /// Get the width of the column
    /// </summary>
    /// <param name="columnIndex">The index of the column</param>
    /// <returns>The width of the column</returns>
    private int GetColumnWidth(int columnIndex)
    {
        return _columnWidths[columnIndex];
    }

    /// <summary>
    /// Returns the total width of the table (including borders)
    /// </summary>
    /// <returns>The width of the table</returns>
    private int GetTotalWidth()
    {
        return _columnWidths.Sum() + (_columnCount * 3) + 1;
    }

    // ----- Public Methods -----

    public void SetColumnNames(params string[] columnNames)
    {
        _columnNames.AddRange(columnNames);
        _columnCount = columnNames.Length;
        _columnWidths.AddRange(columnNames.Select(name => name.Length));
    }

}
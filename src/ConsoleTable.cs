using cli_menu.Properties;
using cli_menu.Utils;
using System.Text;

namespace cli_menu;

public enum BorderType
{
    Top,
    Middle,
    Bottom,
}

public class ConsoleTable
{
    // Constants
    private const char HorizontalLine = '-';
    private const char VerticalLine = '|';
    private const char Space = ' ';

    private static readonly Dictionary<BorderType, char[]> borders = new()
    {
        { BorderType.Top,    new char[3] { '┌', '┬', '┐' } },
        { BorderType.Middle, new char[3] { '├', '┼', '┤' } },
        { BorderType.Bottom, new char[3] { '└', '┴', '┘' } }
    };

    // Instance Fields
    private string? _title;
    private readonly List<string> _columnNames = new();
    private readonly List<string[]> _rows = new();
    private readonly List<int> _columnWidths = new();
    private int _columnCount; // Default to 0

    // Accessors
    public string? Title { set { _title = value; } }

    // ----- Constructors -----
    public ConsoleTable(string? title = null)
    {
        _title = title;
    }

    // ----- Private Methods -----


    private static string GetLine(int columnWidth)
    {
        return new string(HorizontalLine, columnWidth + 2);
    }

    private IEnumerable<string> GetLines()
    {
        for (var i = 0; i < _columnCount; i++)
        {
            yield return GetLine(_columnWidths[i]);
        }
    }

    private string GetBorder(BorderType type)
    {
        StringBuilder sb = new();

        sb.Append(borders[type][0]);
        sb.AppendJoin(borders[type][1], GetLines());
        sb.Append(borders[type][2]);

        return sb.ToString();
    }

    private IEnumerable<string> GetRowColumnsCentered(string[] row)
    {
        for (var i = 0; i < _columnCount; i++)
        {
            yield return row[i].PadCenter(_columnWidths[i], Space);
        }
    }

    private string GetRowString(string[] row)
    {
        StringBuilder sb = new();
        var separator = $"{Space}{VerticalLine}{Space}";

        Array.Resize(ref row, _columnCount);

        sb.Append(separator.Substring(1));
        sb.AppendJoin(separator, GetRowColumnsCentered(row));
        sb.Append(separator.Substring(0, separator.Length - 1));

        return sb.ToString();
    }

    private void AddRow(string[] row)
    {
        var width = row.Length;

        // Resize columnWidths and adjust column Count if necessary
        if (width > _columnCount)
        {
            _columnWidths.AddRange(new int[width - _columnCount]);
            _columnCount = width;
        }

        // Update every columns width
        for (var i = 0; i < width; i++)
        {
            _columnWidths[i] = Math.Max(row[i].Length, _columnWidths[i]);
        }

        // Ajust the length of the row
        Array.Resize(ref row, _columnCount);
        _rows.Add(row);
    }

    // ----- Public Methods -----

    /// <summary>
    /// Add one or multiple columns to the table.
    /// </summary>
    /// <param name="columnNames">The column name(s)</param>
    public void AddColumns(params string[] columnNames)
    {
        for (int i = 0; i < columnNames.Length; i++)
        {
            _columnNames.Add(columnNames[i]);
            _columnCount = Math.Max(_columnCount, _columnNames.Count);

            if (_columnWidths.Count < _columnCount)
                _columnWidths.Add(0);

            var widthIndex = _columnCount - 1;
            _columnWidths[widthIndex] = Math.Max(_columnWidths[widthIndex], columnNames[i].Length);
        }
    }

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <param name="items">Items to be added in a single row</param>
    public void Append(params object?[] items)
    {
        Append<object?>(items);
    }

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <typeparamref name="T"/>
    /// <param name="items">The collection (T) to be added in a single row</param>
    public void Append<T>(IEnumerable<T> items)
    {
        AddRow(items.Select(item => item?.ToString() ?? "").ToArray());
    }

    /// <summary>
    /// Display the table in the console.
    /// </summary>
    public void Display()
    {
        // If the table is empty, display a message
        if (_rows.Count == 0)
        {
            Console.WriteLine(strings.EmptyTable);
            return;
        }

        // Display the bottom border
        Console.WriteLine(this);
    }

    public override string ToString()
    {
        StringBuilder sb = new();

        // Add the title (if there is one)
        if (_title != null)
        {
            sb.AppendLine(_title);
        }

        // Sandwich the table colomn names between the top and middle border (if there are any)
        if (_columnNames.Count > 0)
        {
            sb.AppendLine(GetBorder(BorderType.Top));
            sb.AppendLine(GetRowString(_columnNames.ToArray()));
            sb.AppendLine(GetBorder(BorderType.Middle));
        }
        else
        {
            sb.AppendLine(GetBorder(BorderType.Top));
        }

        // Add the rows
        foreach (var row in _rows)
        {
            sb.AppendLine(GetRowString(row));
        }

        // Add the bottom border
        sb.AppendLine(GetBorder(BorderType.Bottom));
        return sb.ToString();
    }
}
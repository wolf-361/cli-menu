using cli_menu.Properties;
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
        { BorderType.Top, new char[3] {'┌', '┬', '┐'} },
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
    public string Title { set { _title = value; } }

    // ----- Constructors -----
    public ConsoleTable(string? title = null)
    {
        _title = title;
    }

    // ----- Private Methods -----


    private static string GetLine(int columnWidth)
    {
        StringBuilder sb = new();

        for (var i = 0; i < columnWidth + 2; i++)
            sb.Append(HorizontalLine);

        return sb.ToString();
    }

    private string GetBorder(BorderType type)
    {
        StringBuilder sb = new();
        sb.Append(borders[type][0]);
        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(GetLine(_columnWidths[i]));
            sb.Append(borders[type][1]);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(borders[type][2]);
        return sb.ToString();
    }

    private string GetRowString(string[] row)
    {
        StringBuilder sb = new();

        // Ajust the length of the rows if needed
        if (row.Length < _columnCount)
        {
            var newRow = new string[_columnCount];
            for (var i = 0; i < _columnCount; i++)
            {
                if (row.Length > i)
                    newRow[i] = row[i];
                else
                    newRow[i] = string.Empty;
            }
            row = newRow;
        }

        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(VerticalLine);
            sb.Append(Space);

            // Center the text in the column
            var padding = (_columnWidths[i] - row[i].Length) / 2;
            sb.Append(row[i].PadLeft(padding + row[i].Length, Space).PadRight(_columnWidths[i], Space));
            sb.Append(Space);
        }
        sb.Append(VerticalLine);
        return sb.ToString();
    }

    private void AddRow(string[] row)
    {
        // Update the column count (fist cause the value is used later)
        if (row.Length > _columnCount)
            _columnCount = row.Length;

        // Update the column widths
        for (var i = 0; i < _columnCount; i++)
        {
            var width = row[i].Length;

            // Check if there is currently a value at this index
            try
            {
                if (width > _columnWidths[i])
                {
                    _columnWidths[i] = width;
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                _columnWidths.Add(width);
            }

        }

        // Add the row to the table (ajust the length of the rows if needed)
        if (row.Length < _columnCount)
        {
            var newRow = new string[_columnCount];
            for (var i = 0; i < _columnCount; i++)
            {
                if (row.Length > i)
                    newRow[i] = row[i];
                else
                    newRow[i] = string.Empty;
            }
            row = newRow;
        }
        _rows.Add(row);
    }

    // ----- Public Methods -----

    /// <summary>
    /// Add one or multiple columns to the table.
    /// </summary>
    /// <param name="columnNames">The column name(s)</param>
    public void AddColumn(params string[] columnNames)
    {
        _columnNames.AddRange(columnNames);
        _columnCount += columnNames.Length;
        _columnWidths.AddRange(columnNames.Select(name => name.Length));
    }

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <typeparamref name="T"/>
    /// <param name="items">The items (T)</param>
    public void Append<T>(params T[] items) where T : notnull
    {
        Append((IEnumerable<T>)items);
    }

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <typeparamref name="T"/>
    /// <param name="items">The array (T) to add</param>
    public void Append<T>(IEnumerable<T> items) where T : notnull
    {
        AddRow(items.Select(item => item.ToString() ?? "").ToArray());
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
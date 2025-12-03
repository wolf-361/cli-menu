using cli_menu.Properties;
using cli_menu.Utils;
using System.Text;

namespace cli_menu;

public enum BorderVerticalType
{
    Left,
    Middle,
    Right,
}

public enum BorderHorizontalType
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

    private readonly char[,] bordersMatrix =
    {
        { '┌', '┬', '┐' },
        { '├', '┼', '┤' },
        { '└', '┴', '┘' }
    };

    // Instance Fields
    private readonly List<Column> _columns = [];
    private readonly List<List<string>> _rows = [];

    private class Column()
    {
        public string? Title;
        public int Width;
    }

    // Accessors
    public string? Title { get; set; }

    // ----- Constructors -----
    public ConsoleTable(string? title = null)
    {
        Title = title;
    }

    // ----- Private Methods -----

    private static string GetLine(int columnWidth)
    {
        return new string(HorizontalLine, columnWidth + 2);
    }

    private IEnumerable<string> GetLines()
    {
        for (var i = 0; i < _columns.Count; i++)
        {
            yield return GetLine(_columns[i].Width);
        }
    }

    private char GetBorderSymbol(BorderHorizontalType horinzontal, BorderVerticalType vertical)
    {
        return bordersMatrix[(int)horinzontal, (int)vertical];
    }

    private string GetBorder(BorderHorizontalType type)
    {
        StringBuilder sb = new();

        sb.Append(GetBorderSymbol(type, BorderVerticalType.Left));
        sb.AppendJoin(GetBorderSymbol(type, BorderVerticalType.Middle), GetLines());
        sb.Append(GetBorderSymbol(type, BorderVerticalType.Right));

        return sb.ToString();
    }

    private IEnumerable<string?> GetRowColumnsCentered(IEnumerable<string?> row)
    {
        for (var i = 0; i < _columns.Count; i++)
        {
            string element = row.ElementAtOrDefault(i) ?? string.Empty;

            yield return element.PadCenter(_columns[i].Width, Space);
        }
    }

    private string GetRowString(IEnumerable<string?> row)
    {
        StringBuilder sb = new();
        var separator = $"{Space}{VerticalLine}{Space}";

        sb.Append(separator.AsSpan(1))
            .AppendJoin(separator, GetRowColumnsCentered(row))
            .Append(separator.AsSpan(0, separator.Length - 1));

        return sb.ToString();
    }

    private void AddRow(List<string> row)
    {
        // Update columns widths
        for (var i = 0; i < row.Count; i++)
        {
            if (_columns.Count <= i)
                _columns.Add(new() { Width = row[i].Length });
            else
                _columns[i].Width = Math.Max(_columns[i].Width, row[i].Length);
        }

        _rows.Add(row);
    }

    // ----- Public Methods -----

    /// <summary>
    /// Add one or multiple columns to the table.
    /// </summary>
    /// <param name="columnNames">The column name(s)</param>
    public ConsoleTable AddColumns(params string[] columnNames)
    {
        foreach (var columnName in columnNames)
        {
            var column = new Column()
            {
                Title = columnName,
                Width = columnName.Length
            };

            _columns.Add(column);
        }

        return this;
    }

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <param name="items">Items to be added in a single row</param>
    public ConsoleTable Append(params object?[] items)
        => Append<object?>(items);

    /// <summary>
    /// Append a row to the table.
    /// </summary>
    /// <typeparamref name="T"/>
    /// <param name="items">The collection (T) to be added in a single row</param>
    public ConsoleTable Append<T>(IEnumerable<T?> items)
    {
        AddRow(items.Select(item => item?.ToString() ?? string.Empty).ToList());

        return this;
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
        if (!string.IsNullOrWhiteSpace(Title))
        {
            sb.AppendLine(Title);
        }

        sb.AppendLine(GetBorder(BorderHorizontalType.Top))
            .AppendLine(GetRowString(_columns.Select(c => c.Title)))
            .AppendLine(GetBorder(BorderHorizontalType.Middle));

        // Add the rows
        foreach (var row in _rows)
        {
            sb.AppendLine(GetRowString(row));
        }

        // Add the bottom border
        sb.AppendLine(GetBorder(BorderHorizontalType.Bottom));
        return sb.ToString();
    }
}
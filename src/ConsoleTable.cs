using cli_menu.Properties;
using System.Text;

namespace cli_menu;

public class ConsoleTable
{
    // Constants
    private const char TopLeft = '┌';
    private const char TopRight = '┐';
    private const char BottomLeft = '└';
    private const char BottomRight = '┘';
    private const char HorizontalLine = '-';
    private const char VerticalLine = '|';
    private const char Cross = '┼';
    private const char LeftCross = '├';
    private const char RightCross = '┤';
    private const char TopCross = '┬';
    private const char BottomCross = '┴';
    private const char Space = ' ';
    private const string Empty = "";

    // Instance Fields
    private readonly List<string[]> _rows = new();
    private readonly List<int> _columnWidths = new();
    private readonly List<string> _columnNames = new();
    private int _columnCount; // Default to 0

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

    private static string GetLine(int columnWidth)
    {
        StringBuilder sb = new();

        for (var i = 0; i < columnWidth + 2; i++)
            sb.Append(HorizontalLine);

        return sb.ToString();
    }

    private string GetTopBorder()
    {
        StringBuilder sb = new();
        sb.Append(TopLeft);
        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(GetLine(GetColumnWidth(i)));
            sb.Append(TopCross);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(TopRight);
        return sb.ToString();
    }

    private string GetBottomBorder()
    {
        StringBuilder sb = new();
        sb.Append(BottomLeft);
        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(GetLine(GetColumnWidth(i)));
            sb.Append(BottomCross);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(BottomRight);
        return sb.ToString();
    }

    private string GetMiddleBorder()
    {
        StringBuilder sb = new();
        sb.Append(LeftCross);
        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(GetLine(GetColumnWidth(i)));
            sb.Append(Cross);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(RightCross);
        return sb.ToString();
    }

    private string GetRowString(string[] row)
    {
        StringBuilder sb = new();
        for (var i = 0; i < _columnCount; i++)
        {
            sb.Append(VerticalLine);
            sb.Append(Space);

            // Center the text in the column
            var padding = (GetColumnWidth(i) - row[i].Length) / 2;
            sb.Append(row[i].PadLeft(padding + row[i].Length, Space).PadRight(GetColumnWidth(i), Space));
            sb.Append(Space);
        }
        sb.Append(VerticalLine);
        return sb.ToString();
    }

    private void AddRow(string[] row)
    {
        // Add the row to the table (ajust the length of the row if needed)
        if (row.Length < _columnCount)
        {
            var newRow = new string[_columnCount];
            for (var i = 0; i < _columnCount; i++)
            {
                if (row.Length > i)
                    newRow[i] = row[i];
                else
                    newRow[i] = Empty;
            }
            row = newRow;
        }
        _rows.Add(row);

        // Update the column widths
        for (var i = 0; i < _columnCount; i++)
        {
            var width = row[i].Length;
            if (width > GetColumnWidth(i))
            {
                _columnWidths[i] = width;
            }
        }

        // Update the column count
        if (row.Length > _columnCount)
            _columnCount = row.Length;
    }

    // ----- Public Methods -----

    public void SetTitles(params string[] columnNames)
    {
        _columnNames.AddRange(columnNames);
        _columnCount = columnNames.Length;
        _columnWidths.AddRange(columnNames.Select(name => name.Length));
    }

    public void Append(params string[] items)
    {
        AddRow(items);
    }

    public void Display()
    {
        // If the table is empty, display a message
        if (_rows.Count == 0)
        {
            Console.WriteLine(strings.EmptyTable);
            return;
        }

        // Display the top border
        Console.WriteLine(GetTopBorder());

        // Display the column names
        Console.WriteLine(GetRowString(_columnNames.ToArray()));

        // Display the middle border
        Console.WriteLine(GetMiddleBorder());

        // Display the rows
        foreach (var row in _rows)
        {
            Console.WriteLine(GetRowString(row));
        }

        // Display the bottom border
        Console.WriteLine(GetBottomBorder());
    }
}
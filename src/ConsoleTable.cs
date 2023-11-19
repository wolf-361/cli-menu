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
            sb.Append(GetLine(_columnWidths[i]));
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
            sb.Append(GetLine(_columnWidths[i]));
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
            sb.Append(GetLine(_columnWidths[i]));
            sb.Append(Cross);
        }
        sb.Remove(sb.Length - 1, 1);
        sb.Append(RightCross);
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
                    newRow[i] = Empty;
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
                    newRow[i] = Empty;
            }
            row = newRow;
        }
        _rows.Add(row);
    }

    // ----- Public Methods -----

    public void AddColumn(string columnName)
    {
        _columnNames.Add(columnName);
        _columnCount++;
        _columnWidths.Add(columnName.Length);
    }

    public void AddColumns(params string[] columnNames)
    {
        _columnNames.AddRange(columnNames);
        _columnCount += columnNames.Length;
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

        // Display the bottom border
        Console.WriteLine(this);
    }

    // return the table as a string
    public override string ToString()
    {
        StringBuilder sb = new();

        // Sandwich the table colomn names between the top and middle border (if there are any)
        if (_columnNames.Count > 0)
        {
            sb.AppendLine(GetTopBorder());
            sb.AppendLine(GetRowString(_columnNames.ToArray()));
            sb.AppendLine(GetMiddleBorder());
        }
        else
        {
            sb.AppendLine(GetTopBorder());
        }

        // Add the rows
        foreach (var row in _rows)
        {
            sb.AppendLine(GetRowString(row));
        }

        // Add the bottom border
        sb.AppendLine(GetBottomBorder());
        return sb.ToString();
    }
}
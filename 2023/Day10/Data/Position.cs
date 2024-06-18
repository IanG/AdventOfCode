namespace Day10.Data;

public class Position
{
    public int Row { get; } 
    public int Column { get; }

    public Position(int row, int column) { Row = row; Column = column; }

    public override string ToString() => $"(Row {Row}, Col {Column})";

    public override int GetHashCode() => $"{Row}{Column}".GetHashCode();
    public override bool Equals(object? obj) => this.GetHashCode() == obj.GetHashCode();
}
namespace LinqOfCode2024.Utils;

public record AocInput(string RawValue)
{
    public IEnumerable<string> ReadLines()
    {
        return RawValue.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    }
};
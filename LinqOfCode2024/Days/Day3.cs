using System.Text.RegularExpressions;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day3 : AocDayBase<int, int>
{
    protected override int Day => 3;
    public override int Puzzle1()
    {
        return Regex.Matches(Input.RawValue, "mul\\((\\d+),(\\d+)\\)")
            .Select(x => int.Parse(x.Groups[1].Value) * int.Parse(x.Groups[2].Value)).Sum();
    }

    public override int Puzzle2()
    {
        var matches = Regex
            .Matches(Input.RawValue, "(?:(don't|do)(?:\\(\\)).*?)*mul\\((\\d+),(\\d+)\\)");

        var enabled = true;
        var sum = 0;
        foreach (Match match in matches)
        {
            enabled = match.Groups[1].Value switch
            {
                "don't" => false,
                "do" => true,
                _ => enabled
            };

            if (enabled)
            {
                sum += int.Parse(match.Groups[2].Value) * int.Parse(match.Groups[3].Value);
            }
        }

        return sum;
    }
}
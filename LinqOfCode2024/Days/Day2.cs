using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day2 : AocDayBase<int, int>
{
    protected override int Day => 2;
    
    public override int Puzzle1()
    {
        return Input
            .ReadLines()
            .Select(l => l.Split(' '))
            .Select(x => x.Select(int.Parse))
            .Where(IsValidLevel)
            .Count();
    }
    
    public override int Puzzle2()
    { 
        return Input
            .ReadLines()
            .Select(l => l.Split(' '))
            .Select(x => x.Select(int.Parse))
            .Count(line => Enumerable
                .Range(0, line.Count())
                .Any(i => IsValidLevel(line
                        .Index()
                        .Where(x => !x.Index.Equals(i))
                    .Select(x => x.Item)
                )));
    }
    
    private static bool IsValidLevel(IEnumerable<int> line)
    {
        var differences = line.Zip(line.Skip(1), (prev, curr) => curr - prev).ToArray();

        return differences.All(diff => Math.Abs(diff) > 0 && Math.Abs(diff) <= 3)
               && differences.DistinctBy(Math.Sign).Count() == 1;
    }

}
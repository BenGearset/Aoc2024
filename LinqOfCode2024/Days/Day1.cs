using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day1 : AocDayBase<int, int>
{
    protected override int Day => 1;

    public override int Puzzle1()
    {
        var lines = Input
            .ReadLines()
            .Select(l => l.Split("   "))
            .Select(l => (line1: int.Parse(l[0]), line2: int.Parse(l[0]))).ToArray();
        
        var list1 = lines.Select(x => x.line1).Order();
        var list2 =  lines.Select(x => x.line2).Order();
        
        return list1.Zip(list2, (x, y) => Math.Abs(x - y)).Sum();
    }

    public override int Puzzle2()
    {
        var lines = Input
            .ReadLines()
            .Select(l => l.Split("   "))
            .Select(l => (line1: int.Parse(l[0]), line2: int.Parse(l[0]))).ToArray();
        
        var list1 = lines.Select(x => x.line1).Order();
        var list2 = lines.Select(x => x.line2).Order();
        
        var countDict = list2
            .GroupBy(x => x)
            .ToDictionary(x => x.Key, x => x.Count());
        
        return list1.Select(x => x * countDict.GetValueOrDefault(x)).Sum();
    }
}
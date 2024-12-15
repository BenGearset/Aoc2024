using LinqOfCode2024.Days;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024;

public class AocProgram
{
    private static readonly IAocDay s_Day = new Day15();
    
    public static void Main()
    {
        try
        {
            Console.WriteLine($"Puzzle 1 result: {s_Day.Puzzle1()}");
        }
        catch (NotImplementedException)
        {
            // Puzzle not implemented yet
        }
        
        try
        {
            Console.WriteLine($"Puzzle 2 result: {s_Day.Puzzle2()}");
        }
        catch (NotImplementedException)
        {
            // Puzzle not implemented yet
        }
    }
}
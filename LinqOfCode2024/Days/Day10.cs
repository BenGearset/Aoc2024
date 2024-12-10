using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day10 : AocDayBase<int, int>
{
    protected override int Day => 10;
    public override int Puzzle1()
    {
        var map = Input.ReadLines().Select(x => x.Select(y => y - '0').ToArray()).ToArray();
        var startingPos = new List<(int x, int y)>();
        foreach (var (y, row) in map.Index())
        {
            foreach (var (x, value) in row.Index())
            {
                if (value is 0)
                {
                    startingPos.Add((x, y));
                }
            }
        }

        return startingPos.Select(sp =>WalkTrail(map, sp, 0).Count).Sum();
    }

    public override int Puzzle2()
    {
        var map = Input.ReadLines().Select(x => x.Select(y => y - '0').ToArray()).ToArray();
        var startingPos = new List<(int x, int y)>();
        foreach (var (y, row) in map.Index())
        {
            foreach (var (x, value) in row.Index())
            {
                if (value is 0)
                {
                    startingPos.Add((x, y));
                }
            }
        }

        return startingPos.Select(sp => WalkTrailPart2(map, sp, 0)).Sum();
    }


    private ISet<(int x, int y)> WalkTrail(int[][] map, (int x, int y) currPos, int currHeight)
    {
        var result = new HashSet<(int x, int y)>();

        if (currHeight is 9)
        {
            return new HashSet<(int x, int y)> {currPos};
        }

        foreach (var direction in (Direction[])[Direction.North, Direction.South, Direction.East, Direction.West])
        {
            try
            {
                var (xMod, yMod) = GetDirectionModifier(direction);
                if (map[currPos.y + yMod][currPos.x + xMod] == currHeight + 1)
                {
                    result.UnionWith(WalkTrail(map, (currPos.x + xMod, currPos.y + yMod), currHeight + 1));
                }
            } catch (IndexOutOfRangeException) {}
        }

        return result;
    }
    
    private int WalkTrailPart2(int[][] map, (int x, int y) currPos, int currHeight)
    {
        var sum = 0;

        if (currHeight is 9)
        {
            return 1;
        }

        foreach (var direction in (Direction[])[Direction.North, Direction.South, Direction.East, Direction.West])
        {
            try
            {
                var (xMod, yMod) = GetDirectionModifier(direction);
                if (map[currPos.y + yMod][currPos.x + xMod] == currHeight + 1)
                {
                    sum += WalkTrailPart2(map, (currPos.x + xMod, currPos.y + yMod), currHeight + 1);
                }
            } catch (IndexOutOfRangeException) {}
        }

        return sum;
    }

    private static (int x, int y) GetDirectionModifier(Direction direction)
    {
        return direction switch
        {
            Direction.North => (0, -1),
            Direction.East => (1, 0),
            Direction.South => (0, 1),
            Direction.West => (-1, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    enum Direction
    {
        North,
        East,
        South,
        West,
    }
}

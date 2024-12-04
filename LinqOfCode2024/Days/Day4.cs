using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day4 : AocDayBase<int, int>
{
    protected override int Day => 4;
    public override int Puzzle1()
    {
        var grid = Input.ReadLines().Select(x => x.ToCharArray()).ToArray();
        
        List<(int x, int y)> startPositions = new();

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid.Length; j++)
            {
                if (grid[i][j] is 'X')
                {
                    startPositions.Add((i, j));
                }
            }
        }

        var count = 0;

        foreach (var direction in m_Directions)
        {
            var directionModifier = DirectionModifer(direction);
            foreach (var startPosition in startPositions)
            {
                var position = startPosition;

                foreach (var value in "MAS")
                {
                    position = (x: directionModifier.x + position.x, y: directionModifier.y + position.y);
                    if (position.x < 0 || position.y < 0 || grid.Length <= position.x || grid[position.x].Length <= position.y)
                    {
                        break;
                    }
                    if (grid[position.x][position.y] != value)
                    {
                        break;
                    }

                    if (value is 'S')
                    {
                        count++;
                    }
                }
            } 
        }
        return count;
    }

    private (int x, int y) DirectionModifer(Direction direction)
    {
        return direction switch
        {
            Direction.North => (-1, 0),
            Direction.NorthEast => (-1, 1),
            Direction.East => (0, 1),
            Direction.SouthEast => (1, 1),
            Direction.South => (1, 0),
            Direction.SouthWest => (1, -1),
            Direction.West => (0, -1),
            Direction.NorthWest => (-1, -1),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    public override int Puzzle2()
    {
        var grid = Input.ReadLines().Select(x => x.ToCharArray()).ToArray();
        
        List<(int x, int y)> startPositions = new();

        for (var i = 0; i < grid.Length; i++)
        {
            for (var j = 0; j < grid.Length; j++)
            {
                if (grid[i][j] is 'A')
                {
                    startPositions.Add((i, j));
                }
            }
        }

        var count = 0;

        foreach (var position in startPositions)
        {
            
            if (position.x - 1 < 0 || position.y - 1 < 0 || position.x + 1 >= grid.Length ||
                grid[position.x].Length <= position.y + 1)
            {
                continue;
            }

            if (((grid[position.x + 1][position.y + 1] == 'M' && grid[position.x - 1][position.y - 1] == 'S')
                || (grid[position.x + 1][position.y + 1] == 'S' && grid[position.x - 1][position.y - 1] == 'M'))
                && ((grid[position.x - 1][position.y + 1] == 'M' && grid[position.x + 1][position.y - 1] == 'S')
                || (grid[position.x - 1][position.y + 1] == 'S' && grid[position.x + 1][position.y - 1] == 'M')))
            {
                count++;
            }
            
        }
        return count;
    }


    private readonly IEnumerable<Direction> m_Directions =
    [
        Direction.North,
        Direction.NorthEast,
        Direction.East,
        Direction.SouthEast,
        Direction.South,
        Direction.SouthWest,
        Direction.West,
        Direction.NorthWest,
    ];
    
    enum Direction
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
    }
}
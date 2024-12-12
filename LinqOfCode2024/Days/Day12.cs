using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day12 : AocDayBase<long, long>
{
    protected override int Day => 12;
    
    public override long Puzzle1()
    {
        var garden = new Garden(Input.ReadLines().Select(x => x.ToCharArray().ToList()).ToList());

        return garden.CalculateCosts().totalPart1;
    }

    public override long Puzzle2()
    {
        var garden = new Garden(Input.ReadLines().Select(x => x.ToCharArray().ToList()).ToList());

        return garden.CalculateCosts().totalPart2;
    }
}

class Garden
{
    private readonly List<List<char>> m_Garden;

    public Garden(List<List<char>> garden)
    {
        m_Garden = garden;        
    }

    public (int totalPart1, int totalPart2) CalculateCosts()
    {
        var searchedRegions = m_Garden.Select(x => x.Select(_ => false).ToArray()).ToArray();

        var totalPart1 = 0;
        var totalPart2 = 0;

        while (FindRegionToSearch(searchedRegions) is { } startPosition)
        {
            var toSearch = new Queue<(int x, int y)>([startPosition]);
            var searched = new HashSet<(int x, int y)>([]);
            var perimiterPoints = new HashSet<(int x, int y, Direction direction)>([]);

            var plant = m_Garden[startPosition.y][startPosition.x];
            
            while (toSearch.Count != 0)
            {
                var position = toSearch.Dequeue();
                
                if (searched.Contains(position))
                {
                    continue;
                }

                searched.Add((position.x, position.y));
                
                searchedRegions[position.y][position.x] = true;
                
                foreach (var direction in (Direction[])
                         [Direction.North, Direction.South, Direction.East, Direction.West])
                {
                    var (xMod, yMod) = GetDirectionModifier(direction);
                    if (searched.Contains((position.x + xMod, position.y + yMod)))
                    {
                        continue;
                    }
                    
                    try
                    {
                        if (m_Garden[position.y + yMod][position.x + xMod] == plant)
                        {
                            toSearch.Enqueue((position.x + xMod, position.y + yMod));
                        }
                        else
                        {
                            perimiterPoints.Add((position.x + xMod, position.y + yMod, direction));
                        }
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        perimiterPoints.Add((position.x + xMod, position.y + yMod, direction));
                    }
                }

            }

            totalPart1 += perimiterPoints.Count * searched.Count;
            totalPart2 += CountSides(perimiterPoints) * searched.Count;

        }
        return (totalPart1, totalPart2);
    }

    private int CountSides(HashSet<(int x, int y, Direction direction)> fences)
    {
        var verticalFences = fences.Where(x => x.direction is Direction.North or Direction.South); 
        var horizontalFences = fences.Where(x => x.direction is Direction.East or Direction.West);

        var verticalCount = 0;
        foreach (var verticalGroup in horizontalFences.GroupBy(x => (x.x, x.direction)))
        {
            var ySet = verticalGroup.Select(x => x.y).ToHashSet();
            var orderedSet = ySet.Order();
            int? prev = null;
            foreach (var y in orderedSet)
            {
                if (prev is null)
                {
                    verticalCount++;
                }
                else if (y != prev + 1)
                {
                    verticalCount++;
                }

                prev = y;
            }
        }
        
        var horizontalCount = 0;

        foreach (var horizontalGroup in verticalFences.GroupBy(x => (x.y, x.direction)))
        {
            var xSet = horizontalGroup.Select(x => x.x).ToHashSet();
            var orderedSet = xSet.Order();
            int? prev = null;
            foreach (var x in orderedSet)
            {
                if (prev is null)
                {
                    horizontalCount++;
                }
                else if (x != prev + 1)
                {
                    horizontalCount++;
                }
                prev = x;
            }
        }


        return verticalCount + horizontalCount;
    }
    
    private (int x, int y)? FindRegionToSearch(bool[][] searchedRegions)
    {
        foreach (var (y, row) in searchedRegions.Index())
        {
            foreach (var (x, cell) in row.Index())
            {
                if (!cell)
                {
                    return (x, y);
                }
            }
        }

        return null;
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
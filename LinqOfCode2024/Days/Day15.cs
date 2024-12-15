using System.Text;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day15 : AocDayBase<int, int>
{
    protected override int Day => 15;
    
    public override int Puzzle1()
    {
        var inputs = Input.RawValue.Split(Environment.NewLine + Environment.NewLine);
        
        var warehouse = new Warehouse(inputs[0].Split(Environment.NewLine));

        var moves = inputs[1].ReplaceLineEndings("");
        warehouse.ProcessMoves(moves);

        return warehouse.SumBoxCoords();
    }

    public override int Puzzle2()
    {
        var inputs = Input.RawValue.Split(Environment.NewLine + Environment.NewLine);
        
        var warehouse = new Warehouse2(inputs[0].Split(Environment.NewLine));

        var moves = inputs[1].ReplaceLineEndings("");
        warehouse.ProcessMoves(moves);

        return warehouse.SumBoxCoords();
    }
}

class Warehouse2
{
    private readonly List<List<WarehouseCell>> m_Warehouse;
    private (int x, int y) m_Position;

    public Warehouse2(IEnumerable<IEnumerable<char>> input)
    {
        var warehouseContents = new List<List<WarehouseCell>>();
        foreach (var (y, row) in input.Index())
        {
            var rowContent = new List<WarehouseCell>();
            foreach (var (x, cell) in row.Index())
            {
                switch (cell)
                {
                    case '#':
                        rowContent.Add(WarehouseCell.Wall);
                        rowContent.Add(WarehouseCell.Wall);
                        break;
                    case '.':
                        rowContent.Add(WarehouseCell.Empty);
                        rowContent.Add(WarehouseCell.Empty);
                        break;
                    case 'O':
                        rowContent.Add(WarehouseCell.BoxLeft);
                        rowContent.Add(WarehouseCell.BoxRight);
                        break;
                    case '@':
                        rowContent.Add(WarehouseCell.Empty);
                        rowContent.Add(WarehouseCell.Empty);
                        m_Position = (2 * x, y);
                        break;
                }
            }
            warehouseContents.Add(rowContent);
        }

        m_Warehouse = warehouseContents;
    }

    public void ProcessMoves(IEnumerable<char> moves)
    {
        foreach (var move in moves)
        {
            var direction = move switch
            {
                '^' => Direction.North,
                '<' => Direction.West,
                '>' => Direction.East,
                'v' => Direction.South,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (CanMove(direction))
            {
                //Console.WriteLine(direction.ToString());
                Move(direction);
                //Console.WriteLine(ToString());
            }
            
        }

        Console.WriteLine(ToString());
    }

    private bool CanMove(Direction direction)
    {
        var directionModifier = GetDirectionModifier(direction);
        var currentPosition = m_Position;
        if (direction is Direction.East or Direction.West)
        {
            while (true)
            {
                currentPosition = (x: currentPosition.x + directionModifier.x,
                    y: currentPosition.y + directionModifier.y);

                switch (m_Warehouse[currentPosition.y][currentPosition.x])
                {
                    case WarehouseCell.Empty:
                        return true;
                    case WarehouseCell.Wall:
                        return false;
                    case WarehouseCell.BoxLeft:
                    case WarehouseCell.BoxRight:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        if (m_Warehouse[currentPosition.y + directionModifier.y][currentPosition.x] is WarehouseCell.Wall)
        {
            return false;
        }

        var touchingBoxes  = FindVerticalTouchingBoxes(m_Position, direction);
        if (direction is Direction.South)
        {
            foreach (var box in touchingBoxes)
            {
                if (m_Warehouse[box.y + 1][box.x1] == WarehouseCell.Wall)
                {
                    return false;
                }

                if (m_Warehouse[box.y + 1][box.x2] == WarehouseCell.Wall)
                {
                    return false;
                }
            }
        }
        else
        {
            foreach (var box in touchingBoxes)
            {
                if (m_Warehouse[box.y - 1][box.x1] == WarehouseCell.Wall)
                {
                    return false;
                }

                if (m_Warehouse[box.y - 1][box.x2] == WarehouseCell.Wall)
                {
                    return false;
                }
            }
        }

        return true;

    }

    private HashSet<(int x1, int x2, int y)> FindVerticalTouchingBoxes((int x, int y) position, Direction direction)
    {
        var result = new HashSet<(int x1, int x2, int y)>();
        var yMod = direction is Direction.North ? -1 : 1;
        try
        {
            if (m_Warehouse[position.y + yMod][position.x] is WarehouseCell.BoxLeft)
            {
                result.Add((position.x, position.x + 1, position.y + yMod));
            }
        }
        catch (IndexOutOfRangeException)
        {
        }

        try
        {
            if (m_Warehouse[position.y + yMod][position.x] is WarehouseCell.BoxRight)
            {
                result.Add((position.x - 1, position.x, position.y + yMod));
            }
        }
        catch (IndexOutOfRangeException)
        {
        }
        
        var childResults = result.SelectMany(x => FindVerticalTouchingBoxes((x.x1, x.y), direction).Union(FindVerticalTouchingBoxes((x.x2, x.y), direction)));
        
        return result.Union(childResults).ToHashSet();
    }

    private void Move(Direction direction)
    {
        var directionModifier = GetDirectionModifier(direction);
        var currentPosition = m_Position;
        m_Position = (x: currentPosition.x + directionModifier.x, y: currentPosition.y + directionModifier.y);
        var prev = WarehouseCell.Empty;
        if (direction is Direction.East or Direction.West)
        {
            while (true)
            {
                currentPosition = (x: currentPosition.x + directionModifier.x,
                    y: currentPosition.y + directionModifier.y);

                switch (m_Warehouse[currentPosition.y][currentPosition.x])
                {
                    case WarehouseCell.Empty:
                        m_Warehouse[currentPosition.y][currentPosition.x] = prev;
                        return;
                    case WarehouseCell.Wall:
                        throw new Exception("HitWall");
                    case WarehouseCell.BoxLeft:
                        m_Warehouse[currentPosition.y][currentPosition.x] = prev;
                        prev = WarehouseCell.BoxLeft;
                        break;
                    case WarehouseCell.BoxRight:
                        m_Warehouse[currentPosition.y][currentPosition.x] = prev;
                        prev = WarehouseCell.BoxRight;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        var touchingBoxes  = FindVerticalTouchingBoxes(currentPosition, direction);
        if (direction is Direction.South)
        {
            foreach (var box in touchingBoxes.OrderByDescending(x => x.y))
            {
                m_Warehouse[box.y + 1][box.x1] = WarehouseCell.BoxLeft;
                m_Warehouse[box.y + 1][box.x2] = WarehouseCell.BoxRight;
                m_Warehouse[box.y][box.x1] = WarehouseCell.Empty;
                m_Warehouse[box.y][box.x2] = WarehouseCell.Empty;
            }
        }
        else
        {
            foreach (var box in touchingBoxes.OrderBy(x => x.y))
            {
                m_Warehouse[box.y - 1][box.x1] = WarehouseCell.BoxLeft;
                m_Warehouse[box.y - 1][box.x2] = WarehouseCell.BoxRight;
                m_Warehouse[box.y][box.x1] = WarehouseCell.Empty;
                m_Warehouse[box.y][box.x2] = WarehouseCell.Empty;
            }
        }
    }


    public int SumBoxCoords()
    {
        var sum = 0;
        foreach (var (y, row) in m_Warehouse.Index())
        {
            foreach (var (x, cell) in row.Index())
            {
                if (cell is WarehouseCell.BoxLeft)
                {
                    sum += y * 100 + x;
                }
            }
        }

        return sum;
    }
    
    enum Direction
    {
        North,
        East,
        South,
        West,
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
    enum WarehouseCell
    {
        Empty,
        BoxLeft,
        BoxRight,
        Wall
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        foreach (var (y, row) in m_Warehouse.Index())
        {
            foreach (var (x, cell) in row.Index())
            {
                if (m_Position == (x, y))
                {
                    sb.Append('@');
                    continue;
                }
                
                switch (cell)
                {
                    case WarehouseCell.Empty:
                        sb.Append('.');
                        break;
                    case WarehouseCell.Wall:
                        sb.Append('#');
                        break;
                    case WarehouseCell.BoxLeft:
                        sb.Append('[');
                        break;
                    case WarehouseCell.BoxRight:
                        sb.Append(']');
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            sb.AppendLine();
        }

        sb.AppendLine();
        return sb.ToString();
    }

}

class Warehouse
{
    private readonly List<List<WarehouseCell>> m_Warehouse;
    private (int x, int y) m_Position;

    public Warehouse(IEnumerable<IEnumerable<char>> input)
    {
        var warehouseContents = new List<List<WarehouseCell>>();
        foreach (var (y, row) in input.Index())
        {
            var rowContent = new List<WarehouseCell>();
            foreach (var (x, cell) in row.Index())
            {
                switch (cell)
                {
                    case '#':
                        rowContent.Add(WarehouseCell.Wall);
                        break;
                    case '.':
                        rowContent.Add(WarehouseCell.Empty);
                        break;
                    case 'O':
                        rowContent.Add(WarehouseCell.Box);
                        break;
                    case '@':
                        rowContent.Add(WarehouseCell.Empty);
                        m_Position = (x, y);
                        break;
                }
            }
            warehouseContents.Add(rowContent);
        }

        m_Warehouse = warehouseContents;
    }

    public void ProcessMoves(IEnumerable<char> moves)
    {
        foreach (var move in moves)
        {
            var direction = move switch
            {
                '^' => Direction.North,
                '<' => Direction.West,
                '>' => Direction.East,
                'v' => Direction.South,
                _ => throw new ArgumentOutOfRangeException()
            };

            if (CanMove(direction))
            {
                Move(direction);
            }
            
        }
    }

    private bool CanMove(Direction direction)
    {
        var directionModifier = GetDirectionModifier(direction);
        var currentPosition = m_Position;
        while (true)
        {
            currentPosition = (x: currentPosition.x + directionModifier.x, y: currentPosition.y + directionModifier.y);

            switch (m_Warehouse[currentPosition.y][currentPosition.x])
            {
                case WarehouseCell.Empty:
                    return true;
                case WarehouseCell.Wall:
                    return false;
                case WarehouseCell.Box:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    private void Move(Direction direction)
    {
        var directionModifier = GetDirectionModifier(direction);
        var currentPosition = m_Position;
        m_Position = (x: currentPosition.x + directionModifier.x, y: currentPosition.y + directionModifier.y);
        
        var movingBox = false;
        while (true)
        {
            var newPosition = (x: currentPosition.x + directionModifier.x, y: currentPosition.y + directionModifier.y);

            switch (m_Warehouse[newPosition.y][newPosition.x])
            {
                case WarehouseCell.Empty:
                    if (movingBox)
                    {
                        m_Warehouse[newPosition.y][newPosition.x] = WarehouseCell.Box;
                    }
                    return;
                case WarehouseCell.Wall:
                    throw new Exception("Hit wall unexpectedly");
                case WarehouseCell.Box:
                    if (!movingBox)
                    {
                        movingBox = true;
                        m_Warehouse[newPosition.y][newPosition.x] = WarehouseCell.Empty;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            currentPosition = newPosition;
        }
    }


    public int SumBoxCoords()
    {
        var sum = 0;
        foreach (var (y, row) in m_Warehouse.Index())
        {
            foreach (var (x, cell) in row.Index())
            {
                if (cell is WarehouseCell.Box)
                {
                    sum += y * 100 + x;
                }
            }
        }

        return sum;
    }
    
    enum Direction
    {
        North,
        East,
        South,
        West,
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
    enum WarehouseCell
    {
        Empty,
        Box,
        Wall
    }

}
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day6 : AocDayBase<int, int>
{
    protected override int Day => 6;
    public override int Puzzle1()
    {
        var maze = new Maze(Input.ReadLines().Select(x => x.ToCharArray()).ToArray());
        
        try
        {
            while (true)
            {
                if (!maze.IsObstructed())
                {
                    maze.MoveForward();
                }
                else
                {
                    maze.TurnRight();
                }
            }
        }
        catch (OutOfMazeException)
        {
            
        }

        return maze.DistinctVisited();
    }

    public override int Puzzle2()
    {
        var maxY = Input.ReadLines().Select(x => x.ToCharArray()).Count();
        var maxX = Input.ReadLines().Max(x => x.ToCharArray().Length);

        var loops = 0;
        for (var i = 0; i < maxX; i++)
        {
            for (var j = 0; j < maxY; j++)
            {
                var maze = new Maze(Input.ReadLines().Select(x => x.ToCharArray()).ToArray(), (i, j));

                try
                {
                    while (true)
                    {
                        if (!maze.IsObstructed())
                        {
                            maze.MoveForward();
                        }
                        else
                        {
                            maze.TurnRight();
                        }
                    }
                }
                catch (OutOfMazeException)
                {

                }
                catch (LoopException)
                {
                    loops++;
                }
            }
        }
        return loops;
    }
}

class Maze
{
    private readonly List<List<Content>> m_Maze;
    private (int x, int y) m_Position;
    private Direction m_Direction = Direction.North;
    private readonly ISet<(int x, int y)> m_Visited = new HashSet<(int x, int y)>();
    private readonly ISet<(int x, int y, Direction direction)> m_VisitedWithDirection = new HashSet<(int x, int y, Direction direction)>();

    public Maze(IEnumerable<IEnumerable<char>> maze, (int x, int y)? extraObstacle = null)
    {
        var mazeContents = new List<List<Content>>();
        foreach (var (y, row) in maze.Index())
        {
            var rowContent = new List<Content>();
            foreach (var (x, cell) in row.Index())
            {
                switch (cell)
                {
                    case '#':
                        rowContent.Add(Content.Obstruction);
                        break;
                    case '.':
                        rowContent.Add(Content.Clear);
                        break;
                    case '^':
                        rowContent.Add(Content.Clear);
                        m_Position = (x, y);
                        break;
                }
            }
            mazeContents.Add(rowContent);
        }
        
        m_Visited.Add(m_Position);
        m_Maze = mazeContents;
        
        if (extraObstacle is not null)
        {
            m_Maze[extraObstacle.Value.y][extraObstacle.Value.x] = Content.Obstruction;
        }

    }

    
    public void MoveForward()
    {
        var (modifierX, modifierY) = GetDirectionModifier(m_Direction);
        var inFront = (x: modifierX + m_Position.x, y: modifierY + m_Position.y);
        
        if (m_VisitedWithDirection.Contains((inFront.x, inFront.y, m_Direction)))
        {
            throw new LoopException();
        }
        
        try
        {
            m_Position = inFront;
            _ = m_Maze[m_Position.y][m_Position.x];
            m_Visited.Add((inFront.x, inFront.y));
            m_VisitedWithDirection.Add((inFront.x, inFront.y, m_Direction));
        }
        catch
        {
            throw new OutOfMazeException();
        }
    }

    public bool IsObstructed()
    {
        var (modifierX, modifierY) = GetDirectionModifier(m_Direction);
        var inFront = (x: modifierX + m_Position.x, y: modifierY + m_Position.y);

        try
        {
            return m_Maze[inFront.y][inFront.x] is Content.Obstruction;
        }
        catch
        {
            return false;
        }
    }
    
    public void TurnRight()
    {
        m_Direction = GetRight(m_Direction);
    }

    private Direction GetRight(Direction direction)
    {
        return direction switch
        {
            Direction.North => Direction.East,
            Direction.East => Direction.South,
            Direction.South => Direction.West,
            Direction.West => Direction.North,
            _ => throw new ArgumentOutOfRangeException()
        };
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
    
    public int DistinctVisited() => m_Visited.Count;
}

enum Content
{
    Clear,
    Obstruction
}

enum Direction
{
    North,
    East,
    South,
    West,
}

public class OutOfMazeException : Exception;

public class LoopException : Exception;
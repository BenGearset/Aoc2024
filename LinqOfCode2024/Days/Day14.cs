using System.Text;
using System.Text.RegularExpressions;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day14 : AocDayBase<int, int>
{
    protected override int Day => 14;

    private const int c_SizeX = 101;
    private const int c_SizeY = 103;

    public override int Puzzle1()
    {
        var matches = Regex.Matches(Input.RawValue, "p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)").Select(m =>
            new Robot(new Position(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)),
                new Vector(int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value))));
        
        var bathroom = new Bathroom(c_SizeX, c_SizeY, matches);
        
        bathroom.SimulateTime(100);
        
        Console.Write(bathroom.ToString());

        return bathroom.CalculateSafetyFactor();
    }

    public override int Puzzle2()
    {
        var matches = Regex.Matches(Input.RawValue, "p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)").Select(m =>
            new Robot(new Position(int.Parse(m.Groups[1].Value), int.Parse(m.Groups[2].Value)),
                new Vector(int.Parse(m.Groups[3].Value), int.Parse(m.Groups[4].Value))));
        
        var bathroom = new Bathroom(c_SizeX, c_SizeY, matches);

        return bathroom.TimeTillTree();
    }

}


class Bathroom
{
    private readonly int m_SizeX;
    private readonly int m_SizeY;
    private IEnumerable<Robot> m_Robots;

    public Bathroom(int sizeX, int sizeY, IEnumerable<Robot> robots)
    {
        m_SizeX = sizeX;
        m_SizeY = sizeY;
        m_Robots = robots;
    }

    public void SimulateTime(int seconds)
    {
        m_Robots = m_Robots
            .Select(robot =>
            {
                var newX = (robot.Position.X + seconds * robot.Velocity.X) % m_SizeX;
                var newY = (robot.Position.Y + seconds * robot.Velocity.Y) % m_SizeY;
                if (newX < 0)
                {
                    newX = m_SizeX + newX;
                }

                if (newY < 0)
                {
                    newY = m_SizeY + newY;
                }
                
                return robot with
                {
                    Position = new Position(newX, newY)
                };
            }).ToList();
    }

    public int TimeTillTree()
    {
        var time = 0;

        var max = 0;

        while (true)
        {
            time++;
            SimulateTime(1);

            var count = m_Robots.Count(rb => m_Robots.Any(x =>
                Math.Abs(rb.Position.X - x.Position.X) is 1 && Math.Abs(rb.Position.Y - x.Position.Y) is 1));
            
            if (count == m_Robots.Count())
            {
                return time;
            }
            
            var newMax = Math.Max(max, count);
            if (newMax > max)
            {
                if (newMax > 200)
                {
                    Console.Write(ToString());
                    return time;
                }
                
                max = newMax;
                Console.WriteLine(newMax);
            }
        }
    }

    public override string ToString()
    {
        var sb = new StringBuilder();
        for (var y = 0; y < m_SizeY; y++)
        {
            for (var x = 0; x < m_SizeX; x++)
            {
                sb.Append(m_Robots.Count(r => r.Position.X == x && r.Position.Y == y).ToString().Replace('0', '.'));
            }

            sb.AppendLine();
        }

        sb.AppendLine();
        return sb.ToString();
    }

    public int CalculateSafetyFactor()
    {
        var maxX1 = m_SizeX / 2;
        var minX2 = m_SizeX - maxX1;
        
        var maxY1 = m_SizeY / 2;
        var minY2 = m_SizeY - maxY1;

        var quadrants = Enum.GetValues<Quadrant>().ToDictionary(x => x, _ => 0);
        
        
        foreach (var robot in m_Robots)
        {
            if (robot.Position.X < maxX1 && robot.Position.Y < maxY1)
            {
                quadrants[Quadrant.TopLeft]++;
            }
            else if (robot.Position.X >= minX2 && robot.Position.Y < maxY1)
            {
                quadrants[Quadrant.TopRight]++;
            }
            else if (robot.Position.X < maxX1 && robot.Position.Y >= minY2)
            {
                quadrants[Quadrant.BottomLeft]++;
            }
            else if (robot.Position.X >= minX2 && robot.Position.Y >= minY2)
            {
                quadrants[Quadrant.BottomRight]++;
            }
        }

        return quadrants[Quadrant.TopLeft] * quadrants[Quadrant.TopRight] * quadrants[Quadrant.BottomLeft] *
               quadrants[Quadrant.BottomRight];

    }

    enum Quadrant
    {
        TopLeft,
        BottomLeft,
        TopRight,
        BottomRight
    }
}

record Vector(int X, int Y);
record Position(int X, int Y);

record Robot(Position Position, Vector Velocity);

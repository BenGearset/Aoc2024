using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day8 : AocDayBase<int, int>
{
    protected override int Day => 8;
    
    public override int Puzzle1()
    {
        var map = new AntennaMap(Input.ReadLines().Select(x => x.ToCharArray()), part1: true);
        return map.GetAntiNodeLocations().Count;
    }

    public override int Puzzle2()
    {
        var map = new AntennaMap(Input.ReadLines().Select(x => x.ToCharArray()), part1: false);
        return map.GetAntiNodeLocations().Count;
    }
}

class AntennaMap
{
    private readonly bool m_Part1;
    private readonly char[][] m_Map;
    private readonly List<(int x, int y, char antenna)> m_AntennaPositions = new();
    
    public AntennaMap(IEnumerable<IEnumerable<char>> map, bool part1)
    {
        m_Part1 = part1;
        m_Map = map.Select(x => x.ToArray()).ToArray();
        foreach (var (y, row) in m_Map.Index())
        {
            foreach (var (x, cell) in row.Index())
            {
                if (cell is not '.')
                {
                    m_AntennaPositions.Add((x, y, cell));
                }
            }
        }
    }

    public ISet<(int x, int y)> GetAntiNodeLocations()
    {
        var maxX = m_Map[0].Length - 1;
        var maxY = m_Map.Length - 1;
        
        HashSet<(int x, int y)> locations = new();
        foreach (var antennaPosition in m_AntennaPositions)
        {
            var otherPositionsOfSameFrequency = m_AntennaPositions.Where(x => x.antenna == antennaPosition.antenna && x != antennaPosition);
            foreach (var otherPosition in otherPositionsOfSameFrequency)
            {
                var xDiff = antennaPosition.x - otherPosition.x;
                var yDiff = antennaPosition.y - otherPosition.y;

                var modifer = m_Part1 ? 1 : 0;
                while (true)
                {
                    var pingPoint = (x: antennaPosition.x + modifer * xDiff, y: antennaPosition.y + modifer * yDiff);

                    modifer++;
                    if (pingPoint.x >= 0 && pingPoint.x <= maxX && pingPoint.y >= 0 && pingPoint.y <= maxY)
                    {
                        locations.Add((pingPoint.x, pingPoint.y));
                    }
                    else
                    {
                        break;
                    }

                    if (m_Part1)
                    {
                        break;
                    }
                    
                }
            }
        }
        return locations;
    } 
}

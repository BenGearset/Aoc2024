using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day11 : AocDayBase<long, long>
{
    protected override int Day => 11;
    public override long Puzzle1()
    {
        var stoneCollection = new StoneCollection(Input.RawValue.Split(' ').Select(long.Parse));

        stoneCollection.BlinkMultiple(25);

        return stoneCollection.Count;
    }

    public override long Puzzle2()
    {
        var stoneCollection = new StoneCollection(Input.RawValue.Split(' ').Select(long.Parse));

        stoneCollection.BlinkMultiple(75);

        return stoneCollection.Count;
    }
}


class StoneCollection
{
    private Dictionary<long, long> m_StoneCounts;

    public StoneCollection(IEnumerable<long> stones)
    {
        m_StoneCounts = stones.GroupBy(x => x).ToDictionary(stone => stone.Key, stone => (long) stone.Count());
    }

    public void BlinkMultiple(int times)
    {
        foreach (var _ in Enumerable.Range(0, times))
        {
            Blink();
        }
    }

    private void Blink()
    {
        var prevCounts = m_StoneCounts.ToDictionary();
        m_StoneCounts = new Dictionary<long, long>();
        
        foreach (var stone in prevCounts)
        {
            if (stone.Key == 0)
            {
                m_StoneCounts.AddOrIncrement(1, stone.Value);
            } 
            else if (stone.Key.ToString().Length % 2 == 0)
            {
                var split = stone.Key.ToString();
                var firstHalf = long.Parse(split.Take(split.Length/2).ToArray());
                var otherHalf = long.Parse(split.Skip(split.Length/2).ToArray());
                
                m_StoneCounts.AddOrIncrement(firstHalf, stone.Value);
                m_StoneCounts.AddOrIncrement(otherHalf, stone.Value);
            }
            else
            {
                m_StoneCounts.AddOrIncrement(stone.Key * 2024, stone.Value);
            }
        }

    }

    public long Count => m_StoneCounts.Sum(x => x.Value);
}

public static class DictionaryExtensions
{
    public static void AddOrIncrement<K>(this IDictionary<K, long> dict, K key, long value)
    {
        if (!dict.TryAdd(key, value))
        {
            dict[key] += value;
        }
    }
} 

using System.Collections;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day11 : AocDayBase<int, int>
{
    protected override int Day => 11;
    public override int Puzzle1()
    {
        var stoneCollection = new StoneCollection(Input.RawValue.Split(' ').Select(long.Parse));

        stoneCollection.BlinkMultiple(75);

        return stoneCollection.Count();
    }

    public override int Puzzle2()
    {
        throw new NotImplementedException();
    }
}


class StoneCollection : IEnumerable<long>
{
    private readonly List<long> m_Stones;

    public StoneCollection(IEnumerable<long> stones)
    {
        m_Stones = stones.ToList();
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
        var pointer = 0;

        while (pointer < m_Stones.Count)
        {
            var stone = m_Stones[pointer];
            if (stone == 0)
            {
                m_Stones[pointer] = 1;
            } else if (stone.ToString().Length % 2 == 0)
            {
                var split = stone.ToString();
                var firstHalf = long.Parse(split.Take(split.Length/2).ToArray());
                var otherHalf = long.Parse(split.Skip(split.Length/2).ToArray());

                m_Stones[pointer] = otherHalf;
                m_Stones.Insert(pointer, firstHalf);
                pointer++;
            }
            else
            {
                m_Stones[pointer] = stone * 2024;
            }

            pointer++;
        }

    }

    public IEnumerator<long> GetEnumerator() => m_Stones.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
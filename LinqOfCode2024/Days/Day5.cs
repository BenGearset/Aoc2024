using System.Text.RegularExpressions;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day5 : AocDayBase<int, int>
{
    protected override int Day => 5;
    
    public override int Puzzle1()
    {
        var lines = Input.ReadLines().ToList();
        var splitPos = lines.IndexOf("");
        var orderingRules = lines.Take(splitPos).Select(line =>
        {
            var parts = line.Split("|").ToArray();
            return (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
        }).GroupBy(x => x.x).ToDictionary(x => x.Key, x => x.Select(y => y.y));
        var updates = lines.Skip(splitPos + 1).Select(line => line.Split(',').Select(int.Parse).ToList());

        var middleSum = 0;

        foreach (var update in updates)
        {
            var cool = true;
            foreach (var page in update)
            {
                var rules = orderingRules.GetValueOrDefault(page)?.Where(x => update.Contains(x)) ?? [];
                var pageIndex = update.IndexOf(page);
                if (!rules.All(x => update.IndexOf(x) > pageIndex))
                {
                    cool = false;
                    break;
                }
            }

            if (cool)
            {
                middleSum += update[update.Count / 2];
            }
        }

        return middleSum;
    }

    public override int Puzzle2()
    {
        var lines = Input.ReadLines().ToList();
        var splitPos = lines.IndexOf("");
        var orderingRules = lines.Take(splitPos).Select(line =>
        {
            var parts = line.Split("|").ToArray();
            return (x: int.Parse(parts[0]), y: int.Parse(parts[1]));
        }).GroupBy(x => x.x).ToDictionary(x => x.Key, x => x.Select(y => y.y));
        var updates = lines.Skip(splitPos + 1).Select(line => line.Split(',').Select(int.Parse).ToList());
        List<List<int>> incorrectUpdates = new List<List<int>>();
        foreach (var update in updates)
        {
            var cool = true;
            foreach (var page in update)
            {
                var rules = orderingRules.GetValueOrDefault(page)?.Where(x => update.Contains(x)) ?? [];
                var pageIndex = update.IndexOf(page);
                if (!rules.All(x => update.IndexOf(x) > pageIndex))
                {
                    cool = false;
                    break;
                }
            }

            if (!cool)
            {
                incorrectUpdates.Add(update);
            }
        }

        List<List<int>> correctUpdates = new List<List<int>>();
        foreach (var incorrectUpdate in incorrectUpdates)
        {
            var correctOrder = new List<int>();
            var first = incorrectUpdate.Where(page => !orderingRules.GetValueOrDefault(page)?.Where(y => incorrectUpdate.Contains(y)).Any() ?? true);

            correctOrder.AddRange(first);

            var sortedUpdate = new List<int>(incorrectUpdate);

            while (true)
            {
                var updateCount = 0;

                var inprogressUpdate = new List<int>(sortedUpdate);
                for (var i = 0; i < inprogressUpdate.Count; i++)
                {
                    if (inprogressUpdate.Take(i).Any(x =>
                            (orderingRules.GetValueOrDefault(inprogressUpdate[i]) ?? []).Contains(x)))
                    {
                        (inprogressUpdate[i], inprogressUpdate[i - 1]) = (inprogressUpdate[i - 1], inprogressUpdate[i]);
                        updateCount++;
                    }
                }
                
                sortedUpdate = inprogressUpdate;

                if (updateCount == 0)
                {
                    correctUpdates.Add(sortedUpdate);
                    break;
                }
            }
            
        }
        return correctUpdates.Sum(x => x[x.Count/2]);
    }
}
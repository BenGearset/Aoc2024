using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day9 : AocDayBase<long, long>
{
    protected override int Day => 9;
    public override long Puzzle1()
    {
        var line = Input.RawValue.Select(c => c - '0').Chunk(2).Index()
            .SelectMany<(int Index, int[] Item), ISomething>(x =>
                    Enumerable.Range(0, x.Item[0]).Select(_ => (ISomething) new Data(x.Index)).Concat(Enumerable.Range(0, x.Item.ElementAtOrDefault(1)).Select(_ => (ISomething) new FreeSpace()))).ToArray();
            
        
        var freeSpaceIndexes = new Queue<int>(line.Index().Where(x => x.Item is not Data).Select(x => x.Index));
        
        while (true)
        {
            var (index, item) = line.Index().Reverse().First(x => x.Item is Data);
            var replaceIndex = freeSpaceIndexes.Dequeue();
            if (index < replaceIndex)
            {
                break;
            }
            line[replaceIndex] = item;
            freeSpaceIndexes.Enqueue(index);
            line[index] = new FreeSpace();
        }

        return line.Index().Where(x => x.Item is Data).Sum(c => long.Parse(((Data)c.Item).Id.ToString()) * c.Index);
    }

    public override long Puzzle2()
    {
        var line = Input.RawValue.Select(c => c - '0').Chunk(2).Index()
            .SelectMany<(int Index, int[] Item), ISomething>(x => new ISomething[] {new File(x.Index, x.Item[0])}.Concat(Enumerable.Range(0, x.Item.ElementAtOrDefault(1)).Select(_ => (ISomething) new FreeSpace()))).ToList();
        
        foreach (var item in line.AsEnumerable().Reverse().OfType<File>().ToArray())
        {
            var freeSpaceBlocks = FindFreeSpaceBlocks(line);

            var firstFitBox = freeSpaceBlocks.FirstOrDefault(x => x.Length >= item.Size);
            var index = line.IndexOf(item);
            
            if (index < firstFitBox.startIndex || firstFitBox == default)
            {
                continue;
            }
            
            line.RemoveAt(index);

            line.RemoveRange(firstFitBox.startIndex, item.Size);
            
            line.Insert(firstFitBox.startIndex, item);

            if (index - item.Size + 1 < line.Count)
            {
                line.InsertRange(index - item.Size + 1, Enumerable.Range(0, item.Size).Select(_ => (ISomething) new FreeSpace()));
            }
            else
            {
                line.AddRange(Enumerable.Range(0, item.Size).Select(_ => (ISomething) new FreeSpace()));
            }
        }

        var flatLine = line.SelectMany(x =>
        {
            return x switch
            {
                File file => Enumerable.Range(0, file.Size).Select(_ => new Data(file.Id)).Cast<ISomething>(),
                FreeSpace freeSpace => new[] {freeSpace}.Cast<ISomething>().AsEnumerable(),
                _ => throw new ArgumentOutOfRangeException(nameof(x))
            };
        });
        
        return flatLine.Index().Where(x => x.Item is Data).Sum(c => long.Parse(((Data)c.Item).Id.ToString()) * c.Index);
    }

    private static IEnumerable<(int startIndex, int Length)> FindFreeSpaceBlocks(IEnumerable<ISomething> blocks)
    {
        var blocksArray = blocks.ToArray();
        int? currentStartIndex = null;
        var currentLength = 1;
        foreach (var block in blocksArray.Index())
        {
            if (block.Item is FreeSpace)
            {
                if (currentStartIndex is null)
                {
                    currentStartIndex = block.Index;
                }
                else
                {
                    currentLength++;
                }
            } else if (currentStartIndex is not null)
            {
                yield return (currentStartIndex.Value, currentLength);
                currentStartIndex = null;
                currentLength = 1;
            }
        }
    }
    
    interface ISomething;
    
    record struct Data(int Id) : ISomething;

    record File(int Id, int Size) : ISomething;

    record struct FreeSpace : ISomething;

}
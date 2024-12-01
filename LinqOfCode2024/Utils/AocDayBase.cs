namespace LinqOfCode2024.Utils;

public interface IAocDay
{
    public object Puzzle1();
    public object Puzzle2();
}


public interface IAocDay<out T1, out T2> : IAocDay
{
    public new T1 Puzzle1();
    public new T2 Puzzle2();
}

public abstract class AocDayBase<T1, T2> : IAocDay<T1, T2>
{
    protected abstract int Day { get; }

    protected AocInput Input => LoadInput();

    private AocInput LoadInput()
    {
        return new AocInput(File.ReadAllText($"../../../Inputs/{Day}.txt"));
    }

    public abstract T1 Puzzle1();
    public abstract T2 Puzzle2();
    
    object IAocDay.Puzzle2() => Puzzle2() ?? throw new InvalidOperationException();

    object IAocDay.Puzzle1() => Puzzle1() ?? throw new InvalidOperationException();
}
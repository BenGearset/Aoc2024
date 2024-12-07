using System.Text.RegularExpressions;
using LinqOfCode2024.Utils;

namespace LinqOfCode2024.Days;

public class Day7: AocDayBase<long, long>
{
    protected override int Day => 7;
    public override long Puzzle1()
    {
        var calibrations = Regex.Matches(Input.RawValue, "(\\d+):(?: (\\d+))*")
            .Select(match => new CalibrationPoint(long.Parse(match.Groups[1].Value),
                match.Groups[2].Captures.Select(capture => int.Parse(capture.Value))));

        long total = 0;
        foreach (var calibration in calibrations)
        {
            foreach (var operations in GeneratePermutations(m_Operations1, calibration.Parts.Count() - 1))
            {
                var opStack = new Stack<Operation>(operations);
                long sum = calibration.Parts.First();
                foreach (var part in calibration.Parts.Skip(1))
                {
                    var operation = opStack.Pop();
                    sum = operation switch
                    {
                        Operation.Add => sum + part,
                        Operation.Multiply => sum * part,
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                if (sum == calibration.Total)
                {
                    total += calibration.Total;
                    break;
                }
            }
        }


        return total;
    }

    public override long Puzzle2()
    {
        var calibrations = Regex.Matches(Input.RawValue, "(\\d+):(?: (\\d+))*")
            .Select(match => new CalibrationPoint(long.Parse(match.Groups[1].Value),
                match.Groups[2].Captures.Select(capture => int.Parse(capture.Value))));

        long total = 0;
        foreach (var calibration in calibrations)
        {
            foreach (var operations in GeneratePermutations(m_Operations2, calibration.Parts.Count() - 1))
            {
                var opStack = new Stack<Operation>(operations);
                long sum = calibration.Parts.First();
                foreach (var part in calibration.Parts.Skip(1))
                {
                    var operation = opStack.Pop();
                    sum = operation switch
                    {
                        Operation.Add => sum + part,
                        Operation.Multiply => sum * part,
                        Operation.Combine => long.Parse(sum.ToString() + part), 
                        _ => throw new ArgumentOutOfRangeException()
                    };
                }
                if (sum == calibration.Total)
                {
                    total += calibration.Total;
                    break;
                }
            }
        }


        return total;
    }

    private IEnumerable<IEnumerable<Operation>> GeneratePermutations(IEnumerable<Operation> inputs, int length)
    {
        if (length == 1)
        {
            return inputs.Select(item => new[] { item });
        }

        return inputs.SelectMany(item =>
            GeneratePermutations(inputs, length - 1)
                .Select(subPermutation => new[] { item }.Concat(subPermutation)));
    }

    private readonly Operation[] m_Operations1 = [Operation.Add, Operation.Multiply];
    private readonly Operation[] m_Operations2 = [Operation.Add, Operation.Multiply, Operation.Combine];

}

public record CalibrationPoint(long Total, IEnumerable<int> Parts);

enum Operation
{
    Add,
    Multiply,
    Combine
}
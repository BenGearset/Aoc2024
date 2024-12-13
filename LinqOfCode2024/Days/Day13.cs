using System.Text.RegularExpressions;
using LinqOfCode2024.Utils;
using MathNet.Numerics.LinearAlgebra;

namespace LinqOfCode2024.Days;

public class Day13 : AocDayBase<int, long>
{
    protected override int Day => 13;
    
    public override int Puzzle1()
    {
        var splits = Input.RawValue.Split(Environment.NewLine+ Environment.NewLine)
            .Select(x =>
            {
                var split = x.Split(Environment.NewLine);
                var buttonA = Regex.Match(split[0], @"(\d+), Y\+(\d+)").Groups.Values.Skip(1).Select(y => int.Parse(y.Value)).ToArray();
                var buttonB = Regex.Match(split[1], @"(\d+), Y\+(\d+)").Groups.Values.Skip(1).Select(y => int.Parse(y.Value)).ToArray();
                var prize = Regex.Match(split[2], @"(\d+), Y=(\d+)").Groups.Values.Skip(1).Select(y => long.Parse(y.Value)).ToArray();
                
                return new ClawMachine(new Coords(buttonA[0], buttonA[1]), new Coords(buttonB[0], buttonB[1]), new Coords(prize[0] , prize[1]));
            }).ToArray();

        double totalTokens = 0;
        foreach (var machine in splits)
        {
            var a = Matrix<double>.Build.DenseOfArray(new double[,] {
                { machine.ButtonA.X,  machine.ButtonB.X},
                { machine.ButtonA.Y,  machine.ButtonB.Y},
            });
            var det = a.Determinant();
            if (det == 0)
            {
                continue;
            }
            var b = Vector<double>.Build.Dense([machine.Prize.X, machine.Prize.Y]);

            var inverse = a.Inverse();
            
            var sol = inverse * b;
            var x = sol[0];
            var y = sol[1];

            if (IsInteger(x) && IsInteger(y))
            {
                totalTokens += 3 * x + y;
            }
            
        }
        
        return (int) Math.Round(totalTokens);
    }

    bool IsInteger(double number, double epsilon = 1e-3)
    {
        double rounded = Math.Round(number);
        return Math.Abs(number - rounded) < epsilon;
    }
    public override long Puzzle2()
    {
        var splits = Input.RawValue.Split(Environment.NewLine+ Environment.NewLine)
            .Select(x =>
            {
                var split = x.Split(Environment.NewLine);
                var buttonA = Regex.Match(split[0], @"(\d+), Y\+(\d+)").Groups.Values.Skip(1).Select(y => int.Parse(y.Value)).ToArray();
                var buttonB = Regex.Match(split[1], @"(\d+), Y\+(\d+)").Groups.Values.Skip(1).Select(y => int.Parse(y.Value)).ToArray();
                var prize = Regex.Match(split[2], @"(\d+), Y=(\d+)").Groups.Values.Skip(1).Select(y => long.Parse(y.Value)).ToArray();
                
                return new ClawMachine(new Coords(buttonA[0], buttonA[1]), new Coords(buttonB[0], buttonB[1]), new Coords(prize[0] + 10000000000000, prize[1] + 10000000000000));
            }).ToArray();

        double totalTokens = 0;
        foreach (var machine in splits)
        {
            var a = Matrix<double>.Build.DenseOfArray(new double[,] {
                { machine.ButtonA.X,  machine.ButtonB.X},
                { machine.ButtonA.Y,  machine.ButtonB.Y},
            });
            var det = a.Determinant();
            if (det == 0)
            {
                continue;
            }
            var b = Vector<double>.Build.Dense([machine.Prize.X, machine.Prize.Y]);

            var inverse = a.Inverse();
            
            var sol = inverse * b;
            var x = sol[0];
            var y = sol[1];

            if (IsInteger(x) && IsInteger(y))
            {
                totalTokens += 3 * x + y;
            }
            
        }
        
        return (long) Math.Round(totalTokens);
    }

    record ClawMachine(Coords ButtonA, Coords ButtonB, Coords Prize);
    
    private record Coords(long X, long Y);
}

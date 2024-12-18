using System.ComponentModel;
using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec13(ITestOutputHelper output)
{
    private static readonly Regex MatchButtonA = new Regex("Button A: X\\+(\\d+), Y\\+(\\d+)");
    private static readonly Regex MatchButtonB = new Regex("Button B: X\\+(\\d+), Y\\+(\\d+)");
    private static readonly Regex MatchPrize = new Regex("Prize: X=(\\d+), Y=(\\d+)");

    [Fact]
    public void Calculate1()
    {
        var machines = GetData();

        var total = 0;
        foreach (var machine in machines)
        {
            var resultsA = Enumerable.Range(1, 100).Select(x => new { Count = x, X = machine.A.X * x, Y = machine.A.Y * x });
            var resultsB = Enumerable.Range(1, 100).Select(x => new { Count = x, X = machine.B.X * x, Y = machine.B.Y * x });

            var results = new List<Result>();
            foreach (var resA in resultsA)
            {
                foreach (var resB in resultsB)
                {
                    if (resA.X + resB.X == machine.Prize.X && resA.Y + resB.Y == machine.Prize.Y)
                    {
                        results.Add(new Result(resA.Count, resB.Count));
                    }
                }
            }

            if (results.Count > 0)
            {
                var minCost = results.Min(x => x.B + (x.A * 3));
                total += minCost;
            }
        }

        output.WriteLine($"{total}");
    }

    //private Result[] Find(Machine machine, int x, int y, int pressA, int pressB)
    //{
    //    if (machine.Prize.X == x && machine.Prize.Y == y)
    //    {
    //        return [new Result(true, pressA, pressB)];
    //    }

    //    if (x > machine.Prize.X || y > machine.Prize.Y)
    //    {
    //        return [];
    //    }

    //    return Find(machine, x + machine.A.X, y + machine.A.Y, pressA + 1, pressB)
    //        .Concat(Find(machine, x + machine.B.X, y + machine.B.Y, pressA, pressB + 1))
    //        .ToArray();
    //}

    private Machine[] GetData()
    {
        var machines = new List<Machine>();
        var lines = File.ReadAllLines("./input/Dec13.txt");
        for (int i = 0; i < lines.Length; i++)
        {
            var buttonA = MatchButtonA.Match(lines[i++]);
            var buttonB = MatchButtonB.Match(lines[i++]);
            var prize = MatchPrize.Match(lines[i++]);
            var machine = new Machine(
                new XnY(int.Parse(buttonA.Groups[1].Value), int.Parse(buttonA.Groups[2].Value)),
                new XnY(int.Parse(buttonB.Groups[1].Value), int.Parse(buttonB.Groups[2].Value)),
                new XnY(int.Parse(prize.Groups[1].Value), int.Parse(prize.Groups[2].Value)));
            machines.Add(machine);
        }

        return machines.ToArray();
    }

    private record XnY(int X, int Y);

    private record Machine(XnY A, XnY B, XnY Prize);

    private record Result(int A, int B);
}

using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec13(ITestOutputHelper output)
{
    private static readonly Regex MatchButtonA = new Regex("Button A: X\\+(\\d+), Y\\+(\\d+)");
    private static readonly Regex MatchButtonB = new Regex("Button B: X\\+(\\d+), Y\\+(\\d+)");
    private static readonly Regex MatchPrize = new Regex("Prize: X=(\\d+), Y=(\\d+)");

    private const long Extra = 10_000_000_000_000;

    [Fact]
    public void Calculate1()
    {
        var machines = GetData();

        var total = 0L;
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

    [Fact]
    public void Calculate2()
    {
        var machines = GetData();

        var total = 0L;
        foreach (var machine in machines)
        {
            var newPrize = machine.Prize with { X = machine.Prize.X + Extra, Y = machine.Prize.Y + Extra };

            var xIncs = GetIncs(newPrize.X, machine.A.X, machine.B.X);
            var yIncs = GetIncs(newPrize.Y, machine.A.Y, machine.B.Y);
            if (!xIncs.Found || !yIncs.Found)
                continue;

            var currentX = xIncs.InitialInc;
            var currentY = yIncs.InitialInc;
            var lcm = GetLowestCommonMultiple(xIncs.FurtherInc, yIncs.FurtherInc) * 2;
            while (currentX < xIncs.InitialInc + lcm)
            {
                if (currentX == currentY)
                {
                    var a = currentX;
                    var b = (newPrize.X - (currentX * machine.A.X)) / machine.B.X;
                    total += (a * 3) + b;
                    break;
                }

                if (currentX > currentY)
                {
                    currentY += yIncs.FurtherInc;
                }
                else
                {
                    currentX += xIncs.FurtherInc;
                }
            }
        }

        output.WriteLine($"{total}");
    }

    private (bool Found, long InitialInc, long FurtherInc) GetIncs(long total, long num1, long num2)
    {
        var lcm = GetLowestCommonMultiple(num1, num2);
        var val = 0L;
        var inc = 0;
        while (val < lcm)
        {
            if ((total - val) % num2 == 0)
            {
                return (true, inc, lcm / num1);
            }
            val += num1;
            inc++;
        }

        return (false, 0, 0);
    }

    private long GetLowestCommonMultiple(long num, long othernum)
    {
        int i = 1;
        while (true)
        {
            if (num * i % othernum == 0)
            {
                return num * i;
            }
            if (othernum * i % num == 0)
            {
                return othernum * i;
            }
            i++;
        }
    }

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

    private record XnYCount(int Count, long X, long Y);

    private record XnY(long X, long Y);

    private record Machine(XnY A, XnY B, XnY Prize);

    private record Result(long A, long B);
}

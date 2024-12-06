using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec2(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var safeCount = 0;
        var reports = GetData();
        foreach (var report in reports)
        {
            safeCount += IsSafe(report) ? 1 : 0;
        }

        output.WriteLine($"{safeCount}");
    }

    [Fact]
    public void Calculate2()
    {
        var safeCount = 0;
        var reports = GetData();
        foreach (var report in reports)
        {
            safeCount += IsSafeWithinTolerance(report) ? 1 : 0;
        }

        output.WriteLine($"{safeCount}");
    }

    private static bool IsSafe(int[] report)
    {
        var diffs = report.Skip(1).Select((x, i) => report[i] - x).ToArray();
        return diffs.All(x => Math.Abs(x) <= 3) && diffs.All(x => x * diffs[0] > 0);
    }

    private static bool IsSafeWithinTolerance(int[] report)
    {
        var isSafe = IsSafe(report);
        if (isSafe)
            return true;

        for (int i = 0; i < report.Length; i++)
        {
            var start = report[0..i];
            var end = report[(i + 1)..];

            if (IsSafe(start.Concat(end).ToArray()))
                return true;
        }
        return false;
    }

    private static IEnumerable<int[]> GetData()
    {
        var text = File.ReadAllLines("./input/Dec2.txt");
        foreach (var line in text)
        {
            yield return line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray();
        }
    }
}

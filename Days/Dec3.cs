using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec3(ITestOutputHelper output)
{
    private static readonly Regex _mulRegex = new Regex("mul\\((\\d{1,3}),(\\d{1,3})\\)");
    private static readonly Regex _doRegex = new Regex("do\\(\\)");
    private static readonly Regex _dontRegex = new Regex("don't\\(\\)");

    [Fact]
    public void Calculate1()
    {
        var text = GetData();

        var result = GetResult(text);

        output.WriteLine($"{result}");
    }

    [Fact]
    public void Calculate2()
    {
        var text = "do()" + GetData();

        var doMatches = _doRegex.Matches(text).Select(x => x.Index).ToArray();
        var dontMatches = _dontRegex.Matches(text).Select(x => x.Index).ToArray();

        var result = 0L;
        foreach (var doIndex in doMatches)
        {
            var nextDoIndex = doMatches.FirstOrDefault(x => x > doIndex, text.Length);
            var nextDontIndex = dontMatches.FirstOrDefault(x => x > doIndex, text.Length);
            var stopIndex = Math.Min(nextDoIndex, nextDontIndex);

            result += GetResult(text[doIndex..stopIndex]);
        }

        output.WriteLine($"{result}");
    }

    private static long GetResult(string text)
    {
        var result = 0L;
        var results = _mulRegex.Matches(text);
        foreach (Match match in results)
        {
            result += int.Parse(match.Groups[1].Value) * int.Parse(match.Groups[2].Value);
        }

        return result;
    }

    private static string GetData()
    {
        return File.ReadAllText("./input/Dec3.txt");
    }
}
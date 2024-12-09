using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec5(ITestOutputHelper output)
{
    [Fact]
    public async Task Calculate1()
    {
        var total = 0;
        var (rules, updates) = await GetData();

        var after = rules.ToLookup(x => x.Left).ToDictionary(x => x.Key, x => x.Select(y => y.Right).ToHashSet());
        var before = rules.ToLookup(x => x.Right).ToDictionary(x => x.Key, x => x.Select(y => y.Left).ToHashSet());

        foreach(var update in updates)
        {
            bool isvalid = true;
            for(int i = 0; i < update.Length; i++)
            {
                var num = update[i];
                var toLeft = update[0..i];
                var toRight = update[(i + 1)..update.Length];

                var shouldComeAfter = after.TryGetValue(num, out var a) ? a : [];
                var shouldComeBefore = before.TryGetValue(num, out var b) ? b : [];

                var invalidOnLeft = toLeft.Any(shouldComeAfter.Contains);
                var invalidOnRight = toRight.Any(shouldComeBefore.Contains);

                if (invalidOnLeft || invalidOnRight)
                {
                    isvalid = false;
                    break;
                }
            }

            if (isvalid)
            {
                var middle = update[update.Length / 2];
                total += middle;
            }

        }

        output.WriteLine($"{total}");
    }

    private static async Task<(Rule[] Rules, List<int[]> Updates)> GetData()
    {
        var text = await File.ReadAllLinesAsync("./input/Dec5.txt");
        var rules = text
             .Where(x => x.Contains('|'))
             .Select(x => x.Split('|'))
             .Select(x => new Rule(int.Parse(x[0]), int.Parse(x[1])))
             .ToArray();
        var updates = text
             .Where(x => x.Contains(','))
             .Select(x => x.Split(','))
             .Select(x => x.Select(y => int.Parse(y)).ToArray())
             .ToList();

        return (rules.ToArray(), updates);
    }

    private record Rule(int Left, int Right);
}

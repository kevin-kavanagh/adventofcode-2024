using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec5(ITestOutputHelper output)
{
    [Fact]
    public async Task Calculate1()
    {
        var (rules, updates) = await GetData();
        var after = rules.ToLookup(x => x.Left).ToDictionary(x => x.Key, x => x.Select(y => y.Right).ToHashSet());
        var before = rules.ToLookup(x => x.Right).ToDictionary(x => x.Key, x => x.Select(y => y.Left).ToHashSet());

        var validUpdates = updates.Where(x => IsValid(x, before, after));
        var total = validUpdates.Sum(x => x[x.Length / 2]);

        output.WriteLine($"{total}");
    }

    [Fact]
    public async Task Calculate2()
    {
        var (rules, updates) = await GetData();
        var after = rules.ToLookup(x => x.Left).ToDictionary(x => x.Key, x => x.Select(y => y.Right).ToHashSet());
        var before = rules.ToLookup(x => x.Right).ToDictionary(x => x.Key, x => x.Select(y => y.Left).ToHashSet());

        var total = 0;
        var invalidUpdates = updates.Where(x => !IsValid(x, before, after));
        foreach (var update in invalidUpdates)
        {
            var correctedUpdate = new List<int>();
            var remaining = update.ToList();
            while (remaining.Count > 0)
            {
                var next = remaining
                    .FirstOrDefault(x => remaining.Except([x])
                        .All(y => after.TryGetValue(x, out var others) ? others.Contains(y) : false));
                if (next == default)
                {
                    correctedUpdate.AddRange(remaining);
                    remaining.Clear();
                }
                else
                {
                    correctedUpdate.Add(next);
                    remaining.Remove(next);
                }
            }

            Assert.True(IsValid(correctedUpdate.ToArray(), before, after));
            total += correctedUpdate[correctedUpdate.Count / 2];
        }

        output.WriteLine($"{total}");
    }

    private static bool IsValid(int[] update, Dictionary<int, HashSet<int>> before, Dictionary<int, HashSet<int>> after)
    {
        for (int i = 0; i < update.Length; i++)
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
                return false;
            }
        }
        return true;
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

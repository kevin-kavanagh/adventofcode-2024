using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;
public class Dec1(ITestOutputHelper output)
{
    [Fact]
    public async Task Calculate1()
    {
        var (left, right) = await GetData();

        left.Sort();
        right.Sort();
        long total = 0;
        for (int i = 0; i < left.Count; i++)
        {
            total += Math.Abs(left[i] - right[i]);
        }

        output.WriteLine($"{total}");
    }

    [Fact]
    public async Task Calculate2()
    {
        var (left, right) = await GetData();
        var rightCount = right.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());

        long total = 0;
        foreach (var id in left)
        {
            if (rightCount.TryGetValue(id, out var count))
            {
                total += id * count;
            }
        }

        output.WriteLine($"{total}");
    }

    private static async Task<(List<int>, List<int>)> GetData()
    {
        var left = new List<int>();
        var right = new List<int>();
        var text = await File.ReadAllLinesAsync("./input/Dec1.txt");
        foreach (var line in text)
        {
            var tokens = line?.Split(' ', StringSplitOptions.RemoveEmptyEntries) ?? [];
            if (tokens.Length == 2 &&
                int.TryParse(tokens[0], out var lId) &&
                int.TryParse(tokens[1], out var rId))
            {
                left.Add(lId);
                right.Add(rId);
            }
        }

        return (left, right);
    }
}
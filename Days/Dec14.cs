using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec14(ITestOutputHelper output)
{
    private static readonly Regex RobotRegex = new Regex("p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)");

    [Fact]
    public void Calculate1()
    {
        var iterations = 100;
        var maxX = 101;
        var maxY = 103;

        var robots = GetData();

        var locations = new List<Tile>();
        foreach (var robot in robots)
        {
            var mx = iterations * robot.Vx;
            var x = mx > robot.X ? (robot.X + mx) % maxX : (maxX - Math.Abs((robot.X + mx) % maxX)) % maxX;

            var my = iterations * robot.Vy;
            var y = my > robot.Y ? (robot.Y + my) % maxY : (maxY - Math.Abs((robot.Y + my) % maxY)) % maxY;

            locations.Add(new Tile(x, y));
        }

        var ignoreX = maxX / 2;
        var ignoreY = maxY / 2;

        var q1 = locations.Where(x => x.X < ignoreX && x.Y < ignoreY).Count();
        var q2 = locations.Where(x => x.X > ignoreX && x.Y < ignoreY).Count();
        var q3 = locations.Where(x => x.X > ignoreX && x.Y > ignoreY).Count();
        var q4 = locations.Where(x => x.X < ignoreX && x.Y > ignoreY).Count();
        var total = q1 * q2 * q3 * q4;

        output.WriteLine($"{total}");
    }

    [Fact]
    public void Calculate2()
    {
        var maxX = 101;
        var maxY = 103;
        var ignoreX = maxX / 2;
        var ignoreY = maxY / 2;
        var threshold = 0.01;

        var robots = GetData();

        for (int i = 1; i < 1000000; i++)
        {
            var locations = new List<Tile>();
            foreach (var robot in robots)
            {
                var mx = i * robot.Vx;
                var x = mx > robot.X ? (robot.X + mx) % maxX : (maxX - Math.Abs((robot.X + mx) % maxX)) % maxX;

                var my = i * robot.Vy;
                var y = my > robot.Y ? (robot.Y + my) % maxY : (maxY - Math.Abs((robot.Y + my) % maxY)) % maxY;

                locations.Add(new Tile(x, y));
            }

            var q1 = locations.Where(x => x.X < ignoreX && x.Y < ignoreY).Count();
            var q2 = locations.Where(x => x.X > ignoreX && x.Y < ignoreY).Count();
            var q3 = locations.Where(x => x.X > ignoreX && x.Y > ignoreY).Count();
            var q4 = locations.Where(x => x.X < ignoreX && x.Y > ignoreY).Count();

            if (Math.Abs(q1 - q2) / (double)q1 < threshold && Math.Abs(q3 - q4) / (double)q3 < threshold)
            {
                static string ToChar(int num)
                    => num switch
                    {
                        0 => ".",
                        < 0 or > 0 => num.ToString()
                    };

                output.WriteLine($"Iteration: {i}");
                for (int y = 0; y < maxY; y++)
                {
                    var lu = locations.Where(l => l.Y == y).ToLookup(l => l.X);
                    output.WriteLine(string.Join("", Enumerable.Range(0, maxX).Select(i => ToChar(lu[i].Count()))));
                }
            }
        }
    }

    private Robot[] GetData()
    {
        var robots = new List<Robot>();
        var lines = File.ReadAllLines("./input/Dec14.txt");
        foreach (var line in lines)
        {
            var matches = RobotRegex.Matches(line);
            robots.Add(
                new Robot(
                    int.Parse(matches[0].Groups[1].Value),
                    int.Parse(matches[0].Groups[2].Value),
                    int.Parse(matches[0].Groups[3].Value),
                    int.Parse(matches[0].Groups[4].Value)));
        }

        return robots.ToArray();
    }

    private record Robot(int X, int Y, int Vx, int Vy);
    private record Tile(int X, int Y);
}

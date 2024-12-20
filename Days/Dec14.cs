using System.Text.RegularExpressions;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec14(ITestOutputHelper output)
{
    private static readonly Regex RobotRegex = new Regex("p=(\\d+),(\\d+) v=(-?\\d+),(-?\\d+)");

    [Fact]
    public void Calculate1()
    {
        int iterations = 100;
        int maxX = 11;
        int maxY = 7;

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

        for (int y = 0; y < maxY; y++)
        {
            var lu = locations.Where(l => l.Y == y).ToLookup(l => l.X);
            output.WriteLine(string.Join("", Enumerable.Range(0, maxX).Select(i => lu[i].Count().ToString())));
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

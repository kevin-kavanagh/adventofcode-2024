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
            var x = (robot.X + (iterations * robot.Vx)) % maxX;
            var y = (robot.Y + (iterations * robot.Vy)) % maxY;
            locations.Add(new Tile(x, y));
        }

        var ignoreX = (maxX / 2) + 1;
        var ignoreY = (maxY / 2) + 1;

        var total = locations.Where(x => x.X != ignoreX && x.Y != ignoreY).Sum(x => 1);

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

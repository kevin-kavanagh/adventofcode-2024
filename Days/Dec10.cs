using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec10(ITestOutputHelper output)
{
    private static readonly char[] Directions = ['N', 'S', 'W', 'E'];

    [Fact]
    public void Calculate1()
    {
        var data = GetData();
        var locations = data.ToDictionary(x => x.Location);

        var paths = 0;
        foreach (var tile in data.Where(x => x.Height == 0))
        {
            var finalTiles = GetPaths(tile, locations);
            paths += finalTiles.Distinct().Count();
        }

        output.WriteLine($"{paths}");
    }

    private Tile[] GetPaths(Tile tile, Dictionary<Location, Tile> locations)
    {
        if (tile.Height == 9)
        {
            return [tile];
        }

        var finalTiles = new List<Tile>();
        foreach (var direction in Directions)
        {
            var nexTile = Get(direction, tile, locations);
            if (nexTile?.Height == tile.Height + 1)
            {
                finalTiles.AddRange(GetPaths(nexTile, locations));
            }
        }

        return finalTiles.ToArray();
    }

    private Tile? Get(char direction, Tile tile, Dictionary<Location, Tile> locations)
    {
        return direction switch
        {
            'N' => locations.TryGetValue(new Location(tile.Location.X, tile.Location.Y - 1), out var north) ? north : null,
            'S' => locations.TryGetValue(new Location(tile.Location.X, tile.Location.Y + 1), out var south) ? south : null,
            'W' => locations.TryGetValue(new Location(tile.Location.X - 1, tile.Location.Y), out var west) ? west : null,
            'E' => locations.TryGetValue(new Location(tile.Location.X + 1, tile.Location.Y), out var east) ? east : null,
            _ => throw new NotImplementedException()
        };
    }

    private Tile[] GetData()
    {
        var lines = File.ReadAllLines("./input/Dec10.txt");

        var tiles = new List<Tile>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                tiles.Add(new Tile(new Location(x, y), int.TryParse(lines[y][x].ToString(), out var n) ? n : 11));
            }
        }

        return tiles.ToArray();
    }

    private record Location(int X, int Y);
    private record Tile(Location Location, int Height);
}

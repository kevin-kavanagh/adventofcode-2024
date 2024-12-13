using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec8(ITestOutputHelper output)
{
    private const char Empty = '.';

    [Fact]
    public void Calculate1()
    {
        var data = GetData();

        var groups = data.Tiles.Where(x => x.Freq != Empty).GroupBy(x => x.Freq);
        foreach (var group in groups.Select(x => x.ToArray()))
        {
            for (int t1 = 0; t1 < group.Length - 1; t1++)
            {
                for (int t2 = t1 + 1; t2 < group.Length; t2++)
                {
                    var tile = group[t1];
                    var otherTile = group[t2];

                    var distance = tile.Location.GetDistance(otherTile.Location);
                    var antinode1 = data.GetTile(tile.Location.X + distance.X, tile.Location.Y + distance.Y);
                    var antinode2 = data.GetTile(otherTile.Location.X - distance.X, otherTile.Location.Y - distance.Y);

                    antinode1?.MarkAsAntinode();
                    antinode2?.MarkAsAntinode();
                }
            }
        }

        var antinodes = data.Tiles.Count(x => x.IsAntinode());
        output.WriteLine($"{antinodes}");
    }

    [Fact]
    public void Calculate2()
    {
        var data = GetData();

        var groups = data.Tiles.Where(x => x.Freq != Empty).GroupBy(x => x.Freq);
        foreach (var group in groups.Select(x => x.ToArray()))
        {
            for (int t1 = 0; t1 < group.Length - 1; t1++)
            {
                for (int t2 = t1 + 1; t2 < group.Length; t2++)
                {
                    var tile = group[t1];
                    tile.MarkAsAntinode();
                    var otherTile = group[t2];
                    otherTile.MarkAsAntinode();

                    var distance = tile.Location.GetDistance(otherTile.Location);

                    var antinode1 = data.GetTile(tile.Location.X + distance.X, tile.Location.Y + distance.Y);
                    while (antinode1 != null)
                    {
                        antinode1.MarkAsAntinode();
                        antinode1 = data.GetTile(antinode1.Location.X + distance.X, antinode1.Location.Y + distance.Y);
                    }

                    var antinode2 = data.GetTile(otherTile.Location.X - distance.X, otherTile.Location.Y - distance.Y);
                    while (antinode2 != null)
                    {
                        antinode2.MarkAsAntinode();
                        antinode2 = data.GetTile(antinode2.Location.X - distance.X, antinode2.Location.Y - distance.Y);
                    }
                }
            }
        }

        var antinodes = data.Tiles.Count(x => x.IsAntinode());
        output.WriteLine($"{antinodes}");
    }

    private Area GetData()
    {
        var lines = File.ReadAllLines("./input/Dec8.txt");

        var tiles = new List<Tile>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                tiles.Add(new Tile(lines[y][x], new Location(x, y)));
            }
        }

        return new Area([.. tiles]);
    }

    public record Area
    {
        private Dictionary<Location, Tile> _tileLookup;

        public Area(Tile[] tiles)
        {
            Tiles = tiles;

            _tileLookup = tiles.ToDictionary(x => x.Location);
        }

        public Tile[] Tiles { get; }

        public Tile? GetTile(int x, int y)
        {
            return _tileLookup.TryGetValue(new Location(x, y), out var tile) ? tile : null;
        }
    }

    public record Tile(char Freq, Location Location)
    {
        private bool _isAntinode;

        public void MarkAsAntinode()
        {
            _isAntinode = true;
        }

        public bool IsAntinode()
        {
            return _isAntinode;
        }
    }

    public record struct Location(int X, int Y)
    {
        public (int X, int Y) GetDistance(Location location)
        {
            return (X - location.X, Y - location.Y);
        }
    }
}

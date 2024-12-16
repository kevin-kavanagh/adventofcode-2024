using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec12(ITestOutputHelper output)
{
    private static readonly char[] Directions = ['N', 'S', 'W', 'E'];

    [Fact]
    public void Calculate1()
    {
        var data = GetData();
        var garden = new Garden(data);

        var total = 0;
        foreach (var plot in garden.Plots)
        {
            if (!plot.IsInGroup)
            {
                var result = Group(plot, garden);
                var price = result.Area * result.Perimeter;
                total += price;
            }
        }

        output.WriteLine($"{total}");
    }

    private (int Area, int Perimeter) Group(Plot plot, Garden garden)
    {
        plot.IsInGroup = true;
        var area = 1;
        var perimeter = 0;
        foreach (var direction in Directions)
        {
            var neighbour = garden.Get(direction, plot);
            if (neighbour == null || neighbour.Type != plot.Type)
            {
                perimeter++;
            }
            else if (!neighbour.IsInGroup)
            {
                var res = Group(neighbour, garden);
                area += res.Area;
                perimeter += res.Perimeter;
            }
        }

        return (area, perimeter);
    }

    private Plot[] GetData()
    {
        var lines = File.ReadAllLines("./input/Dec12.txt");

        var tiles = new List<Plot>();
        for (int y = 0; y < lines.Length; y++)
        {
            for (int x = 0; x < lines[0].Length; x++)
            {
                tiles.Add(new Plot(new Location(x, y), lines[y][x]));
            }
        }

        return tiles.ToArray();
    }

    private record Location(int X, int Y);
    private record Plot(Location Location, char Type)
    {
        public bool IsInGroup { get; set; }
    }
    private record Garden
    {
        private Dictionary<Location, Plot> _plots;

        public Garden(Plot[] plots)
        {
            _plots = plots.ToDictionary(x => x.Location);
            Plots = plots;
        }

        public Plot[] Plots { get; }

        public Plot? Get(char direction, Plot plot)
        {
            return direction switch
            {
                'N' => _plots.TryGetValue(new Location(plot.Location.X, plot.Location.Y - 1), out var north) ? north : null,
                'S' => _plots.TryGetValue(new Location(plot.Location.X, plot.Location.Y + 1), out var south) ? south : null,
                'W' => _plots.TryGetValue(new Location(plot.Location.X - 1, plot.Location.Y), out var west) ? west : null,
                'E' => _plots.TryGetValue(new Location(plot.Location.X + 1, plot.Location.Y), out var east) ? east : null,
                _ => throw new NotImplementedException()
            };
        }
    }
}

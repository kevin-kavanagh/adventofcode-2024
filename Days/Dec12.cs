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
                var price = result.Plots.Length * result.Perimeter;
                total += price;
            }
        }

        output.WriteLine($"{total}");
    }

    [Fact]
    public void Calculate2()
    {
        var data = GetData();
        var garden = new Garden(data);

        var total = 0;
        foreach (var plot in garden.Plots)
        {
            if (!plot.IsInGroup)
            {
                var result = Group(plot, garden);
                var corners = result.Plots.SelectMany(x => x.GetCorners());

                // Counting the corners is the same as counting the sides, but we only care
                // about corners where 1 or 3 plots return the corner. 2 is a straight line,
                // 4 is enclosed.
                var sides = corners.GroupBy(x => x).Sum(x =>
                {
                    var count = x.Count();
                    if (count is 1 or 3)
                        return 1;

                    if(count == 2)
                    {
                        // Diagonals are a special case
                        var corner = x.Key;
                        var plotLookup = result.Plots.ToDictionary(x => x.Location);
                        if(plotLookup.ContainsKey(new Location(corner.X, corner.Y)) &&
                            plotLookup.ContainsKey(new Location(corner.X - 1, corner.Y - 1)))
                        {
                            return 2;
                        }
                        if(plotLookup.ContainsKey(new Location(corner.X - 1, corner.Y)) &&
                            plotLookup.ContainsKey(new Location(corner.X, corner.Y - 1)))
                        {
                            return 2;
                        }
                    }

                    return 0;
                });

                total += result.Plots.Length * sides;
            }
        }

        output.WriteLine($"{total}");
    }

    private (int Perimeter, Plot[] Plots) Group(Plot plot, Garden garden)
    {
        var plots = new List<Plot> { plot };
        plot.IsInGroup = true;
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
                perimeter += res.Perimeter;
                plots.AddRange(res.Plots);
            }
        }

        return (perimeter, plots.ToArray());
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

        public Location[] GetCorners()
        {
            return
                [
                new Location(Location.X, Location.Y),
                new Location(Location.X + 1, Location.Y),
                new Location(Location.X + 1, Location.Y + 1),
                new Location(Location.X, Location.Y + 1),
                ];
        }
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

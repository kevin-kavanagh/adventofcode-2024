using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec6(ITestOutputHelper output)
{
    private const char Guard = '^';
    private const char Barrier = '#';
    private const char Open = '.';

    [Fact]
    public async Task Calculate1()
    {
        var data = GetData();
        var board = new Board(data);

    }

    private static List<char[]> GetData()
    {
        var lines = File.ReadLines("./input/Dec4.txt");
        return lines.Select(line => line.ToArray()).ToList();
    }

    public class Board
    {
        private Dictionary<Location, Tile> _board = new();
        private Location _guardLocation;
        private int _maxX;
        private int _maxY;

        public Board(List<char[]> data)
        {
            for (var y = 0; y < data.Count; y++)
            {
                for (var x = 0; x < data[y].Length; x++)
                {
                    var location = new Location(x, y);
                    var type = data[y][x];
                    _board[location] = new Tile(type);
                }
            }
            _guardLocation = _board.Single(x => x.Value.Type == Guard).Key;
            _maxX = _board.Keys.Max(x => x.X);
            _maxY = _board.Keys.Max(x => x.Y);
        }

        public void Process()
        {

        }
    }

    public record Location(int X, int Y);

    public record Tile(char Type)
    {
        public bool Visited { get; set; }
    }
}
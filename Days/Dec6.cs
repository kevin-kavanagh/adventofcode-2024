using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec6(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var data = GetData();
        var board = new Board(data);

        board.Process();
        var visited = board.GetVisited();

        output.WriteLine($"{visited}");
    }

    private static List<char[]> GetData()
    {
        var lines = File.ReadLines("./input/Dec6.txt");
        return lines.Select(line => line.ToArray()).ToList();
    }

    public class Board
    {
        private Dictionary<Location, Tile> _board = new();
        private Guard _guard;
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
                    _board[location] = new Tile(type, location);
                }
            }

            var guardLocation = _board.Single(x => x.Value.Type != Tile.Barrier && x.Value.Type != Tile.Open).Key;
            var guardDirection = new[] { Direction.Left, Direction.Right, Direction.Up, Direction.Down }
                .Single(x => x.C == _board[guardLocation].Type);
            _guard = new Guard(guardLocation, guardDirection);

            _maxX = _board.Keys.Max(x => x.X);
            _maxY = _board.Keys.Max(x => x.Y);
        }

        public void Process()
        {
            // Mark first tile as visited

            var startTile = GetTile(_guard.Location.X, _guard.Location.Y);
            startTile?.Visit(_guard);

            while (!_guard.LeftArea)
            {
                var nextTile = GetTile(
                    _guard.Location.X + _guard.Direction.X,
                    _guard.Location.Y + _guard.Direction.Y);

                _guard.Move(nextTile);
            }
        }

        public int GetVisited()
        {
            return _board.Values.Count(x => x.Visited);
        }

        private Tile? GetTile(int x, int y)
        {
            return _board.TryGetValue(new Location(x, y), out var tile) ? tile : null;
        }
    }

    public record Location(int X, int Y);

    public record Direction(char C, int X, int Y)
    {
        public static readonly Direction Left = new('<', -1, 0);
        public static readonly Direction Right = new('>', 1, 0);
        public static readonly Direction Up = new('^', 0, -1);
        public static readonly Direction Down = new('v', 0, 1);
    }

    public record Tile(char Type, Location Location)
    {
        private Dictionary<Direction, int> _visits = new();

        public const char Barrier = '#';
        public const char Open = '.';

        public bool Visited => _visits.Count > 0;

        public void Visit(Guard guard)
        {
            if (_visits.TryGetValue(guard.Direction, out var count))
            {
                _visits[guard.Direction] = count + 1;
            }
            else
            {
                _visits[guard.Direction] = 1;
            }
        }
    }

    public record Guard
    {
        public Guard(Location location, Direction direction)
        {
            Location = location;
            Direction = direction;
        }

        public Location Location { get; private set; }
        public Direction Direction { get; private set; }
        public bool LeftArea { get; private set; }

        public void Move(Tile? nextTile)
        {
            if (LeftArea)
                return;

            if (nextTile is null)
            {
                LeftArea = true;
                return;
            }

            if (nextTile.Type == Tile.Barrier)
            {
                Turn();
                return;
            }

            MoveForward(nextTile);
        }

        private void Turn()
        {
            Direction = Direction.C switch
            {
                '<' => Direction.Up,
                '^' => Direction.Right,
                '>' => Direction.Down,
                'v' => Direction.Left,
                _ => throw new NotImplementedException()
            };
        }

        private void MoveForward(Tile tile)
        {
            Location = tile.Location;
            tile.Visit(this);
        }
    }
}
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec4(ITestOutputHelper output)
{
    private static readonly char[] Xmas = "XMAS".ToArray();
    private static readonly string Ms = "MS";
    private static readonly string Sm = "SM";

    [Fact]
    public void Calculate1()
    {
        var board = GetData();

        var result = 0;
        for (int y = 0; y < board.Count; y++)
        {
            for (int x = 0; x < board[0].Length; x++)
            {
                if (board[y][x] == Xmas[0])
                {
                    result +=
                        // E
                        IsXmas(board, x, y, 1, 0) +
                        // SE
                        IsXmas(board, x, y, 1, 1) +
                        // S
                        IsXmas(board, x, y, 0, 1) +
                        // SW
                        IsXmas(board, x, y, -1, 1) +
                        // W
                        IsXmas(board, x, y, -1, 0) +
                        // NW
                        IsXmas(board, x, y, -1, -1) +
                        // N
                        IsXmas(board, x, y, 0, -1) +
                        // NE
                        IsXmas(board, x, y, 1, -1);
                }
            }
        }

        output.WriteLine($"{result}");
    }

    [Fact]
    public void Calculate2()
    {
        var board = GetData();

        var result = 0;
        for (int y = 0; y < board.Count; y++)
        {
            for (int x = 0; x < board[0].Length; x++)
            {
                result += IsXmasPair(board, x, y);
            }
        }

        output.WriteLine($"{result}");
    }

    private static int IsXmas(List<char[]> board, int x, int y, int xDelta, int yDelta)
    {
        if (board[y][x] != Xmas[0])
            return 0;

        for (int i = 1; i < 4; i++)
        {
            var xpos = x + xDelta * i;
            var ypos = y + yDelta * i;
            if (xpos < 0 || xpos >= board[0].Length || ypos < 0 || ypos >= board.Count)
                return 0;
            if (board[ypos][xpos] != Xmas[i])
                return 0;
        }

        return 1;
    }

    private static int IsXmasPair(List<char[]> board, int x, int y)
    {
        if (board[y][x] != Xmas[2])
            return 0;

        if (x - 1 < 0 || x + 1 >= board[0].Length || y - 1 < 0 || y + 1 >= board.Count)
            return 0;

        var pair1 = $"{board[y - 1][x - 1]}{board[y + 1][x + 1]}";
        var pair2 = $"{board[y - 1][x + 1]}{board[y + 1][x - 1]}";
        if ((pair1 == Sm || pair1 == Ms) && (pair2 == Sm || pair2 == Ms))
            return 1;

        return 0;
    }


    private static List<char[]> GetData()
    {
        var lines = File.ReadLines("./input/Dec4.txt");
        return lines.Select(line => line.ToArray()).ToList();
    }
}

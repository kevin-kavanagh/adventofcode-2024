using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec11(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var stones = GetData();

        for (int i = 0; i < 25; i++)
        {
            stones = stones.SelectMany(x => x.Blink()).ToArray();
        }

        output.WriteLine($"{stones.Length}");
    }

    [Fact]
    public void Calculate2()
    {
        var stones = GetData();

        for (int i = 0; i < 75; i++)
        {
            stones = stones.SelectMany(x => x.Blink()).ToArray();
        }

        output.WriteLine($"{stones.Length}");
    }

    private Stone[] GetData()
    {
        var nums = File.ReadAllText("./input/Dec11.txt");
        return nums.Split(" ").Select(x => new Stone(int.Parse(x))).ToArray();
    }

    private record struct Stone(long Num)
    {
        public Stone[] Blink()
        {
            if (Num == 0)
                return [new Stone(1)];

            var num = Num.ToString();
            if (num.Length % 2 == 0)
            {
                return
                [
                    new Stone(long.Parse(num[0 .. (num.Length / 2)])),
                    new Stone(long.Parse(num[(num.Length / 2) ..])),
                ];
            }

            return [new Stone(Num * 2024)];
        }
    }
}

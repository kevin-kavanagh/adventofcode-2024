using System;
using System.Reflection.Metadata.Ecma335;
using System.Security.AccessControl;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec11(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var stones = GetData().Select(x => x.Num);

        var count = stones.Sum(x => Blink(x, 25));

        output.WriteLine($"{count}");
    }

    [Fact]
    public void Calculate2()
    {
        var stones = GetData().Select(x => x.Num);

        var count = stones.Sum(x => Blink(x, 75));

        output.WriteLine($"{count}");
    }

    private int Blink(long stone, int blinks)
    {
        var nextBlinks = blinks - 1;
        if (blinks == 0)
        {
            return 1;
        }

        if (stone == 0)
        {
            return Blink(1, nextBlinks);
        }

        var digits = Digits(stone);
        if (digits % 2 == 0)
        {
            var m = Convert.ToInt64(Math.Pow(10, digits / 2));
            var left = stone / m;
            var right = stone % m;
            return Blink(left, nextBlinks) + Blink(right, nextBlinks);
        }

        return Blink(stone * 2024, nextBlinks);
    }

    private static int Digits(long number)
    {
        int count = 0;
        while (number > 0)
        {
            number /= 10;
            count++;
        }
        return count;
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

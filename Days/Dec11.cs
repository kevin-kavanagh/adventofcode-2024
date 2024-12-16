using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec11(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var stones = GetData();

        var cache = new Cache();
        var count = stones.Sum(x => Blink(x, 25, cache));

        output.WriteLine($"{count}");
    }

    [Fact]
    public void Calculate2()
    {
        var stones = GetData();

        var cache = new Cache();
        var count = stones.Sum(x => Blink(x, 75, cache));

        output.WriteLine($"{count}");
    }

    private long Blink(long stone, int blinks, Cache cache)
    {
        var key = new Key(stone, blinks);
        return GetOrCreate(key, cache, () =>
        {

            var nextBlinks = blinks - 1;
            if (blinks == 0)
            {
                return 1;
            }

            if (stone == 0)
            {
                return Blink(1, nextBlinks, cache);
            }


            var digits = Digits(stone);
            if (digits % 2 == 0)
            {
                var m = Convert.ToInt64(Math.Pow(10, digits / 2));
                var left = stone / m;
                var right = stone % m;
                return Blink(left, nextBlinks, cache) + Blink(right, nextBlinks, cache);
            }

            return Blink(stone * 2024, nextBlinks, cache);
        });
    }

    private static long GetOrCreate(Key key, Cache cache, Func<long> getter)
    {
        if (cache.Data.TryGetValue(key, out var val))
        {
            return val;
        }
        var newVal = getter();
        cache.Data.Add(key, newVal);
        return newVal;
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

    private long[] GetData()
    {
        var nums = File.ReadAllText("./input/Dec11.txt");
        return nums.Split(" ").Select(long.Parse).ToArray();
    }

    public record Key(long Num, int Blinks);
    public class Cache
    {
        public Dictionary<Key, long> Data { get; } = new Dictionary<Key, long>();
    }
}

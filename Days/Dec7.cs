﻿using System.Data;
using System.Security.Cryptography.X509Certificates;
using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec7(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var data = GetData();

        var total = 0L;
        foreach(var item in data)
        {
            var results = Calc(item.Numbers);
            if(results.Any(x => x == item.Total))
            {
                total += item.Total;
            }
        }
        output.WriteLine($"{total}");
    }

    public long[] Calc(long[] numbers)
    {
        if (numbers.Length == 1)
            return numbers;

        var sub = Calc(numbers[..^1]);
        var multiply = sub.Select(x => numbers[^1] * x).ToArray();
        var addition = sub.Select(x => numbers[^1] + x).ToArray();

        return [..multiply, ..addition];
    }



    private Equation[] GetData()
    {
        var lines = File.ReadAllLines("./input/Dec7.txt");

        return lines
            .Select(x => x.Split(":", StringSplitOptions.TrimEntries))
            .Select(x => new Equation(long.Parse(x[0]), x[1].Split(" ", StringSplitOptions.TrimEntries).Select(long.Parse).ToArray()))
            .ToArray();
    }

    public record Equation(long Total, long[] Numbers);
}
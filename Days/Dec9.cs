using Xunit.Abstractions;

namespace AdventOfCode_2024.Days;

public class Dec9(ITestOutputHelper output)
{
    [Fact]
    public void Calculate1()
    {
        var data = GetData();

        var list = new List<FileBlock?>();
        var id = 0;
        bool isFile = true;
        foreach (var digit in data)
        {
            if (isFile)
            {
                list.AddRange(Enumerable.Range(0, digit).Select(_ => new FileBlock(id)));
                id++;
            }
            else
            {
                list.AddRange(Enumerable.Range(0, digit).Select(_ => (FileBlock)null!));
            }
            isFile = !isFile;
        }

        var start = 0;
        var end = list.Count - 1;
        while (start < end)
        {
            while (list[start] is not null)
            {
                start++;
            }

            while (list[end] is null)
            {
                end--;
            }

            if (start < end)
            {
                list[start] = list[end];
                list[end] = null;
            }
        }

        var result = list.Select((block, i) => Convert.ToInt64(i) * block?.Id ?? 0).Sum();

        output.WriteLine($"{result}");
    }

    private int[] GetData()
    {
        var nums = File.ReadAllText("./input/Dec9.txt");
        return nums.Select(x => int.Parse(x.ToString())).ToArray();
    }

    private record FileBlock(int Id);
}

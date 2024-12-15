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

    [Fact]
    public void Calculate2()
    {
        var data = GetData();

        var list = new List<Memory>();
        var id = 0;
        bool isFile = true;
        foreach (var digit in data)
        {
            if (isFile)
            {
                if (digit > 0)
                {
                    var blocks = Enumerable.Range(0, digit).Select(_ => new FileBlock(id)).ToArray();
                    list.Add(new Memory(blocks));
                }
                id++;
            }
            else
            {
                if (digit > 0)
                {
                    var empties = Enumerable.Range(0, digit).Select(_ => new Memory([null]));
                    list.AddRange(empties);
                }
            }
            isFile = !isFile;
        }

        foreach (var file in list.Skip(1).Where(x => x.Blocks[0] != null).Reverse().ToArray())
        {
            var fileIndex = list.IndexOf(file);
            for (int i = 0; i < fileIndex; i++)
            {
                if (list[i].Blocks[0] != null)
                {
                    continue;
                }

                var emptyIndex = i;
                if(emptyIndex + file.Blocks.Length > list.Count)
                {
                    break;
                }

                var contiguous = list[emptyIndex .. (emptyIndex + file.Blocks.Length)];
                if(contiguous.All(x => x.Blocks[0] is null))
                {
                    list.Remove(file);
                    list.InsertRange(fileIndex, contiguous);
                    list.RemoveRange(emptyIndex, contiguous.Count);
                    list.Insert(emptyIndex, file);
                    break;
                }
            }
        }
        var newList = list.SelectMany(x => x.Blocks);

        var result = newList.Select((block, i) => Convert.ToInt64(i) * block?.Id ?? 0).Sum();

        output.WriteLine($"{result}");
    }

    private int[] GetData()
    {
        var nums = File.ReadAllText("./input/Dec9.txt");
        return nums.Select(x => int.Parse(x.ToString())).ToArray();
    }

    private record FileBlock(int Id);
    private record Memory(FileBlock?[] Blocks)
    {
        public override string ToString()
        {
            return string.Join("", Blocks.Select(x => x?.Id.ToString() ?? "."));
        }
    }
}

using static Library.Parsing;
using static Library.Testing;

long[] min_beetles(long sparkball, List<long> stamps)
{
    long[] beetles = new long[sparkball + 1];
    Array.Fill(beetles, long.MaxValue);
    beetles[0] = 0;

    foreach (var stamp in stamps)
    {
        for (long i = stamp; i <= sparkball; ++i)
        {
            if (beetles[i - stamp] != long.MaxValue)
            {
                beetles[i] = Math.Min(beetles[i], 1 + beetles[i - stamp]);
            }
        }
    }

    return beetles;
}

long min_split_beetles(long sparkball, List<long> stamps)
{
    var mid = sparkball / 2;
    var start = mid - ((sparkball % 2 == 0) ? 50 : 49);

    long[] beetles = min_beetles(sparkball - start, stamps);

    List<long> possibilities = new();

    for (var split1 = start; split1 <= mid; ++split1)
    {
        var split2 = sparkball - split1;
        possibilities.Add(beetles[split1] + beetles[split2]);
    }

    return possibilities.Min();
}

long part1(string file_name)
{
    List<long> stamps = new() { 1, 3, 5, 10 };
    var input = readFileLines(file_name).Select(long.Parse).ToList();
    return input.Select(sparkball => min_beetles(sparkball, stamps)[sparkball]).Sum();
}

long part2(string file_name)
{
    List<long> stamps = new() { 1, 3, 5, 10, 15, 16, 20, 24, 25, 30 };
    var input = readFileLines(file_name).Select(long.Parse).ToList();
    return input.Select(sparkball => min_beetles(sparkball, stamps)[sparkball]).Sum();
}

long part3(string file_name)
{
    List<long> stamps = new() { 1, 3, 5, 10, 15, 16, 20, 24, 25, 30, 37, 38, 49, 50, 74, 75, 100, 101 };
    var input = readFileLines(file_name).Select(long.Parse).ToList();
    return input.Select(sparkball => min_split_beetles(sparkball, stamps)).Sum();
}

test(part1, "part1", "Sample.txt", 10);
test(part1, "part1", "Notes1.txt", 13724);

test(part2, "part2", "Sample2.txt", 10);
test(part2, "part2", "Notes2.txt", 5116);

test(part3, "part3", "Sample3.txt", 10449);
test(part3, "part3", "Notes3.txt", 145899);

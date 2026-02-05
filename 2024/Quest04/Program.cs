using static Library.Parsing;
using static Library.Testing;

long part1(string file_name)
{
    var input = readFileLines(file_name).Select(long.Parse);
    return input.Sum() - input.Min() * input.Count();
}

long part2(string file_name)
{
    return part1(file_name);
}

long find_median(IEnumerable<long> values)
{
    var ordered = values.Order();

    if (values.Count() % 2 == 1)
    {
        return ordered.ElementAt(values.Count() / 2);
    }

    int right_index = values.Count() / 2;
    int left_index = right_index - 1;
    return (ordered.ElementAt(left_index) + ordered.ElementAt(right_index)) / 2;
}

long part3(string file_name)
{
    var input = readFileLines(file_name).Select(long.Parse);
    long median = find_median(input);
    return input.Select(v => Math.Abs(v - median)).Sum();
}

test(part1, "part1", "Notes1.txt", 65);

test(part2, "part2", "Notes2.txt", 811900);

test(part3, "part3", "Sample.txt", 8);
test(part3, "part3", "Notes3.txt", 126026200);

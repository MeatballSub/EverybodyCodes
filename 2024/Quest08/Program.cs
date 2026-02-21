using static Library.Parsing;
using static Library.Testing;

long part1(string file_name)
{
    var input = long.Parse(readFileLines(file_name)[0]);
    var sqrt_input_rounded_up = (long)Math.Ceiling(Math.Sqrt(input));
    var total_blocks = sqrt_input_rounded_up * sqrt_input_rounded_up;
    var needed_blocks = total_blocks - input;
    var width = (sqrt_input_rounded_up * 2) - 1;

    return width * needed_blocks;
}

long part2(string file_name)
{
    var priests = long.Parse(readFileLines(file_name)[0]);
    long acolytes = 1111;
    long initial_blocks = 20240000;
    long total_blocks = 1;
    long thickness = 1;
    long columns = 1;
    for (long layer = 2; total_blocks < initial_blocks; ++layer)
    {
        thickness = (thickness * priests) % acolytes;
        columns += 2;
        var layer_blocks = columns * thickness;
        total_blocks += layer_blocks;
    }

    long needed_blocks = total_blocks - initial_blocks;

    return needed_blocks * columns;
}

long part3(string file_name)
{
    var priests = long.Parse(readFileLines(file_name)[0]);
    long acolytes = 10;
    long initial_blocks = 202400000;
    long total_blocks = 1;
    long thickness = 1;

    // priests = 2;
    // acolytes = 5;
    // initial_blocks = 160;

    List<long> column_heights = new() { 1 };

    for (long layer = 2; total_blocks <= initial_blocks; ++layer)
    {
        thickness = ((priests * thickness) % acolytes) + acolytes;
        for (int i = 0; i < column_heights.Count; ++i)
        {
            column_heights[i] += thickness;
        }
        column_heights.Insert(0, thickness);
        column_heights.Add(thickness);

        var width = column_heights.Count;
        var spaces = column_heights.Skip(1).SkipLast(1).Select(ch => (priests * width * ch) % acolytes).Sum();

        total_blocks = column_heights.Sum() - spaces;
    }
    return total_blocks - initial_blocks;
}

test(part1, "part1", "Sample.txt", 21);
test(part1, "part1", "Notes1.txt", 11276465);

test(part2, "part2", "Sample.txt", 27);
test(part2, "part2", "Notes2.txt", 113541750);

test(part3, "part3", "Sample.txt", 2);
test(part3, "part3", "Notes3.txt", 37396);

using static Library.Parsing;
using static Library.Testing;

long missing_pinecones(string file_name, Func<long, long> shape_size) =>
    readFile(file_name).ExtractLongs().Sum(x => shape_size(x) - x);

long filled_square(long x)
{
    long edge_length = (long)Math.Ceiling(Math.Sqrt(x));
    return edge_length * edge_length;
}

long hollow_ring(long x) => Math.Max((((x + 3) / 4) * 4), 8);

long square_minus_center(long x)
{
    long edge_length = (long)Math.Ceiling(Math.Sqrt(x + 1));
    if (edge_length % 2 == 0) edge_length++;
    return Math.Max(edge_length * edge_length - 1, 8);
}

long part1(string fileName) => missing_pinecones(fileName, filled_square);
long part2(string fileName) => missing_pinecones(fileName, hollow_ring);
long part3(string fileName) => missing_pinecones(fileName, square_minus_center);

test(part1, "part1", "Sample.txt", 11);
test(part1, "part1", "Notes1.txt", 1343);

test(part2, "part2", "Sample.txt", 9);
test(part2, "part2", "Notes2.txt", 310);

test(part3, "part3", "Sample.txt", 61);
test(part3, "part3", "Notes3.txt", 636081);

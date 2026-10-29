using static Library.Parsing;
using static Library.Testing;

(string standard, string vote) parse(string line)
{
    int prefix_length = line.IndexOf('-') + 1;
    int space_index = line.IndexOf(' ');

    string standard = line.Substring(prefix_length, space_index - prefix_length);
    string vote = line.Substring(space_index + 1);

    return (standard, vote);
}

long to_decimal((string standard, string vote) line)
{
    int standard_len = line.standard.Length;
    int vote_len = line.vote.Length;

    long total = 0;
    int exponent = 0;

    for (int i = 0; i < vote_len; i++)
    {
        char vote_char = line.vote[vote_len - 1 - i];
        long vote_digit = vote_char - '0';

        int standard_index = standard_len - 1 - (i % standard_len);
        char standard_char = line.standard[standard_index];
        long standard_digit = standard_char - '0';

        long power_value = (long)Math.Pow(standard_digit, exponent);
        total += vote_digit * power_value;

        exponent++;
    }

    return total;
}

bool is_valid((string standard, string vote) line)
{
    long target = to_decimal(line);

    long curr_value = long.Parse(line.vote);
    int original_length = line.vote.Length;

    return Enumerable.Range(0, (int)curr_value)
        .Select(v => v.ToString().PadLeft(original_length, '0'))
        .All(smaller_vote_str => to_decimal((line.standard, smaller_vote_str)) != target);
}

long part1(string file_name)
{
    return readFileLines(file_name).Select(parse).Select(to_decimal).Sum();
}

long part2(string file_name)
{
    return readFileLines(file_name).Select(parse).Select(to_decimal).Sum();
}

long part3(string file_name)
{
    return readFileLines(file_name).Select(parse).Where(is_valid).Select(to_decimal).Sum();
}

test(part1, "part1", "Sample.txt", 1478);
test(part1, "part1", "Notes1.txt", 11643);

test(part2, "part2", "Sample2.txt", 1161463);
test(part2, "part2", "Notes2.txt", 24720127781);

test(part3, "part3", "Sample3.txt", 50987);
test(part3, "part3", "Notes3.txt", 521234);

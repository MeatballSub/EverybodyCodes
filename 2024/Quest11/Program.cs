using static Library.Parsing;
using static Library.Testing;

Dictionary<string, string[]> parse(string file_name)
{
    Dictionary<string, string[]> conversions = new();
    var input = readFileLines(file_name);
    foreach (var line in input)
    {
        var line_split = line.Split(':');
        var key = line_split[0];
        var values = line_split[1].Split(',');
        conversions.Add(key, values);
    }
    return conversions;
}

long simulate(Dictionary<string, string[]> conversions, Dictionary<string, long> population, int num_days)
{
    for (int day = 0; day < num_days; ++day)
    {
        Dictionary<string, long> new_population = new();
        foreach (var (category, count) in population)
        {
            if (conversions.ContainsKey(category))
            {
                foreach (var succesor in conversions[category])
                {
                    if (new_population.ContainsKey(succesor))
                    {
                        new_population[succesor] += count;
                    }
                    else
                    {
                        new_population.Add(succesor, count);
                    }
                }
            }
        }
        population = new_population;
    }

    return population.Sum(e => e.Value);
}

long part1(string file_name)
{
    var conversions = parse(file_name);
    Dictionary<string, long> population = new() { { "A", 1 } };
    return simulate(conversions, population, 4);
}

long part2(string file_name)
{
    var conversions = parse(file_name);
    Dictionary<string, long> population = new() { { "Z", 1 } };
    return simulate(conversions, population, 10);
}

long part3(string file_name)
{
    var conversions = parse(file_name);

    var get_count = (string category) =>
    {
        Dictionary<string, long> population = new() { { category, 1 } };
        return simulate(conversions, population, 20);
    };

    var counts = conversions.Keys.Select(get_count);
    return counts.Max() - counts.Min();
}

test(part1, "part1", "Sample.txt", 8);
test(part1, "part1", "Notes1.txt", 31);

test(part2, "part2", "Sample.txt", 0);
test(part2, "part2", "Notes2.txt", 275872);

test(part3, "part3", "Sample3.txt", 268815);
test(part3, "part3", "Notes3.txt", 1389256676046);

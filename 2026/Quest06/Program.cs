using static Library.Parsing;
using static Library.Testing;

Dictionary<string, HashSet<string>> parse(string file_name)
{
    return readFileLines(file_name)
        .Select(line => line.Split(':'))
        .ToDictionary(parts => parts[0], parts => parts[1].Split(',').ToHashSet());
}

Dictionary<string, HashSet<string>> reverse_graph(Dictionary<string, HashSet<string>> graph)
{
    var reversed = graph.Keys.ToDictionary(item => item, _ => new HashSet<string>());

    foreach (var (product, ingredients) in graph)
    {
        foreach (var ingredient in ingredients)
        {
            if (!reversed.ContainsKey(ingredient)) reversed[ingredient] = new();

            reversed[ingredient].Add(product);
        }
    }

    return reversed;
}

HashSet<string> reachable(Dictionary<string, HashSet<string>> graph, string start)
{
    var found = new HashSet<string>();
    var frontier = new Stack<string>(graph[start]);

    while (frontier.TryPop(out var ingredient))
    {
        if (ingredient == start || !found.Add(ingredient)) continue;

        if (!graph.TryGetValue(ingredient, out var neighbors)) continue;

        foreach (var neighbor in neighbors)
        {
            frontier.Push(neighbor);
        }
    }

    return found;
}

long part1(string file_name)
{
    var items = parse(file_name);
    return reachable(items, "NULL").Count;
}

long part2(string file_name)
{
    var items = reverse_graph(parse(file_name));
    return reachable(items, "NULL").Count;
}

long part3(string file_name)
{
    var items = reverse_graph(parse(file_name));
    var null_versions = items.Keys
        .Concat(items.Values.SelectMany(ingredients => ingredients))
        .Where(item => item.StartsWith("NULLv", StringComparison.Ordinal))
        .ToHashSet();

    var reachable_count = new Dictionary<string, int>();

    foreach (var null_version in null_versions)
    {
        foreach (var item in reachable(items, null_version))
        {
            reachable_count[item] = reachable_count.GetValueOrDefault(item) + 1;
        }
    }

    return reachable_count.Count(item => item.Value > 1);
}

test(part1, "part1", "Sample.txt", 7);
test(part1, "part1", "Notes1.txt", 46);

test(part2, "part2", "Sample2.txt", 6);
test(part2, "part2", "Notes2.txt", 852);

test(part3, "part3", "Sample3.txt", 3);
test(part3, "part3", "Notes3.txt", 1612);

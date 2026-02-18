using static Library.Parsing;
using static Library.Testing;

long part1(string file_name)
{
    Dictionary<string, List<string>> node_to_neighbors = new();
    Dictionary<int, List<string>> count_to_paths = new();
    var input = readFileLines(file_name);

    foreach(var line in input)
    {
        var node_and_neighbors = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var node = node_and_neighbors[0];
        var neighbors = node_and_neighbors[1].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        node_to_neighbors.Add(node, neighbors);
    }

    Stack<List<string>> paths = new();
    paths.Push(new List<string> { "RR" });

    while(paths.Count > 0)
    {
        var top = paths.Pop();
        var last = top.Last();
        if (last != "@")
        {
            foreach (var neighbor in node_to_neighbors.GetValueOrDefault(last, new()))
            {
                var new_list = top.ToList();
                new_list.Add(neighbor);
                paths.Push(new_list);
            }
        }
        else
        {
            var path = string.Join("", top);
            var length = top.Count;
            if (count_to_paths.ContainsKey(length))
            {
                count_to_paths[length].Add(path);
            }
            else
            {
                count_to_paths[length] = new List<string>(){ path };
            }
        }
    }

    var powerful_path = count_to_paths.Where(kvp => kvp.Value.Count == 1).First().Value.First();
    Console.WriteLine($"Powerful Path: '{powerful_path}'");

    return 0;
}

long part2(string file_name)
{
    Dictionary<string, List<string>> node_to_neighbors = new();
    Dictionary<int, List<string>> count_to_paths = new();
    var input = readFileLines(file_name);

    foreach (var line in input)
    {
        var node_and_neighbors = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var node = node_and_neighbors[0];
        var neighbors = node_and_neighbors[1].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        node_to_neighbors.Add(node, neighbors);
    }

    Stack<List<string>> paths = new();
    paths.Push(new List<string> { "RR" });

    while (paths.Count > 0)
    {
        var top = paths.Pop();
        var last = top.Last();
        if (last != "@")
        {
            foreach (var neighbor in node_to_neighbors.GetValueOrDefault(last, new()))
            {
                var new_list = top.ToList();
                new_list.Add(neighbor);
                paths.Push(new_list);
            }
        }
        else
        {
            var path = string.Join("", top.Select(name => name[0]));
            var length = top.Count;
            if (count_to_paths.ContainsKey(length))
            {
                count_to_paths[length].Add(path);
            }
            else
            {
                count_to_paths[length] = new List<string>() { path };
            }
        }
    }

    var powerful_path = count_to_paths.Where(kvp => kvp.Value.Count == 1).First().Value.First();
    Console.WriteLine($"Powerful Path: '{powerful_path}'");

    return 0;
}

long part3(string file_name)
{
    Dictionary<string, List<string>> node_to_neighbors = new();
    Dictionary<int, List<string>> count_to_paths = new();
    var input = readFileLines(file_name);

    foreach (var line in input)
    {
        var node_and_neighbors = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var node = node_and_neighbors[0];
        var neighbors = node_and_neighbors[1].Split(",", StringSplitOptions.RemoveEmptyEntries).ToList();
        node_to_neighbors.Add(node, neighbors);
    }

    Stack<List<string>> paths = new();
    paths.Push(new List<string> { "RR" });

    while (paths.Count > 0)
    {
        var top = paths.Pop();
        var last = top.Last();
        if (last != "@")
        {
            foreach (var neighbor in node_to_neighbors.GetValueOrDefault(last, new()))
            {
                if (!top.Contains(neighbor))
                {
                    var new_list = top.ToList();
                    new_list.Add(neighbor);
                    paths.Push(new_list);
                }
            }
        }
        else
        {
            var path = string.Join("", top.Select(name => name[0]));
            var length = top.Count;
            if (count_to_paths.ContainsKey(length))
            {
                count_to_paths[length].Add(path);
            }
            else
            {
                count_to_paths[length] = new List<string>() { path };
            }
        }
    }

    var powerful_path = count_to_paths.Where(kvp => kvp.Value.Count == 1).First().Value.First();
    Console.WriteLine($"Powerful Path: '{powerful_path}'");

    return 0;
}

test(part1, "part1", "Sample.txt", 0);
test(part1, "part1", "Notes1.txt", 0);

test(part2, "part2", "Sample.txt", 0);
test(part2, "part2", "Notes2.txt", 0);

test(part3, "part3", "Sample.txt", 0);
test(part3, "part3", "Notes3.txt", 0);

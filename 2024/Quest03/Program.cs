int min_neighbor_depth(Dictionary<(int x, int y), int> depths, (int x, int y) location)
{
    var up = depths.GetValueOrDefault((location.x, location.y - 1), 0);
    var down = depths.GetValueOrDefault((location.x, location.y + 1), 0);
    var left = depths.GetValueOrDefault((location.x - 1, location.y), 0);
    var right = depths.GetValueOrDefault((location.x + 1, location.y), 0);
    return new int[] { up, down, left, right }.Min();
}

int min_neighbor_depth_diagonals(Dictionary<(int x, int y), int> depths, (int x, int y) location)
{
    var up = depths.GetValueOrDefault((location.x, location.y - 1), 0);
    var down = depths.GetValueOrDefault((location.x, location.y + 1), 0);
    var left = depths.GetValueOrDefault((location.x - 1, location.y), 0);
    var right = depths.GetValueOrDefault((location.x + 1, location.y), 0);
    var up_left = depths.GetValueOrDefault((location.x - 1, location.y - 1), 0);
    var up_right = depths.GetValueOrDefault((location.x + 1, location.y - 1), 0);
    var down_left = depths.GetValueOrDefault((location.x - 1, location.y + 1), 0);
    var down_right = depths.GetValueOrDefault((location.x + 1, location.y + 1), 0);
    return new int[] { up, down, left, right, up_left, up_right, down_left, down_right }.Min();
}

int solve(string file_name, Func<Dictionary<(int x, int y), int>, (int x, int y), int> check_neighbors)
{
    var map = File.ReadAllText(file_name).Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries).Select(l => l.ToCharArray()).ToArray();
    Dictionary<(int x, int y), int> depths = new();

    for (int y = 0; y < map.Length; y++)
    {
        for (int x = 0; x < map[y].Length; x++)
        {
            if (map[y][x] == '#')
            {
                depths.Add((x, y), 0);
            }
        }
    }

    while (true)
    {
        HashSet<(int x, int y)> dig = new();
        foreach (var (location, depth) in depths)
        {
            if (check_neighbors(depths, location) == depth)
            {
                dig.Add(location);
            }
        }

        if (dig.Count == 0) break;

        foreach (var location in dig)
        {
            ++depths[location];
        }
    }

    return depths.Select(kvp => kvp.Value).Sum();
}

var sample = solve("Sample.txt", min_neighbor_depth_diagonals);
var part1 = solve("Notes1.txt", min_neighbor_depth);
var part2 = solve("Notes2.txt", min_neighbor_depth);
var part3 = solve("Notes3.txt", min_neighbor_depth_diagonals);

Console.WriteLine($"Sample: {sample}");
Console.WriteLine($"Part1: {part1}");
Console.WriteLine($"Part2: {part2}");
Console.WriteLine($"Part3: {part3}");
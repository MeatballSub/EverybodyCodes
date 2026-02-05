using Library;

int min_neighbor_depth(Dictionary<Library.Geometry.Point, int> depths, Library.Geometry.Point location)
{
    return location.orthogonalNeighbors().Select(n => depths.GetValueOrDefault(n, 0)).Min();
}

int min_neighbor_depth_diagonals(Dictionary<Library.Geometry.Point, int> depths, Library.Geometry.Point location)
{
    return location.diagonalNeighbors().Union(location.orthogonalNeighbors()).Select(n => depths.GetValueOrDefault(n, 0)).Min();
}

int solve(string file_name, Func<Dictionary<Library.Geometry.Point, int>, Library.Geometry.Point, int> check_neighbors)
{
    var map = Library.Parsing.readFileAsGrid(file_name);
    Dictionary<Library.Geometry.Point, int> depths = new();

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
        HashSet<Library.Geometry.Point> dig = depths.Where(kvp => check_neighbors(depths, kvp.Key) == kvp.Value).Select(kvp => kvp.Key).ToHashSet();
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
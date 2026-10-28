using Library;
using static Library.GraphExtensions;
using static Library.Optimize;
using static Library.Parsing;
using static Library.Testing;

(Graph g, char[] alloys) parse(string line)
{
    Dictionary<char, HashSet<char>> harder_than = new();
    HashSet<char> alloys = new();

    var comparisons = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);

    foreach (var comp in comparisons)
    {
        char left = comp[0];
        char op = comp[1];
        char right = comp[2];

        char harder = (op == '>') ? left : right;
        char softer = (op == '>') ? right : left;

        alloys.Add(harder);
        alloys.Add(softer);

        if (!harder_than.ContainsKey(harder)) harder_than[harder] = new HashSet<char>();
        harder_than[harder].Add(softer);
    }

    return (new(harder_than), alloys.ToArray());
}

long checksum(string line)
{
    var (g, alloys) = parse(line);

    Array.Sort(alloys, (a, b) =>
    {
        if (g.GetAllShortestPaths(a, b).Count > 0) return -1;
        if (g.GetAllShortestPaths(b, a).Count > 0) return 1;
        return 0;
    });

    string groupNumberStr = new string(alloys);
    return long.Parse(groupNumberStr);
}

long solve(string file_name)
{
    var lines = readFileLines(file_name);
    return lines.Select(line => checksum(line)).Sum();
}

test(solve, "part1", "Sample.txt", 122157);
test(solve, "part1", "Notes1.txt", 379095);

test(solve, "part2", "Sample2.txt", 122157);
test(solve, "part2", "Notes2.txt", 56807512371);

test(solve, "part3", "Sample.txt", 122157);
test(solve, "part3", "Notes3.txt", 611490845916);


class Graph : BaseGraph<char, long>
{
    public Dictionary<char, HashSet<char>> harder_than;
    public Graph(Dictionary<char, HashSet<char>> harder_than)
    {
        this.harder_than = harder_than;
    }

    public override long cost(char vertex1, char vertex2)
    {
        return 1;
    }

    public override List<char> neighbors(char vertex)
    {
        if (harder_than.TryGetValue(vertex, out var neighbors))
        {
            return neighbors.ToList();
        }
        return new();
    }
}
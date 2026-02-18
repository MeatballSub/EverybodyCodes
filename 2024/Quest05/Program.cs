using System.Collections.Concurrent;
using System.Text;
using static Library.Parsing;
using static Library.Testing;

long part1(string file_name)
{
    var solution = 0L;
    var input = readFileLines(file_name);
    List<List<long>> columns = new();
    foreach (var line in input)
    {
        var row = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        if (columns.Count < 1)
        {
            foreach (var person in row)
            {
                columns.Add(new());
            }
        }

        for (int i = 0; i < row.Count(); ++i)
        {
            columns[i].Add(row[i]);
        }
    }

    for (int round = 0; round < 10; ++round)
    {
        var clapper_column = round % columns.Count;
        var clapper = columns[clapper_column][0];
        columns[clapper_column].RemoveAt(0);
        var target_column = (round + 1) % columns.Count;
        var is_left = ((clapper - 1) % (columns[target_column].Count * 2)) < columns[target_column].Count;
        var insert_before_index = (int)((clapper - 1) % columns[target_column].Count);
        if (!is_left)
        {
            insert_before_index = columns[target_column].Count - insert_before_index;
        }
        columns[target_column].Insert(insert_before_index, clapper);
    }

    foreach (var column in columns)
    {
        solution *= 10;
        solution += column[0];
    }

    return solution;
}

long part2(string file_name)
{
    var solution = 0L;
    var input = readFileLines(file_name);
    ConcurrentDictionary<string, int> shout_counts = new();
    List<List<long>> columns = new();
    foreach (var line in input)
    {
        var row = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        if (columns.Count < 1)
        {
            foreach (var person in row)
            {
                columns.Add(new());
            }
        }

        for (int i = 0; i < row.Count(); ++i)
        {
            columns[i].Add(row[i]);
        }
    }

    for (int round = 0; true; ++round)
    {
        var clapper_column = round % columns.Count;
        var clapper = columns[clapper_column][0];
        columns[clapper_column].RemoveAt(0);
        var target_column = (round + 1) % columns.Count;
        var is_left = ((clapper - 1) % (columns[target_column].Count * 2)) < columns[target_column].Count;
        var insert_before_index = (int)((clapper - 1) % columns[target_column].Count);
        if (!is_left)
        {
            insert_before_index = columns[target_column].Count - insert_before_index;
        }
        columns[target_column].Insert(insert_before_index, clapper);
        StringBuilder shout = new();
        foreach (var column in columns)
        {
            shout.Append(column[0].ToString());
        }
        shout_counts.AddOrUpdate(shout.ToString(), 1, (_, count) => count + 1);
        if (shout_counts[shout.ToString()] == 2024)
        {
            solution = (round + 1) * long.Parse(shout.ToString());
            break;
        }
    }

    return solution;
}

long part3(string file_name)
{
    var solution = 0L;
    var input = readFileLines(file_name);
    HashSet<(List<List<long>>, int)> seen_states = new(new ListOfListsComparer());
    List<List<long>> columns = new();
    foreach (var line in input)
    {
        var row = line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
        if (columns.Count < 1)
        {
            foreach (var person in row)
            {
                columns.Add(new());
            }
        }

        for (int i = 0; i < row.Count(); ++i)
        {
            columns[i].Add(row[i]);
        }
    }

    for (int round = 0; true; ++round)
    {
        var clapper_column = round % columns.Count;
        if (!seen_states.Add((columns, clapper_column)))
        {
            break;
        }
        var clapper = columns[clapper_column][0];
        columns[clapper_column].RemoveAt(0);
        var target_column = (round + 1) % columns.Count;
        var is_left = ((clapper - 1) % (columns[target_column].Count * 2)) < columns[target_column].Count;
        var insert_before_index = (int)((clapper - 1) % columns[target_column].Count);
        if (!is_left)
        {
            insert_before_index = columns[target_column].Count - insert_before_index;
        }
        columns[target_column].Insert(insert_before_index, clapper);
        StringBuilder shout = new();
        foreach (var column in columns)
        {
            shout.Append(column[0].ToString());
        }
        solution = Math.Max(solution, long.Parse(shout.ToString()));
    }

    return solution;
}

test(part1, "part1", "Sample1.txt", 2323);
test(part1, "part1", "Notes1.txt", 2425);

test(part2, "part2", "Sample2.txt", 50877075);
test(part2, "part2", "Notes2.txt", 18031509608450);

test(part3, "part3", "Sample3.txt", 6584);
test(part3, "part3", "Notes3.txt", 9435100510031005);

public class ListOfListsComparer : IEqualityComparer<(List<List<long>>, int)>
{
    public bool Equals((List<List<long>>, int) x, (List<List<long>>, int) y)
    {
        if (x.Item2 != y.Item2) return false;

        if (x.Item1 == null && y.Item1 == null) return true;
        if (x.Item1 == null || y.Item1 == null) return false;
        if (x.Item1.Count != y.Item1.Count) return false;

        var innerComparer = new ListComparer<long>();

        for (int i = 0; i < x.Item1.Count; i++)
        {
            if (!innerComparer.Equals(x.Item1[i], y.Item1[i]))
            {
                return false;
            }
        }
        return true;
    }

    public int GetHashCode((List<List<long>>, int) obj)
    {
        if (obj.Item1 == null) return obj.Item2.GetHashCode();

        int hash = 1;
        var innerComparer = new ListComparer<long>();
        foreach (var innerList in obj.Item1)
        {
            hash = hash * 31 + innerComparer.GetHashCode(innerList);
        }
        hash = hash * 31 + obj.Item2.GetHashCode();
        return hash;
    }
}

public class ListComparer<T> : IEqualityComparer<List<T>>
{
    public bool Equals(List<T> x, List<T> y)
    {
        if (x == null && y == null) return true;
        if (x == null || y == null) return false;
        return x.SequenceEqual(y);
    }

    public int GetHashCode(List<T> obj)
    {
        if (obj == null) return 0;
        int hash = 1;
        foreach (var item in obj)
        {
            hash = hash * 31 + (item?.GetHashCode() ?? 0);
        }
        return hash;
    }
}
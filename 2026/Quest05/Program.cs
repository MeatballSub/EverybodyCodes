using static Library.Parsing;
using static Library.Testing;
using static Library.Geometry;

void grow_land(char[][] map)
{
    map[map.Length / 2][map[0].Length / 2] = '#';

    while (true)
    {
        var candidates = map.find_all('#')
            .SelectMany(point => point.allNeighbors())
            .Where(point => point.boundsCheck(map) && map.at(point) != '#')
            .ToHashSet();

        var new_land = candidates
            .Where(point => point.allNeighbors()
                .Where(neighbor => neighbor.boundsCheck(map))
                .Count(neighbor => map.at(neighbor) == '#' || map.at(neighbor) == '1') >= 5)
            .ToList();

        if (new_land.Count == 0) break;

        new_land.ForEach(point => map.at(point) = '#');
    }
}

void grow_sand(char[][] map)
{
    var coastline = map.find_all('#')
        .Where(point => point.allNeighbors()
            .Any(neighbor => !neighbor.boundsCheck(map) || map.at(neighbor) != '#'))
        .ToList();

    coastline.ForEach(point => map.at(point) = '=');

    while (true)
    {
        var candidates = map.find_all('=')
            .SelectMany(point => point.allNeighbors())
            .Where(point => point.boundsCheck(map) && map.at(point) == '#')
            .ToHashSet();

        var new_sand = candidates
            .Where(point => point.allNeighbors()
                .Where(neighbor => neighbor.boundsCheck(map))
                .Count(neighbor => map.at(neighbor) == '=') >= 5)
            .ToList();

        if (new_sand.Count == 0) break;

        new_sand.ForEach(point => map.at(point) = '=');
    }
}

void tree_stamp(char[][] map)
{
    string[] stamp = """
        .+++.
        +++++
        ++X++
        +++++
        .+++.
        """.SplitLines();

    var stamp_offsets = stamp
        .SelectMany((row, y) => row
            .Select((cell, x) => (cell, x, y)))
        .Where(stamp_cell => stamp_cell.cell == '+')
        .Select(stamp_cell => new Point(stamp_cell.x - 2, stamp_cell.y - 2))
        .ToList();

    var new_trees = map.find_all('#')
        .Where(center => stamp_offsets.All(offset =>
        {
            var point = center + offset;

            return point.boundsCheck(map)
                && (map.at(point) == '#' || map.at(point) == '@');
        }))
        .ToList();

    new_trees.ForEach(point => map.at(point) = '@');
}

void grow_forest(char[][] map)
{
    for (var step = 0; step < 2; step++)
    {
        var new_trees = map.find_all('#')
            .Where(point =>
            {
                var neighbors = point.allNeighbors()
                    .Where(neighbor => neighbor.boundsCheck(map))
                    .ToList();

                return neighbors.Count(neighbor => map.at(neighbor) == '@') >= 4
                    && !neighbors.Any(neighbor => map.at(neighbor) == '=');
            })
            .ToList();

        new_trees.ForEach(point => map.at(point) = '@');
    }
}

long part1(string file_name)
{
    var map = readFileAsGrid(file_name);

    grow_land(map);

    return map.find_all('#').Count;
}

long part2(string file_name)
{
    var map = readFileAsGrid(file_name);

    grow_land(map);
    grow_sand(map);

    return map.find_all('=').Count;
}

long part3(string file_name)
{
    var map = readFileAsGrid(file_name);

    grow_land(map);
    grow_sand(map);
    tree_stamp(map);
    grow_forest(map);

    return map.find_all('@').Count;
}

test(part1, "part1", "Sample.txt", 137);
test(part1, "part1", "Notes1.txt", 1031);

test(part2, "part2", "Sample2.txt", 69);
test(part2, "part2", "Notes2.txt", 1132);

test(part3, "part3", "Sample3.txt", 14);
test(part3, "part3", "Notes3.txt", 9534);

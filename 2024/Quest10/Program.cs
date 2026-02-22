using System.Data;
using System.Text;
using static Library.Parsing;
using static Library.Testing;

long word_power(string? runic_word) => (null == runic_word) ? 0 : runic_word.Select((ch, i) => (ch - 'A' + 1) * (i + 1)).Sum();

void part1(string file_name)
{
    var runic_word = Grid.from_lines(readFileLines(file_name)).find_runic_word();
    Console.WriteLine($"part 1 - {file_name}: '{runic_word}'");
}

long part2(string file_name)
{
    return Grid.list_from_file(file_name).SelectMany(row => row.Select(g => g.find_runic_word())).Sum(word_power);
}

long part3(string file_name)
{
    return new Wall(file_name).get_runic_words().Sum(word_power);
}

part1("Sample.txt");
part1("Notes1.txt");

Console.WriteLine();

test(part2, "part2", "Sample.txt", 1851);
test(part2, "part2", "Notes2.txt", 194289);

Console.WriteLine();

test(part3, "part3", "Sample3.txt", 3889);
test(part3, "part3", "Notes3.txt", 215899);

class Wall
{
    class Block
    {
        const int width = 8;
        const int height = 8;

        int x_offset;
        int y_offset;
        bool solvable = true;
        Wall wall;
        List<int> edge_indices = new() { 0, 1, 6, 7 };
        List<int> word_indices = new() { 2, 3, 4, 5 };
        List<char> question_marks = new() { '?' };
        List<char> used = new();

        public Block(Wall wall, int x_offset, int y_offset)
        {
            this.wall = wall;
            this.x_offset = x_offset;
            this.y_offset = y_offset;
        }

        ref char at(int x, int y)
        {
            return ref wall.symbols[y + y_offset][x + x_offset];
        }

        public bool is_solvable() => solvable;

        public string runic_word()
        {
            StringBuilder word = new();
            foreach (var y in word_indices)
            {
                foreach (var x in word_indices)
                {
                    word.Append(at(x, y));
                }
            }
            return word.ToString();
        }

        public void find_runic_symbols()
        {
            for (int y = 0; solvable && y < height; ++y)
            {
                for (int x = 0; solvable && x < width; ++x)
                {
                    if (at(x, y) == '.')
                    {
                        var row_chars = edge_indices.Select(i => at(i, y));
                        var col_chars = edge_indices.Select(i => at(x, i));
                        var intersection = row_chars.Intersect(col_chars).Except(question_marks);
                        if (intersection.Count() == 1)
                        {
                            var item = intersection.Single();
                            at(x, y) = item;
                            used.Add(item);
                        }
                        else if (intersection.Count() > 1)
                        {
                            solvable = false;
                        }
                        else if (intersection.Count() < 1)
                        {
                            var union = row_chars.Union(col_chars);
                            if (!union.Contains('?'))
                            {
                                solvable = false;
                            }
                            else
                            {
                                var unused_row_chars = row_chars.Except(used);
                                var unused_col_chars = col_chars.Except(used);
                                if (!row_chars.Contains('?') && unused_row_chars.Count() == 1)
                                {
                                    var item = unused_row_chars.Single();
                                    at(x, y) = item;
                                    used.Add(item);
                                    var question_indices = edge_indices.Where(i => at(x, i) == '?');
                                    if (question_indices.Count() == 1)
                                    {
                                        var index = question_indices.Single();
                                        at(x, index) = item;
                                    }
                                }
                                else if (!col_chars.Contains('?') && unused_col_chars.Count() == 1)
                                {
                                    var item = unused_col_chars.Single();
                                    at(x, y) = item;
                                    used.Add(item);
                                    var question_indices = edge_indices.Where(i => at(i, y) == '?');
                                    if (question_indices.Count() == 1)
                                    {
                                        var index = question_indices.Single();
                                        at(index, y) = item;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    char[][] symbols;
    List<Block> blocks = new();
    bool solvable = true;
    public Wall(string file_name)
    {
        symbols = readFileAsGrid(file_name);
        for (int y = 0; y < symbols.Length - 7; y += 6)
        {
            for (int x = 0; x < symbols[0].Length - 7; x += 6)
            {
                blocks.Add(new(this, x, y));
            }
        }
    }

    int get_symbols_hash()
    {
        unchecked
        {
            int hash = 17;
            foreach (char[] innerArray in symbols)
            {
                if (innerArray != null)
                {
                    foreach (char c in innerArray)
                    {
                        hash = hash * 31 + c.GetHashCode();
                    }
                }
            }
            return hash;
        }
    }

    public List<string> get_runic_words()
    {
        List<string> runic_words = new();
        find_runic_symbols();
        foreach (var block in blocks)
        {
            var runic_word = block.runic_word();
            if (!runic_word.Contains('.'))
            {
                runic_words.Add(runic_word);
            }
        }
        return runic_words;
    }

    void find_runic_symbols()
    {
        int? prev_hash = null;
        while (prev_hash != get_symbols_hash())
        {
            prev_hash = get_symbols_hash();
            foreach (var block in blocks)
            {
                if (block.runic_word().Contains('.') && block.is_solvable())
                {
                    block.find_runic_symbols();
                }
            }
        }
    }
}

class Grid
{
    const int width = 8;
    const int height = 8;
    public char[,] symbols = new char[height, width];

    static public Grid from_lines(string[] lines)
    {
        Grid grid = new();
        for (int y = 0; y < lines.Length; ++y)
        {
            for (int x = 0; x < lines[y].Length; ++x)
            {
                grid.symbols[y, x] = lines[y][x];
            }
        }
        return grid;
    }

    static public List<List<Grid>> list_from_file(string file_name)
    {
        List<List<Grid>> grids = new();
        var grid_rows = SplitBlankLine(file_name);
        foreach (var grid_row in grid_rows)
        {
            grids.Add(new());
            var row = grids.Last();
            List<StringBuilder> grid_strings = new();
            var lines = grid_row.SplitLines();
            foreach (var line in lines)
            {
                var sections = line.Split(' ');
                for (int i = 0; i < sections.Length; ++i)
                {
                    if (grid_strings.Count != sections.Length)
                    {
                        grid_strings.Add(new());
                    }
                    grid_strings[i].AppendLine(sections[i]);
                }
            }
            foreach (var grid_string in grid_strings)
            {
                row.Add(from_lines(grid_string.ToString().SplitLines()));
            }
        }
        return grids;
    }

    public string find_runic_word()
    {
        StringBuilder runic_word = new();
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                if (symbols[y, x] == '.')
                {
                    runic_word.Append(find_duplicate(x, y));
                }
            }
        }
        return runic_word.ToString();
    }

    char find_duplicate(int col, int row)
    {
        bool[] seen = new bool[26];
        Array.Fill(seen, false);

        for (int y = 0; y < height; ++y)
        {
            if (Char.IsLetter(symbols[y, col]))
            {
                seen[symbols[y, col] - 'A'] = true;
            }
        }

        for (int x = 0; x < width; ++x)
        {
            if (Char.IsLetter(symbols[row, x]))
            {
                if (seen[symbols[row, x] - 'A'])
                {
                    return symbols[row, x];
                }
            }
        }

        return '.';
    }
}
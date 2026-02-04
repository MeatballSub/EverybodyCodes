//#define SAMPLE

int count_words(string [] runic_words, string inscription)
{
    if (string.IsNullOrEmpty(inscription) || runic_words.Length == 0)
    {
        return 0;
    }

    var count = 0;
    for (var inscription_index = 0; inscription_index < inscription.Length; ++inscription_index)
    {
        for (var word_index = 0; word_index < runic_words.Length; ++word_index)
        {
            var runic_word = runic_words[word_index];
            if (inscription_index + runic_word.Length <= inscription.Length)
            {
                if (inscription.Substring(inscription_index, runic_word.Length) == runic_word)
                {
                    ++count;
                }
            }
        }
    }

    return count;
}

int count_symbols(string[] runic_words, string inscription)
{
    if (string.IsNullOrEmpty(inscription) || runic_words.Length == 0)
    {
        return 0;
    }

    HashSet<int> symbols = new();

    for (var inscription_index = 0; inscription_index < inscription.Length; ++inscription_index)
    {
        for (var word_index = 0; word_index < runic_words.Length; ++word_index)
        {
            var runic_word = runic_words[word_index];
            if (inscription_index + runic_word.Length <= inscription.Length)
            {
                var substring = inscription.Substring(inscription_index, runic_word.Length);
                if (substring == runic_word || substring == new string(runic_word.Reverse().ToArray()))
                {
                    for(var i = 0; i < runic_word.Length; ++i)
                    {
                        symbols.Add(inscription_index + i);
                    }
                }
            }
        }
    }

    return symbols.Count;
}

HashSet<(int, int)> search(char[][] inscription, int start_x, int start_y, int delta_x, int delta_y, string runic_word)
{
    HashSet<(int, int)> matches = new();

    for (int i = 0; i < runic_word.Length; ++i)
    {
        int check_y = start_y + delta_y * i;

        if (check_y < 0 || check_y >= inscription.Length)
        {
            break;
        }

        int check_x = (start_x + delta_x * i) % inscription[check_y].Length;
        if (check_x < 0)
        {
            check_x += inscription[check_y].Length;
        }

        if (inscription[check_y][check_x] == runic_word[i])
        {
            matches.Add((check_x, check_y));
        }
    }

    return (matches.Count == runic_word.Length) ? matches : new();
}

int count_scales(string[] runic_words, char[][] inscription)
{
    HashSet<(int, int)> scales = new();
    for (int y = 0; y < inscription.Length; ++y)
    {
        for(int x = 0;  x < inscription[y].Length; ++x)
        {
            foreach(var runic_word in runic_words)
            {
                scales.UnionWith(search(inscription, x, y, -1, 0, runic_word));
                scales.UnionWith(search(inscription, x, y, 1, 0, runic_word));
                scales.UnionWith(search(inscription, x, y, 0, -1, runic_word));
                scales.UnionWith(search(inscription, x, y, 0, 1, runic_word));
            }
        }
    }
    return scales.Count;
}

#if SAMPLE
    var input = File.ReadAllText("Sample.txt").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var runic_words = input[0].Substring(6).Split(',', StringSplitOptions.RemoveEmptyEntries);
    var inscription = input.Skip(1).Select(l => l.ToCharArray()).ToArray();
    var answer = count_scales(runic_words, inscription);
    Console.WriteLine($"Sample: {answer}");
#else
{
    var input = File.ReadAllText("Notes1.txt").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var runic_words = input[0].Substring(6).Split(',', StringSplitOptions.RemoveEmptyEntries);
    var inscription = input[1];
    var answer = count_words(runic_words, inscription);
    Console.WriteLine($"Part 1: {answer}");
}
{
    var input = File.ReadAllText("Notes2.txt").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var runic_words = input[0].Substring(6).Split(',', StringSplitOptions.RemoveEmptyEntries);
    var answer = 0;
    foreach (var inscription in input.Skip(1))
    {
        answer += count_symbols(runic_words, inscription);
    }
    Console.WriteLine($"Part 2: {answer}");
}
{
    var input = File.ReadAllText("Notes3.txt").Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
    var runic_words = input[0].Substring(6).Split(',', StringSplitOptions.RemoveEmptyEntries);
    var inscription = input.Skip(1).Select(l => l.ToCharArray()).ToArray();
    var answer = count_scales(runic_words, inscription);
    Console.WriteLine($"Part 3: {answer}");
}
#endif

//#define SAMPLE

int monster_count(string monsters)
{
    return monsters.Length - monsters.Count(c => c == 'x');
}

int monster_potions(string monsters)
{
    var potions = 0;
    foreach (char c in monsters)
    {
        potions += c switch
        {
            'B' => 1,
            'C' => 3,
            'D' => 5,
            _ => 0,
        };
    }
    return potions;
}

int potions_needed(string input, int group_size)
{
    var potions = 0;
    foreach (var group in input.Chunk(group_size))
    {
        var group_str = new string(group);
        var count = monster_count(group_str);
        potions += ((count - 1) * count) + monster_potions(group_str);
    }
    return potions;
}

#if SAMPLE
    var input = File.ReadAllText("Sample.txt");
    var answer = potions_needed(input, 3);
    Console.WriteLine($"Sample: {answer}");
#else
    foreach (var part in Enumerable.Range(1, 3))
    {
        var file = "Notes" + part + ".txt";
        var input = File.ReadAllText(file);
        var answer = potions_needed(input, part);

        Console.WriteLine($"Part {part}: {answer}");
    }
#endif
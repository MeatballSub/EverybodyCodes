using static Library.Parsing;
using static Library.Testing;

void part1(string file_name)
{
    var input = readFileLines(file_name);
    List<(string name, List<String> adjustments, long score)> plans = new();
    foreach(var line in input)
    {
        var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var plan_name = parts[0];
        var adjustments = parts[1].Split(",").ToList();
        long power = 10;
        long score = 0;
        for(int round = 0; round < 10; ++round)
        {
            var round_adjustment = adjustments[round % adjustments.Count];
            power = round_adjustment switch
            {
                "+" => power + 1,
                "-" => Math.Max(0, power - 1),
                _ => power,
            };
            score += power;
        }
        plans.Add((plan_name, adjustments, score));
    }

    var ranking = String.Join("", plans.OrderByDescending(p => p.score).Select(p => p.name));
    Console.WriteLine(ranking);
}

void part2(string file_name)
{
    var track = "-=++=-==++=++=-=+=-=+=+=--=-=++=-==++=-+=-=+=-=+=+=++=-+==++=++=-=-=---=++==--+++==++=+=--==++==+++=++=+++=--=+=-=+=-+=-+=-+-=+=-=+=-+++=+==++++==---=+=+=-S";
    var input = readFileLines(file_name);
    List<(string name, List<String> adjustments, long score)> plans = new();
    foreach (var line in input)
    {
        var parts = line.Split(":", StringSplitOptions.RemoveEmptyEntries);
        var plan_name = parts[0];
        var adjustments = parts[1].Split(",").ToList();
        long power = 10;
        long score = 0;
        int round = 0;
        for(int lap = 0; lap < 10; ++lap)
        {
            for (int segment = 0; segment < track.Length; ++segment)
            {
                var track_adjustment = track[segment];
                var round_adjustment = adjustments[round % adjustments.Count];
                power = track_adjustment switch
                {
                    '+' => power + 1,
                    '-' => Math.Max(0, power - 1),
                    _ => round_adjustment switch
                    {
                        "+" => power + 1,
                        "-" => Math.Max(0, power - 1),
                        _ => power,
                    }
                };
                score += power;
                ++round;
            }
        }
        plans.Add((plan_name, adjustments, score));
    }

    var ranking = String.Join("", plans.OrderByDescending(p => p.score).Select(p => p.name));
    Console.WriteLine(ranking);
}

void get_combos(HashSet<String> combinations, char[] plan, int i, int j)
{
    if(i == j)
    {
        combinations.Add(new String(plan));
        return;
    }

    for(int k = i; k <= j; ++k)
    {
        var temp = plan[k];
        plan[k] = plan[i];
        plan[i] = temp;

        get_combos(combinations, plan, i + 1, j);

        temp = plan[k];
        plan[k] = plan[i];
        plan[i] = temp;
    }
}

List<String> get_combinations(string structure)
{
    HashSet<String> combinations = new();
    get_combos(combinations, structure.ToCharArray(), 0, structure.Length - 1);
    return combinations.ToList();
}

void part3(string file_name)
{
    //53822750616
    var track = "+=+++===-+++++=-==+--+=+===-++=====+--===++=-==+=++====-==-===+=+=--==++=+========-=======++--+++=-++=-+=+==-=++=--+=-====++--+=-==++======+=++=-+==+=-==++=-=-=---++=-=++==++===--==+===++===---+++==++=+=-=====+==++===--==-==+++==+++=++=+===--==++--===+=====-=++====-+=-+--=+++=-+-===++====+++--=++====+=-=+===+=====-+++=+==++++==----=+=+=-S";
    long rival_score = 0;
    {
        var input = readFileLines(file_name);

        var parts = input[0].Split(":", StringSplitOptions.RemoveEmptyEntries);
        var plan_name = parts[0];
        var adjustments = parts[1].Split(",").ToList();
        long power = 10;
        int round = 0;
        for (int lap = 0; lap < 2024; ++lap)
        {
            for (int segment = 0; segment < track.Length; ++segment)
            {
                var track_adjustment = track[segment];
                var round_adjustment = adjustments[round % adjustments.Count];
                power = track_adjustment switch
                {
                    '+' => power + 1,
                    '-' => Math.Max(0, power - 1),
                    _ => round_adjustment switch
                    {
                        "+" => power + 1,
                        "-" => Math.Max(0, power - 1),
                        _ => power,
                    }
                };
                rival_score += power;
                ++round;
            }
        }
    }

    String plan_structure = "+++++---===";
    List<String> plans = get_combinations(plan_structure);
    long wins = 0;
    foreach(var plan in plans)
    {
        long power = 10;
        long score = 0;
        int round = 0;
        for (int lap = 0; lap < 2024; ++lap)
        {
            for (int segment = 0; segment < track.Length; ++segment)
            {
                var track_adjustment = track[segment];
                var round_adjustment = plan[round % plan.Length];
                power = track_adjustment switch
                {
                    '+' => power + 1,
                    '-' => Math.Max(0, power - 1),
                    _ => round_adjustment switch
                    {
                        '+' => power + 1,
                        '-' => Math.Max(0, power - 1),
                        _ => power,
                    }
                };
                score += power;
                ++round;
            }
        }
        if (score > rival_score) wins++;
    }

    Console.WriteLine($"wins = {wins}");

}

part1("Sample.txt");
part1("Notes1.txt");

part2("Sample.txt");
part2("Notes2.txt");

part3("Notes3.txt");
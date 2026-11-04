using static Library.Parsing;
using static Library.Testing;

(Dictionary<long, long> energy, Dictionary<long, List<long>> connections) parse(string file_name)
{
    var clouds = readFileLines(file_name)
        .Select(line => line.ExtractLongs().ToArray())
        .ToList();

    var energy = clouds.ToDictionary(cloud => cloud[0], cloud => cloud[1]);
    var connections = clouds.ToDictionary(cloud => cloud[0], cloud => cloud.Skip(2).ToList());

    return (energy, connections);
}

long find_starting_cloud(Dictionary<long, List<long>> connections)
{
    return connections.MaxBy(cloud => cloud.Value.Count).Key;
}

Dictionary<long, long> propagate_impulse(
    Dictionary<long, long> energy,
    Dictionary<long, List<long>> connections,
    long starting_cloud,
    long strength)
{
    var updated_energy = new Dictionary<long, long>(energy);
    var frontier = new Queue<(long cloud, long strength)>();
    frontier.Enqueue((starting_cloud, strength));

    while (frontier.TryDequeue(out var impulse))
    {
        updated_energy[impulse.cloud] += impulse.strength;

        if (impulse.strength <= 1 || !connections.TryGetValue(impulse.cloud, out var outgoing)) continue;

        foreach (var cloud in outgoing)
        {
            frontier.Enqueue((cloud, impulse.strength - 1));
        }
    }

    return updated_energy;
}

Dictionary<long, long> propagate_with_explosions(
    Dictionary<long, long> energy,
    Dictionary<long, List<long>> connections,
    long starting_cloud,
    long strength,
    long explosion_limit)
{
    var remaining_energy = new Dictionary<long, long>(energy);
    var exploded = new HashSet<long>();
    var stage = new Dictionary<(long cloud, long strength), long>
    {
        [(starting_cloud, strength)] = 1
    };

    while (stage.Count > 0)
    {
        var next_stage = new Dictionary<(long cloud, long strength), long>();

        foreach (var (impulse, count) in stage)
        {
            var (cloud, impulse_strength) = impulse;
            if (exploded.Contains(cloud)) continue;

            remaining_energy[cloud] += impulse_strength * count;

            if (impulse_strength <= 1 || !connections.TryGetValue(cloud, out var outgoing)) continue;

            foreach (var target in outgoing)
            {
                var next_impulse = (target, impulse_strength - 1);
                next_stage[next_impulse] = next_stage.GetValueOrDefault(next_impulse) + count;
            }
        }

        var exploded_clouds = remaining_energy
            .Where(cloud => cloud.Value >= explosion_limit)
            .Select(cloud => cloud.Key)
            .ToList();

        foreach (var cloud in exploded_clouds)
        {
            exploded.Add(cloud);
            remaining_energy.Remove(cloud);

            if (!connections.TryGetValue(cloud, out var outgoing)) continue;

            foreach (var target in outgoing)
            {
                var explosion_impulse = (target, 10);
                next_stage[explosion_impulse] = next_stage.GetValueOrDefault(explosion_impulse) + 1;
            }
        }

        stage = next_stage;
    }

    return remaining_energy;
}

long max_remaining_energy(
    Dictionary<long, long> energy,
    Dictionary<long, List<long>> connections,
    long explosion_limit)
{
    return energy.Keys
        .Select(starting_cloud => propagate_with_explosions(
            energy, connections, starting_cloud, 10, explosion_limit).Values.Sum())
        .Max();
}

long part1(string file_name)
{
    var (energy, connections) = parse(file_name);
    var starting_cloud = find_starting_cloud(connections);
    var final_energy = propagate_impulse(energy, connections, starting_cloud, 5);

    return final_energy.Values.Sum();
}

long part2(string file_name)
{
    var (energy, connections) = parse(file_name);
    var starting_cloud = find_starting_cloud(connections);
    var final_energy = propagate_impulse(energy, connections, starting_cloud, 10);

    return final_energy.Values.Sum();
}

long part3sample(string file_name)
{
    var (energy, connections) = parse(file_name);

    return max_remaining_energy(energy, connections, 100);
}

long part3(string file_name)
{
    var (energy, connections) = parse(file_name);

    return max_remaining_energy(energy, connections, 1000000);
}

test(part1, "part1", "Sample.txt", 102);
test(part1, "part1", "Notes1.txt", 275);

test(part2, "part2", "Sample2.txt", 1316);
test(part2, "part2", "Sample2b.txt", 1214633);
test(part2, "part2", "Notes2.txt", 183073821);

test(part3sample, "part3sample", "Sample3.txt", 187);
test(part3, "part3", "Notes3.txt", 28564573);

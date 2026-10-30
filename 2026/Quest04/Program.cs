using System.Text.RegularExpressions;
using static Library.Testing;

(long id, long quest, long part, long time) parse_line(string line)
{
    var regex = new Regex(@"id=(?<id>\d+).*?quest=(?<quest>\d+).*?part=(?<part>\d+).*?time=(?<time>\d+)", RegexOptions.Compiled);
    var match = regex.Match(line);
    long id = long.Parse(match.Groups["id"].Value);
    long quest = long.Parse(match.Groups["quest"].Value);
    long part = long.Parse(match.Groups["part"].Value);
    long time = long.Parse(match.Groups["time"].Value);
    return (id, quest, part, time);
}

IEnumerable<(long id, long quest, long part, long time)> parse(string file_name)
{
    return File.ReadAllLines(file_name).Select(parse_line);    
}

long checksum(List<long> times)
{
    long checksum = 0;
    long curr_place = 0;
    long prev_points = 0;
    long tie_count = 0;
    foreach (var time in times)
    {
        if (prev_points == time)
        {
            tie_count += 1;
        }
        else
        {
            curr_place += tie_count + 1;
            tie_count = 0;
            prev_points = time;
        }
        checksum += time * curr_place;
    }

    return checksum;
}

long part1(string file_name)
{
    var sorted_times = parse(file_name)
        .GroupBy(result => result.id)
        .Select(group => group.Sum(result => result.time))
        .OrderBy(time => time)
        .ToList();

    return checksum(sorted_times);
}

long part2(string file_name)
{
    var sorted_times = parse(file_name)
        .GroupBy(result => result.id)
        .Select(group => (Parts: group.Count(), Times: group.Sum(r => r.time)))
        .OrderByDescending(result => result.Parts)
        .ThenBy(result => result.Times)
        .Select(result=> result.Times)
        .ToList();

    return checksum(sorted_times);
}

long part3(string file_name)
{
    long[] part_max = [0, 50, 100, 150];

    Dictionary<long, long> id_points = new();
    Dictionary<(long, long), long> quest_times = new();

    var sorted = parse(file_name).OrderBy(result => result.id).ThenBy(result => result.quest).ThenBy(result => result.part).ToList();
    List<(long id, long quest, long part, long time)> new_sorted = new();
    long curr_time = 0;
    long prev_id = 0;
    long prev_quest = 0;
    foreach(var result in sorted)
    {
        if(result.id != prev_id)
        {
            prev_id = result.id;
            prev_quest = result.quest;
            curr_time = result.time;
        }
        else if(result.quest != prev_quest)
        {
            prev_quest = result.quest;
            curr_time = result.time;
        }
        else
        {
            curr_time += result.time;
        }
        new_sorted.Add((result.id, result.quest, result.part, curr_time));
    }

    var sorted_results = new_sorted
        .OrderBy(r => r.quest)
        .ThenBy(r => r.part)
        .ThenBy(r => r.time)
        .ToList();

    long curr_quest = 0;
    long curr_part = 0;
    long curr_points = 0;
    long tie_count = 0;
    long prev_time = 0;
    foreach (var result in sorted_results)
    {
        if (result.quest != curr_quest)
        {
            curr_quest = result.quest;
            curr_part = result.part;
            curr_points = part_max[curr_part];
            prev_time = result.time;
            tie_count = 0;
        }
        else if (result.part != curr_part)
        {
            curr_part = result.part;
            curr_points = part_max[curr_part];
            prev_time = result.time;
            tie_count = 0;
        }
        else if(result.time != prev_time)
        {
            curr_points = curr_points - tie_count - 1;
            prev_time = result.time;
            tie_count = 0;
        }
        else
        {
            tie_count += 1;
        }
        if (!id_points.ContainsKey(result.id))
        {
            id_points[result.id] = 0;
        }
        id_points[result.id] += Math.Max(curr_points,0);
    }

    var sorted_points = id_points.Values.OrderByDescending(points => points).ToList();

    return checksum(sorted_points);
}

test(part1, "part1", "Sample.txt", 27598);
test(part1, "part1", "Notes1.txt", 176205);

test(part2, "part2", "Sample2.txt", 6470);
test(part2, "part2", "Notes2.txt", 621307560);

test(part3, "part3", "Sample3.txt", 2998);
test(part3, "part3", "Sample3b.txt", 3004);
test(part3, "part3", "Sample3c.txt", 6232480);
test(part3, "part3", "Notes3.txt", 6303118);

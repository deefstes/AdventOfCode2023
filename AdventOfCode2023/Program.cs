using AdventOfCode2023;
using AdventOfCode2023.Utils;
using System.Reflection;

string? userInput = null;
if (string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SOLVE_CURRENT_DAY")))
{
    Console.WriteLine("Which day do you want to solve? (1-25):");
    userInput = Console.ReadLine();
}

if (String.IsNullOrEmpty(userInput))
    userInput = DateTime.Now.Day.ToString();

if (userInput == "x")
{
    Experiment.Run();
    return;
}

if (!int.TryParse(userInput, out var day))
{
    Console.WriteLine("Incorrect day specified");
    return;
}

var solverType = Assembly
    .GetEntryAssembly()!
    .GetTypes()
    .Where(t => t.GetTypeInfo().IsClass && typeof(ISolver).IsAssignableFrom(t))
    .FirstOrDefault(t => t.FullName!.Split('.')[1] == $"Day{day:D2}");

if (solverType == null)
{
    Console.WriteLine($"No solver found for day {day}", day);
    return;
}

var solver = Activator.CreateInstance(solverType) as ISolver;

if (solver == null)
{
    Console.WriteLine("Could not instantiate solver");
    return;
}

var fileInput = File.ReadAllText($"Day{day:D2}\\input.txt");

var p1 = Utils.MeasureExecutionTime(() => solver.Part1(fileInput));
var p2 = Utils.MeasureExecutionTime(() => solver.Part2(fileInput));

Console.WriteLine();
Console.WriteLine($"Solution for day {day}");
Console.WriteLine($"Part 1 ({p1.Item2.Format()}): {p1.Item1}");
Console.WriteLine($"Part 2 ({p2.Item2.Format()}): {p2.Item1}");

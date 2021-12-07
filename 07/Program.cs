using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .SelectMany(x => x.Split(','))
    .Where(x => !String.IsNullOrWhiteSpace(x))
    .Select(long.Parse)
    .ToArray();


long GetSolution(Func<long, long> costFunction)
{
    var maxPosition = input.Max();
    var cost = new long[maxPosition + 1];

    for (int position = 0; position <= maxPosition; position++)
        foreach (var value in input)
        {
            var distance = Math.Abs(value - position);
            cost[position] += costFunction(distance);
        }

    return cost.Min();
}


long GetAnswer1() => GetSolution(distance => distance);

long GetAnswer2() => GetSolution(
    // Sum of first n natural numbers; (n* (n+1) ) /2    
    distance => (distance * (distance + 1)) / 2
);


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");

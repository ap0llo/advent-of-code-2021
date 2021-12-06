using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .SelectMany(x => x.Split(','))
    .Where(x => !String.IsNullOrWhiteSpace(x))
    .Select(long.Parse)
    .ToArray();

long SimulateGrowth(int days)
{
    // get a array with the number of fish of each age
    var state = new long[9];
    foreach (var value in input)
    {
        state[value] += 1;
    }

    // simulate the specified number of days
    for (int day = 1; day <= days; day++)
    {        
        // save number of fish with age 0
        var zeroValue = state[0];

        // decrease counters for all fish with age >= 1 (= move values in array down one index)
        for (int age = 1; age < state.Length; age++)
        {
            state[age - 1] = state[age];
        }

        // no need do update newState[0] since it was overwritten in the loop above

        state[8] = zeroValue;
        state[6] += zeroValue;
    }

    return state.Sum();
}

long GetAnswer1() => SimulateGrowth(80);

long GetAnswer2() => SimulateGrowth(256);


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .SelectMany(x => x.Split(','))
    .Where(x => !String.IsNullOrWhiteSpace(x))
    .Select(long.Parse)
    .ToArray();

long SimulateGrowth(long[] initialState, int days)
{
    // all fish are between 0 and 8 days old
    var ages = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 };

    // get a dictionary that maps the number of fish for each day
    var currentState = ages.ToDictionary(age => (long)age, age => 0L);
    foreach (var value in initialState)
    {
        currentState[value] += 1;
    }

    // simulate the specified number of days
    for (int day = 1; day <= days; day++)
    {
        // copy dictionary so we can modify it while retaining the original data 
        // (fish added while evaluationg the current day are not considered for the current day)
        var newState = new Dictionary<long, long>(currentState);

        foreach (var age in ages)
        {
            var currentCount = currentState.GetValueOrDefault(age);

            // for fish of age 0, add the same number of fish with age 8 and reset them to age 6
            if (age == 0)
            {
                newState[0] -= currentCount;
                newState[6] += currentCount;
                newState[8] += currentCount;
            }
            // for all other fish, decrease their age by 1
            else
            {
                newState[age] -= currentCount;
                newState[age - 1] += currentCount;
            }
        }

        // replace the current state with the new one
        currentState = newState;
    }

    return currentState.Values.Sum();
}

long GetAnswer1() => SimulateGrowth(input, 80);

long GetAnswer2() => SimulateGrowth(input, 256);


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");

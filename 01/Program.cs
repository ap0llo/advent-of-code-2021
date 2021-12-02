using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(x => !String.IsNullOrEmpty(x))
    .Select(int.Parse)
    .ToArray();

int GetPart1()
{

    int numberOfIncreases = 0;
    int? previousValue = null;
    foreach (var value in input)
    {
        if (previousValue is null)
        {
            Console.WriteLine("(N/A - no previous measurement)");
        }
        else if (previousValue.Value == value)
        {
            Console.WriteLine("(No Change)");
        }
        else if (previousValue.Value < value)
        {
            Console.WriteLine("(increased)");
            numberOfIncreases += 1;
        }
        else
        {
            Console.WriteLine("(decreased)");
        }
        previousValue = value;
    }

    return numberOfIncreases;
}

int GetPart2()
{
    int numberOfIncreases = 0;
    int? previousSum = null; 

    for(int i = 0; i < input.Length - 2; i++)
    {
        var currentSum = input[i] + input[i + 1] + input[i + 2];

        if (previousSum is not null && currentSum > previousSum.Value)
            numberOfIncreases += 1;

        previousSum = currentSum;
    }


    return numberOfIncreases;

}


var part1 = GetPart1();
var part2 = GetPart2();
Console.WriteLine($"Answer 1: {part1}");
Console.WriteLine($"Answer 2: {part2}");
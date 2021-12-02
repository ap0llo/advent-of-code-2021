using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(x => !String.IsNullOrEmpty(x))
    .ToArray();

int GetPart1()
{
    int depth = 0;
    int horizontalPosition = 0;
  

    foreach(var line in input)
    {
        var split = line.Split(' ');
        var command = split[0];
        var arg = int.Parse(split[1]);


        switch (command)
        {
            case "forward":
                horizontalPosition += arg;
                break;

            case "down":
                depth += arg;
                break;

            case "up":
                depth -= arg;
                break;

            default:
                throw new NotImplementedException();
        }

    }

    return depth * horizontalPosition;
}

int GetPart2()
{
    int depth = 0;
    int horizontalPosition = 0;
    int aim = 0;

    foreach (var line in input)
    {
        var split = line.Split(' ');
        var command = split[0];
        var x = int.Parse(split[1]);

        switch (command)
        {
            case "forward":
                horizontalPosition += x;
                depth += (aim * x);
                break;

            case "down":
                aim += x;
                break;

            case "up":
                aim -= x;
                break;

            default:
                throw new NotImplementedException();
        }

    }

    return depth * horizontalPosition;
}


var part1 = GetPart1();
Console.WriteLine($"Answer 1: {part1}");
var part2 = GetPart2();
Console.WriteLine($"Answer 2: {part2}");
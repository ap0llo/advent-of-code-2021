using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(ParseInputLine)
    .ToArray();


InputLine ParseInputLine(string line)
{
    var parts = line.Split('|');

    var signalPatterns = parts[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToHashSet()).ToArray();
    var outputValue = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(x => x.ToHashSet()).ToArray();

    return new(signalPatterns, outputValue);
}


long GetAnswer1()
{
    var occurrences = new long[10];

    foreach (var record in input)
    {
        foreach (var outputValue in record.OutputValue)
        {
            var number = outputValue.Count switch
            {
                2 => 1,
                4 => 4,
                3 => 7,
                7 => 8,
                _ => -1
            };

            if (number >= 0)
                occurrences[number] += 1;
        }
    }

    return occurrences.Sum();
}


var commonSegmentTable = GetCommonSegmentTable();

/// <summary>
/// For a 7-segment display, calculate the number of common segments for each combination of possible values
/// </summary>
int[,] GetCommonSegmentTable()
{

    // Assuming the seven segments are labeld a-g with the following pattern
    //
    //  aaaa  
    // b    c 
    // b    c 
    //  dddd  
    // e    f 
    // e    f 
    //  gggg  
    //
    // the following segments are active when displaying the numbers 0-9:

    var patterns = new string[10];
    patterns[0] = "abcefg";
    patterns[1] = "cf";
    patterns[2] = "acdeg";
    patterns[3] = "acdfg";
    patterns[4] = "bcdf";
    patterns[5] = "abdfg";
    patterns[6] = "abdefg";
    patterns[7] = "acf";
    patterns[8] = "abcdefg";
    patterns[9] = "abcdfg";

    // Calculate the table containing the number of common segments 
    // for each combination of two numbers

    var commonSegments = new int[10, 10];
    for (int i = 0; i < patterns.Length; i++)
        for (int j = 0; j < patterns.Length; j++)
            commonSegments[i, j] = patterns[i].Intersect(patterns[j]).Distinct().Count();

    return commonSegments;
}


int GetIntersectSize(HashSet<char> left, HashSet<char> right) => left.Intersect(right).Distinct().Count();

/// <summary>
/// Based on the already known patterns, determines the signal pattern for the specified value
/// by comparing the number of common segments between a signal and the known patterns to the 
/// expected number of common segments in the "commonSegmentTable"
/// </summary>
HashSet<char> FindPatternForValue(InputLine record, HashSet<char>[] knownPatterns, int value)
{
    // There should be a single signal matching the value
    return record.SignalPatterns.Single(pattern =>
    {
        var isMatch = true;
        for (int i = 0; i < knownPatterns.Length; i++)
        {
            if (knownPatterns[i] is null)
                continue;

            var overlap = GetIntersectSize(pattern, knownPatterns[i]);

            if (overlap != commonSegmentTable![value, i])
                isMatch = false;
        }

        return isMatch;
    });
}

long GetValue(InputLine record)
{

    var knownPatterns = new HashSet<char>[10];

    // 2 signals correspond to a value of 1
    knownPatterns[1] = record.SignalPatterns.Single(x => x.Count == 2);

    // 4 signals correspond to a value of 4
    knownPatterns[4] = record.SignalPatterns.Single(x => x.Count == 4);

    // 3 signals correspond to a value of 7
    knownPatterns[7] = record.SignalPatterns.Single(x => x.Count == 3);

    // 7 signals correspond to a value of 8
    knownPatterns[8] = record.SignalPatterns.Single(x => x.Count == 7);

    // patterns with 5 signals can correspond to values of either 2, 3 or 5        
    knownPatterns[2] = FindPatternForValue(record, knownPatterns, 2);
    knownPatterns[3] = FindPatternForValue(record, knownPatterns, 3);
    knownPatterns[5] = FindPatternForValue(record, knownPatterns, 5);

    // patterns with 6 signals can correspond to values of either 0, 6 or 9
    knownPatterns[0] = FindPatternForValue(record, knownPatterns, 0);
    knownPatterns[6] = FindPatternForValue(record, knownPatterns, 6);
    knownPatterns[9] = FindPatternForValue(record, knownPatterns, 9);

    // decode output value
    var decodedValues = 0;
    foreach (var value in record.OutputValue)
    {
        for (int i = 0; i < knownPatterns.Length; i++)
        {
            // if the current pattern matches the value, i is the digit value at that position:
            // Multiply the decoded value by 10 (we're iterating from most to least ignificant digits)
            if (knownPatterns[i].Count == value.Count && GetIntersectSize(value, knownPatterns[i]) == value.Count)
            {
                decodedValues *= 10;
                decodedValues += i;
                break;
            }
        }
    }

    return decodedValues;
}


long GetAnswer2() => input.Sum(GetValue);


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");


record InputLine(IReadOnlyList<HashSet<char>> SignalPatterns, IReadOnlyList<HashSet<char>> OutputValue);
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var inputStrings = File.ReadAllLines("input.txt").Where(x => !String.IsNullOrEmpty(x)).ToArray();
var maxInputLength = inputStrings.Max(x => x.Length);
var input = inputStrings.Select(str => Convert.ToInt64(str, 2)).ToArray();

long GetAnswer1()
{
    var gamma = 0L;
    var epsilon = 0L;

    for (var bitIndex = maxInputLength - 1; bitIndex >= 0; bitIndex--)
    {
        gamma <<= 1;
        epsilon <<= 1;

        var (numberOfOnes, numberOfZeros) = CountBits(input, bitIndex);

        if (numberOfOnes > numberOfZeros)
        {
            gamma += 1;
        }
        else
        {
            epsilon += 1;
        }
    }

    return gamma * epsilon;
}

long GetAnswer2()
{
    var oxygenGeneratorRating = 0L;
    {
        var remainingValues = input;
        for (int bitIndex = maxInputLength - 1; bitIndex >= 0; bitIndex--)
        {
            var (numberOfOnes, numberOfZeros) = CountBits(remainingValues, bitIndex);
            var mostCommonValue = numberOfOnes >= numberOfZeros ? 1 : 0;
            remainingValues = remainingValues.Where(x => GetBitAt(x, bitIndex) == mostCommonValue).ToArray();

            if (remainingValues.Length == 1)
            {
                oxygenGeneratorRating = remainingValues[0];
                break;
            }
        }
    }

    var co2ScrubberRating = 0L;
    {
        var remainingValues = input;
        for (int bitIndex = maxInputLength - 1; bitIndex >= 0; bitIndex--)
        {
            var (numberOfOnes, numberOfZeros) = CountBits(remainingValues, bitIndex);
            var leastCommonValue = numberOfZeros <= numberOfOnes ? 0 : 1;
            remainingValues = remainingValues.Where(x => GetBitAt(x, bitIndex) == leastCommonValue).ToArray();

            if (remainingValues.Length == 1)
            {
                co2ScrubberRating = remainingValues[0];
                break;
            }
        }
    }

    return oxygenGeneratorRating * co2ScrubberRating;
}

(int ones, int zeros) CountBits(IReadOnlyList<long> values, int bitIndex)
{
    var ones = values.Select(x => GetBitAt(x, bitIndex)).Sum();
    var zeros = values.Count - ones;

    return (ones, zeros);
}

int GetBitAt(long value, int index)
{
    var mask = 0b1 << index;
    return (value & mask) == 0 ? 0 : 1;
}


var firstAnswer = GetAnswer1();
Console.WriteLine($"Answer 1: {firstAnswer}");
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 2: {secondAnswer}");
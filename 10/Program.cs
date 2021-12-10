using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

var closingCharacters = new Dictionary<char, char>()
{
    { '(', ')' },
    { '[', ']' },
    { '{', '}' },
    { '<', '>' },
};


var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(ParseLine)
    .ToArray();

Line ParseLine(string inputLine)
{
    var openingChars = new Stack<char>();
    foreach (var c in inputLine)
    {
        switch (c)
        {
            case '(':
            case '[':
            case '{':
            case '<':
                openingChars.Push(c);
                break;

            case ')':
            case ']':
            case '}':
            case '>':
                var expectedChar = closingCharacters[openingChars.Peek()];
                if (c == expectedChar)
                {
                    openingChars.Pop();
                }
                else
                {
                    Console.WriteLine($"Line '{inputLine}': Expected '{expectedChar}' but found '{c}' instead");
                    return Line.Invalid(inputLine, c);
                }
                break;

            default:
                throw new NotImplementedException();
        }

    }

    if (openingChars.Count == 0)
    {
        return Line.Valid(inputLine);
    }
    else
    {
        var missingCharactersBuilder = new StringBuilder();
        while (openingChars.Count > 0)
        {
            missingCharactersBuilder.Append(closingCharacters[openingChars.Pop()]);
        }

        var missingCharacters = missingCharactersBuilder.ToString();
        Console.WriteLine($"Line '{inputLine}': Incomplete, missing '{missingCharacters}'.");
        return Line.Incomplete(inputLine, missingCharacters);
    }
}

long GetAnswer1()
{
    var points = new Dictionary<char, long>()
    {
        { ')', 3 },
        { ']', 57 },
        { '}', 1197 },
        { '>', 25137 }
    };

    return input
        .Where(x => x.State == LineState.Invalid)
        .Select(x => points[x.InvalidCharacter!.Value])
        .Sum();
}

long GetAnswer2()
{
    var points = new Dictionary<char, int>()
    {
        { ')', 1 },
        { ']', 2 },
        { '}', 3 },
        { '>', 4 }
    };

    var lineScores = input
        .Where(x => x.State == LineState.Incomplete)
        .Select(x =>
        {
            var lineScore = 0L;
            foreach (var c in x.MissingCharacters!)
            {
                lineScore *= 5;
                lineScore += points[c];
            }
            return lineScore;
        })
        .ToArray();

    return lineScores.OrderBy(x => x).Skip(lineScores.Length / 2).First();
}


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");


enum LineState
{
    Unknown = 0,
    Valid = 1,
    Invalid = 2,
    Incomplete = 3
}


record Line
{
    public string Value { get; }

    public LineState State { get; }

    public char? InvalidCharacter { get; }

    public string? MissingCharacters { get; }


    private Line(string value, LineState state, char? invalidCharacter, string? missingCharacters)
    {
        Value = value;
        State = state;
        InvalidCharacter = invalidCharacter;
        MissingCharacters = missingCharacters;
    }


    public static Line Valid(string value) => new Line(value, LineState.Valid, null, null);

    public static Line Invalid(string value, char invalidCharacter) => new Line(value, LineState.Invalid, invalidCharacter, missingCharacters: null);

    public static Line Incomplete(string value, string missingCharacters) => new Line(value, LineState.Incomplete, invalidCharacter: null, missingCharacters);
}


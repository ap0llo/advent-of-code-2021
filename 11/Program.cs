using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray())
    .ToArray();


IEnumerable<Point> EnumerateAllPoints(OctopusMap data)
{
    for (int x = 0; x < data.Width; x++)
    {
        for (int y = 0; y < data.Height; y++)
        {
            yield return new(x, y);
        }
    }
}

IEnumerable<Point> GetAdjacentPoints(OctopusMap data, Point p)
{
    // left
    if (p.X > 0)
        yield return new(p.X - 1, p.Y);

    //right
    if (p.X < data.Width - 1)
        yield return new(p.X + 1, p.Y);

    // top
    if (p.Y > 0)
        yield return new(p.X, p.Y - 1);

    // bottom
    if (p.Y < data.Height - 1)
        yield return new(p.X, p.Y + 1);


    // top left
    if (p.X > 0 && p.Y > 0)
        yield return new(p.X - 1, p.Y - 1);

    // top right
    if (p.X < data.Width - 1 && p.Y > 0)
        yield return new Point(p.X + 1, p.Y - 1);

    // bottom left
    if (p.X > 0 && p.Y < data.Height - 1)
        yield return new Point(p.X - 1, p.Y + 1);

    // bottom right
    if (p.X < data.Width - 1 && p.Y < data.Height - 1)
        yield return new Point(p.X + 1, p.Y + 1);
}

HashSet<Point> SimulateStep(OctopusMap currentState)
{
    var flashedOctopuses = new HashSet<Point>();
    var octopusesToFlash = new HashSet<Point>();

    foreach (var p in EnumerateAllPoints(currentState))
    {
        currentState[p] += 1;

        if (currentState[p] > 9)
            octopusesToFlash.Add(p);
    }

    while (octopusesToFlash.Count > 0)
    {
        foreach (var p in octopusesToFlash.ToArray())
        {
            flashedOctopuses.Add(p);
            octopusesToFlash.Remove(p);

            foreach (var adjacentPoint in GetAdjacentPoints(currentState, p))
            {
                currentState[adjacentPoint] += 1;
                if (currentState[adjacentPoint] > 9 && !flashedOctopuses.Contains(adjacentPoint))
                {
                    octopusesToFlash.Add(adjacentPoint);
                }
            }
        }
    }

    foreach (var p in flashedOctopuses)
    {
        currentState[p] = 0;
    }


    return flashedOctopuses;
}


long GetAnswer1(int steps)
{
    var currentState = new OctopusMap(data);

    var totalFlashes = 0L;

    for (int i = 0; i < steps; i++)
    {
        var flashedOctopuses = SimulateStep(currentState);
        totalFlashes += flashedOctopuses.Count;
    }

    return totalFlashes;
}

long GetAnswer2()
{
    var currentState = new OctopusMap(data);

    for (int i = 0; ; i++)
    {
        var flashedOctopuses = SimulateStep(currentState);

        if (flashedOctopuses.Count == currentState.Height * currentState.Width)
        {
            return i + 1;
        }
    }
}

var firstAnswer = GetAnswer1(100);
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");



record Point(int X, int Y);

record OctopusMap
{
    public int[][] Data { get; }

    public int Height => Data.Length;

    public int Width => Data[0].Length;

    public int this[Point p]
    {
        get => Data[p.Y][p.X];
        set => Data[p.Y][p.X] = value;
    }

    public OctopusMap(int[][] data)
    {
        Data = data.Select(x => x.ToArray()).ToArray();
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var data = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(x => x.ToCharArray().Select(x => int.Parse(x.ToString())).ToArray())
    .ToArray();

var input = new HeightMap(data);


IEnumerable<Point> EnumerateAllPoints()
{
    for (int x = 0; x < input.Width; x++)
    {
        for (int y = 0; y < input.Height; y++)
        {
            yield return new(x, y);
        }
    }
}

IEnumerable<Point> GetAdjacentPoints(Point p)
{
    // left
    if (p.X > 0)
        yield return new(p.X - 1, p.Y);

    //right
    if (p.X < input.Width - 1)
        yield return new(p.X + 1, p.Y);

    // top
    if (p.Y > 0)
        yield return new(p.X, p.Y - 1);

    // bottom
    if (p.Y < input.Height - 1)
        yield return new(p.X, p.Y + 1);
}

IEnumerable<Point> GetLowPoints()
{
    var allPoints = EnumerateAllPoints().ToArray();

    foreach (var point in EnumerateAllPoints())
    {
        var adjacentPoints = GetAdjacentPoints(point);

        if (adjacentPoints.All(neighbour => input[neighbour] > input[point]))
            yield return point;
    }
}

IEnumerable<HashSet<Point>> GetBasins()
{
    foreach (var lowPoint in GetLowPoints())
    {
        var basin = new HashSet<Point>();
        basin.Add(lowPoint);

        var pointsToVisit = new HashSet<Point>();
        pointsToVisit.Add(lowPoint);

        while (pointsToVisit.Count > 0)
        {
            var currentPoint = pointsToVisit.First();
            pointsToVisit.Remove(currentPoint);

            var adjacentPoints = GetAdjacentPoints(currentPoint);

            foreach (var adjacentPoint in adjacentPoints)
            {
                // Skip points already part of the current basin
                if (basin.Contains(adjacentPoint))
                    continue;

                // Skip points with a value of 9
                if (input[adjacentPoint] >= 9)
                    continue;

                basin.Add(adjacentPoint);
                pointsToVisit.Add(adjacentPoint);
            }
        }

        yield return basin;
    }
}


long GetAnswer1() => GetLowPoints()
    .Select(p => input[p] + 1)
    .Sum();

long GetAnswer2() => GetBasins()
    .Select(x => x.Count)
    .OrderByDescending(x => x)
    .Take(3)
    .Aggregate((a, b) => a * b);


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");


record Point(int X, int Y);

record HeightMap(int[][] Data)
{
    public int Height => Data.Length;

    public int Width => Data[0].Length;

    public int this[Point p] => Data[p.Y][p.X];
}
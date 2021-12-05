using System;
using System.IO;
using System.Linq;

var input = File.ReadAllLines("input.txt")
    .Where(line => !String.IsNullOrWhiteSpace(line))
    .Select(ParseInput)
    .ToArray();


Line ParseInput(string line)
{
    Point ParsePoint(string pointStr)
    {
        var segments = pointStr.Split(',');
        return new Point(int.Parse(segments[0]), int.Parse(segments[1]));
    }

    var segments = line.Split("->");
    var fromString = segments[0];
    var toString = segments[1];

    return new Line(ParsePoint(fromString), ParsePoint(toString));
}

int[,] CreateCanvas()
{
    var maxX = input.Max(x => Math.Max(x.Start.X, x.End.X));
    var maxY = input.Max(x => Math.Max(x.Start.Y, x.End.Y));

    return new int[maxX + 1, maxY + 1];
}

void DrawLine(int[,] canvas, Line line)
{
    // Check if line is vertical line, we cannot use the linear function to draw this
    if (line.IsVertical)
    {
        var yStart = Math.Min(line.Start.Y, line.End.Y);
        var yEnd = Math.Max(line.Start.Y, line.End.Y);

        for (int y = yStart; y <= yEnd; y++)
            canvas[line.Start.X, y] += 1;
    }
    else
    {
        // determine the first and last x-coordinate to draw 
        // line might go from left to rgiht or the other way round, but the x loop always gos from smaller to higher values
        var xStart = Math.Min(line.Start.X, line.End.X);
        var xEnd = Math.Max(line.Start.X, line.End.X);

        // The y coordinate can be computed from the x coordiante using a linear function of the form: y = ax + b 
        // a = Δx / Δy
        // b = y - ax

        int a;

        // we cannot use the formula for horizontal lines (divide by zero)
        // for horizontal lines, a is 0
        if (line.End.Y == line.Start.Y)
            a = 0;
        // a = Δx / Δy
        else
            a = (line.End.X - line.Start.X) / (line.End.Y - line.Start.Y);

        var b = line.Start.Y - (a * line.Start.X);

        for (int x = xStart; x <= xEnd; x++)
        {
            // y = ax + b 
            var y = (a * x) + b;
            canvas[x, y] += 1;
        }
    }
}

int CountOverlaps(int[,] canvas)
{
    var overlaps = 0;
    for (int x = 0; x < canvas.GetLength(0); x++)
        for (int y = 0; y < canvas.GetLength(1); y++)
            if (canvas[x, y] >= 2)
                overlaps += 1;

    return overlaps;
}


long GetAnswer1()
{
    var canvas = CreateCanvas();

    foreach (var line in input.Where(line => line.IsHorizontal || line.IsVertical))
    {
        DrawLine(canvas, line);
    }

    return CountOverlaps(canvas);
}

long GetAnswer2()
{
    var canvas = CreateCanvas();

    foreach (var line in input)
    {
        DrawLine(canvas, line);
    }

    return CountOverlaps(canvas);
}


var firstAnswer = GetAnswer1();
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 1: {firstAnswer}");
Console.WriteLine($"Answer 2: {secondAnswer}");


record Line(Point Start, Point End)
{
    public bool IsVertical => Start.X == End.X;

    public bool IsHorizontal => Start.Y == End.Y;
}

record Point(int X, int Y);
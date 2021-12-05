using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

var inputLines = File.ReadAllLines("input.txt");

(IReadOnlyList<int> numbersDrawn, IReadOnlyList<BingoBoard> boards) ReadInput()
{
    var numbersDrawn = inputLines[0].Split(',').Select(int.Parse).ToArray();
    var boards = new List<BingoBoard>();
    var boardNumber = 1;

    for (int i = 2; i < inputLines.Length; i++)
    {
        var currentLine = inputLines[i];

        if (!String.IsNullOrEmpty(currentLine))
        {
            var (nextline, board) = ReadBoard(i, boardNumber++);
            boards.Add(board);
            i = nextline;
        }
    }

    return (numbersDrawn, boards);
}

(int lastReadLine, BingoBoard board) ReadBoard(int startIndex, int boardNumber)
{
    var board = new BingoBoard(boardNumber);

    var i = startIndex;
    for (; i < inputLines.Length; i++)
    {
        if (String.IsNullOrWhiteSpace(inputLines[i]))
            break;

        var row = inputLines[i].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Select(int.Parse);
        board.AddRow(row);
    }

    return (i, board);
}

var (numbersDrawn, boards) = ReadInput();

long GetAnswer1()
{
    foreach (var number in numbersDrawn)
    {
        foreach (var board in boards)
        {
            board.MarkCells(number);
        }

        foreach (var board in boards)
        {
            if (board.IsWinning)
            {
                return board.UnmarkedCells.Sum(x => x.Value) * number;
            }
        }
    }

    throw new Exception("No winning board");
}

long GetAnswer2()
{
    var winningBoards = new HashSet<BingoBoard>();

    foreach (var number in numbersDrawn)
    {
        foreach (var board in boards)
        {
            board.MarkCells(number);
        }

        foreach (var board in boards)
        {
            if (board.IsWinning)
            {
                winningBoards.Add(board);

                // we found the last board to win
                if (winningBoards.Count == boards.Count)
                {
                    return board.UnmarkedCells.Sum(x => x.Value) * number;
                }
            }
        }
    }

    throw new Exception("No winning board");
}


var firstAnswer = GetAnswer1();
Console.WriteLine($"Answer 1: {firstAnswer}");
var secondAnswer = GetAnswer2();
Console.WriteLine($"Answer 2: {secondAnswer}");

class BingoBoard
{
    private readonly List<List<BingoBoardCell>> m_Rows = new();

    public int BoardNumber { get; }

    public bool IsWinning => HasMarkedRow || HasMarkedColumn;

    public IEnumerable<BingoBoardCell> UnmarkedCells => m_Rows.SelectMany(row => row.Where(cell => !cell.IsMarked));


    private bool HasMarkedRow => m_Rows.Any(row => row.All(cell => cell.IsMarked));

    private bool HasMarkedColumn
    {
        get
        {
            for (int column = 0; column < m_Rows[0].Count; column++)
            {
                if (m_Rows.All(row => row[column].IsMarked))
                    return true;
            }
            return false;
        }
    }


    public BingoBoard(int boardNumber)
    {
        BoardNumber = boardNumber;
    }

    public void AddRow(IEnumerable<int> rowValues)
    {
        m_Rows.Add(rowValues.Select(x => new BingoBoardCell(x)).ToList());
    }

    public void MarkCells(int value)
    {
        foreach (var row in m_Rows)
        {
            foreach (var cell in row)
            {
                if (cell.Value == value)
                    cell.IsMarked = true;
            }
        }
    }
}

record BingoBoardCell(int Value)
{
    public bool IsMarked { get; set; }
}
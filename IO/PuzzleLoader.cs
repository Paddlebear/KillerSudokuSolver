using System.Text.RegularExpressions;
using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.IO;

public static class PuzzleLoader
{
    public static Puzzle Load(string path)
    {
        var cages = new List<Cage>();
        var solution = new int[Puzzle.Size, Puzzle.Size]; // stays all 0 if no values present
        bool hasSolution = false;

        // matches (row, col) optionally followed by ,value
        var cellRegex = new Regex(@"\((\d+),\s*(\d+)\)(?:,(\d+))?");

        foreach (var line in File.ReadLines(path))
        {
            if (string.IsNullOrWhiteSpace(line)) continue;

            var parts = line.Split('=', 2);
            int sum = int.Parse(parts[0].Trim());

            var cells = new List<(int Row, int Col)>();

            foreach (Match m in cellRegex.Matches(parts[1]))
            {
                int row = int.Parse(m.Groups[1].Value);
                int col = int.Parse(m.Groups[2].Value);
                cells.Add((row, col));

                if (m.Groups[3].Success)
                {
                    solution[row, col] = int.Parse(m.Groups[3].Value);
                    hasSolution = true;
                }
            }

            cages.Add(new Cage(sum, cells));
        }

        var puzzle = new Puzzle(cages);
        if (hasSolution) puzzle.Solution = solution;

        return puzzle;
    }
}
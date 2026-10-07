using System.Text.RegularExpressions;
using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Testing;

public static class AnswerFileLoader
{
    public static Grid Load(string path)
    {
        var grid = new Grid();

        var lines = File.ReadLines(path)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        if (lines.Count != Grid.Size)
        {
            throw new InvalidOperationException(
                $"Expected {Grid.Size} rows in {path}, found {lines.Count}.");
        }

        for (int row = 0; row < Grid.Size; row++)
        {
            var values = Regex.Matches(lines[row], @"\d+")
                .Select(match => int.Parse(match.Value))
                .ToList();

            if (values.Count != Grid.Size)
            {
                throw new InvalidOperationException(
                    $"Expected {Grid.Size} values in row {row + 1}.");
            }

            for (int col = 0; col < Grid.Size; col++)
            {
                grid[row, col] = values[col];
            }
        }

        return grid;
    }
}
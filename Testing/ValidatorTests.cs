using KillerSudokuSolver.IO;
using KillerSudokuSolver.Model;
using KillerSudokuSolver.Solver_v4;
using System.Text.RegularExpressions;

namespace KillerSudokuSolver.Testing;

public static class ValidatorTests
{
    public static void RunAll()
    {
        var tests = new[]
        {
            new TestCase(
                "560.ans",
                "data/subset/2/560.killer",
                "data/subset/2/560.ans"),
            new TestCase(
                "711.ans",
                "data/subset/2/711.killer",
                "data/subset/2/711.ans")
        };

        foreach (var test in tests)
        {
            var puzzle = PuzzleLoader.Load(test.PuzzlePath);
            var expected = AnswerFileLoader.Load(test.AnswerPath);

            var valid = FullGridValidator.IsValid(expected, puzzle);

            Assert(valid,
                $"{test.Name}: expected answer should be valid.");

            Assert(GridMatches(expected, puzzle),
                $"{test.Name}: expected answer does not match the puzzle clues.");

            Console.WriteLine(
                $"PASS {test.Name}: valid solution and clues preserved.");
        }

        TestInvalidGrid();
    }

    private static void TestInvalidGrid()
    {
        var puzzle = PuzzleLoader.Load("data/2/560.killer");
        var invalidGrid = new Grid();

        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                invalidGrid[row, col] = 1;
            }
        }

        Assert(
            !FullGridValidator.IsValid(invalidGrid, puzzle),
            "Invalid grid containing duplicate row values should fail validation.");
    }

    private static bool GridMatches(Grid actual, Puzzle puzzle)
    {
        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                if (puzzle.IsFixed(row, col) &&
                    actual[row, col] != puzzle.Givens[row, col])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static void Assert(bool condition, string message)
    {
        if (!condition)
            throw new InvalidOperationException(message);
    }

    private sealed record TestCase(
        string Name,
        string PuzzlePath,
        string AnswerPath);
}
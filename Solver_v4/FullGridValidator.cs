using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v4;

public static class FullGridValidator
{
    public static bool IsValid(Grid grid, Puzzle puzzle)
    {
        return IsValid(grid, puzzle, null, out _);
    }

    public static bool IsValid(
        Grid grid,
        Puzzle puzzle,
        Grid? knownAnswer,
        out bool knownAnswerMatches)
    {
        knownAnswerMatches = false;

        if (grid == null)
            return false;

        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                int value = grid[row, col];

                if (value < 1 || value > 9)
                    return false;
            }
        }

        if (!ValidateRows(grid) ||
            !ValidateColumns(grid) ||
            !ValidateBoxes(grid))
        {
            return false;
        }

        if (!ValidateCages(grid, puzzle) ||
            !ValidateGivens(grid, puzzle))
        {
            return false;
        }

        if (knownAnswer != null)
            knownAnswerMatches = GridsEqual(grid, knownAnswer);

        return true;
    }

    private static bool ValidateRows(Grid grid)
    {
        for (int row = 0; row < Grid.Size; row++)
        {
            var seen = new bool[10];

            for (int col = 0; col < Grid.Size; col++)
            {
                int value = grid[row, col];

                if (seen[value])
                    return false;

                seen[value] = true;
            }
        }

        return true;
    }

    private static bool ValidateColumns(Grid grid)
    {
        for (int col = 0; col < Grid.Size; col++)
        {
            var seen = new bool[10];

            for (int row = 0; row < Grid.Size; row++)
            {
                int value = grid[row, col];

                if (seen[value])
                    return false;

                seen[value] = true;
            }
        }

        return true;
    }

    private static bool ValidateBoxes(Grid grid)
    {
        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                var seen = new bool[10];

                for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
                {
                    for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                    {
                        int value = grid[row, col];

                        if (seen[value])
                            return false;

                        seen[value] = true;
                    }
                }
            }
        }

        return true;
    }

    private static bool ValidateCages(Grid grid, Puzzle puzzle)
    {
        foreach (var cage in puzzle.Cages)
        {
            var values = cage.Cells
                .Select(cell => grid[cell.Row, cell.Col])
                .ToList();

            if (values.Any(value => value == 0))
                return false;

            if (values.Sum() != cage.Sum)
                return false;

            if (values.Distinct().Count() != values.Count)
                return false;
        }

        return true;
    }

    private static bool ValidateGivens(Grid grid, Puzzle puzzle)
    {
        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                if (puzzle.IsFixed(row, col) &&
                    grid[row, col] != puzzle.Givens[row, col])
                {
                    return false;
                }
            }
        }

        return true;
    }

    private static bool GridsEqual(Grid first, Grid second)
    {
        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                if (first[row, col] != second[row, col])
                    return false;
            }
        }

        return true;
    }
}
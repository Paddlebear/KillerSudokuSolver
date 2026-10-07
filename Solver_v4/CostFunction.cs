using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v4;

public static class CostFunction
{
    public static int Calculate(Grid grid, Puzzle puzzle)
    {
        int missingCells = 0;
        int cageErrors = 0;

        for (int row = 0; row < Grid.Size; row++)
        {
            for (int col = 0; col < Grid.Size; col++)
            {
                if (grid[row, col] == 0)
                    missingCells++;
            }
        }

        foreach (var cage in puzzle.Cages)
        {
            var values = cage.Cells
                .Select(cell => grid[cell.Row, cell.Col])
                .ToList();

            if (values.Any(value => value == 0))
                cageErrors++;

            if (values.Sum() != cage.Sum)
                cageErrors++;

            if (values.Distinct().Count() != values.Count)
                cageErrors++;
        }

        return missingCells * 1000 +
               cageErrors * 100 +
               CountRowConflicts(grid) +
               CountColumnConflicts(grid) +
               CountBoxConflicts(grid);
    }

    private static int CountRowConflicts(Grid grid)
    {
        int conflicts = 0;

        for (int row = 0; row < Grid.Size; row++)
        {
            var counts = new int[10];

            for (int col = 0; col < Grid.Size; col++)
                counts[grid[row, col]]++;

            conflicts += ExtraCopies(counts);
        }

        return conflicts;
    }

    private static int CountColumnConflicts(Grid grid)
    {
        int conflicts = 0;

        for (int col = 0; col < Grid.Size; col++)
        {
            var counts = new int[10];

            for (int row = 0; row < Grid.Size; row++)
                counts[grid[row, col]]++;

            conflicts += ExtraCopies(counts);
        }

        return conflicts;
    }

    private static int CountBoxConflicts(Grid grid)
    {
        int conflicts = 0;

        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                var counts = new int[10];

                for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
                {
                    for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                        counts[grid[row, col]]++;
                }

                conflicts += ExtraCopies(counts);
            }
        }

        return conflicts;
    }

    private static int ExtraCopies(int[] counts)
    {
        int extra = 0;

        for (int digit = 1; digit <= 9; digit++)
        {
            if (counts[digit] > 1)
                extra += counts[digit] - 1;
        }

        return extra;
    }
}
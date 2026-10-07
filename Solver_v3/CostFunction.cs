using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v3;

public static class CostFunction
{
    public static int Calculate(Grid grid, Puzzle puzzle)
    {
        return CountRowConflicts(grid) + CountColumnConflicts(grid) + CountBoxConflicts(grid);
    }

    private static int CountRowConflicts(Grid grid)
    {
        int conflicts = 0;
        for (int r = 0; r < Grid.Size; r++)
        {
            var counts = new int[10];
            for (int c = 0; c < Grid.Size; c++) counts[grid[r, c]]++;
            conflicts += ExtraCopies(counts);
        }
        return conflicts;
    }

    private static int CountColumnConflicts(Grid grid)
    {
        int conflicts = 0;
        for (int c = 0; c < Grid.Size; c++)
        {
            var counts = new int[10];
            for (int r = 0; r < Grid.Size; r++) counts[grid[r, c]]++;
            conflicts += ExtraCopies(counts);
        }
        return conflicts;
    }

    private static int CountBoxConflicts(Grid grid)
    {
        int conflicts = 0;
        for (int boxRow = 0; boxRow < 3; boxRow++)
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                var counts = new int[10];
                for (int r = boxRow * 3; r < boxRow * 3 + 3; r++)
                    for (int c = boxCol * 3; c < boxCol * 3 + 3; c++)
                        counts[grid[r, c]]++;
                conflicts += ExtraCopies(counts);
            }
        return conflicts;
    }

    private static int ExtraCopies(int[] counts)
    {
        int extra = 0;
        for (int digit = 1; digit <= 9; digit++)
            if (counts[digit] > 1) extra += counts[digit] - 1;
        return extra;
    }
}
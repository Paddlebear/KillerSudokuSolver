using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v2;

public static class CostFunction
{
    public static int Calculate(Grid grid, Puzzle puzzle)
    {
        int cost = 0;
        cost += CountRowConflicts(grid);
        cost += CountColumnConflicts(grid);
        cost += CountBoxConflicts(grid);
        cost += CountCageViolations(grid, puzzle);
        return cost;
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
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                var counts = new int[10];
                for (int r = boxRow * 3; r < boxRow * 3 + 3; r++)
                    for (int c = boxCol * 3; c < boxCol * 3 + 3; c++)
                        counts[grid[r, c]]++;
                conflicts += ExtraCopies(counts);
            }
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

    private static int CountCageViolations(Grid grid, Puzzle puzzle)
    {
        int violations = 0;
        foreach (var cage in puzzle.Cages)
        {
            var values = cage.Cells.Select(cell => grid[cell.Row, cell.Col]).ToList();
            int sum = values.Sum();
            violations += Math.Abs(sum - cage.Sum);

            var counts = new Dictionary<int, int>();
            foreach (var v in values) counts[v] = counts.GetValueOrDefault(v) + 1;
            foreach (var count in counts.Values)
                if (count > 1) violations += (count - 1) * 3;
        }
        return violations;
    }
}
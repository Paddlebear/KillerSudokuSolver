using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v1;

public static class CostFunction
{
    public static int Calculate(Grid grid, Puzzle puzzle)
    {
        int cost = 0;
        cost += CountRowConflicts(grid);
        cost += CountColumnConflicts(grid);
        cost += CountCageViolations(grid, puzzle);
        return cost;
    }

    private static int CountRowConflicts(Grid grid)
    {
        int conflicts = 0;
        for (int r = 0; r < Grid.Size; r++)
        {
            var counts = new int[10]; // index 1-9 used
            for (int c = 0; c < Grid.Size; c++)
                counts[grid[r, c]]++;

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
            for (int r = 0; r < Grid.Size; r++)
                counts[grid[r, c]]++;

            conflicts += ExtraCopies(counts);
        }
        return conflicts;
    }

    // for a set of digit counts, "extra copies" = how many cells would need
    // to change to remove all duplicates, e.g. if a digit appears 3 times, that's 2 extra
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

    private static int CountCageViolations(Grid grid, Puzzle puzzle)
    {
        int violations = 0;

        foreach (var cage in puzzle.Cages)
        {
            var values = cage.Cells.Select(cell => grid[cell.Row, cell.Col]).ToList();

            // sum mismatch penalty: how far off the cage total is from its target
            int sum = values.Sum();
            violations += Math.Abs(sum - cage.Sum);

            // duplicate penalty within the cage
            var counts = new Dictionary<int, int>();
            foreach (var v in values)
                counts[v] = counts.GetValueOrDefault(v) + 1;

            foreach (var count in counts.Values)
                if (count > 1)
                    violations += (count - 1) * 3; // weighted higher than a plain sum-off-by-one
        }

        return violations;
    }
}
using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v3;

public static class MoveGenerator2
{
    public static Grid Move(
        Grid grid,
        Puzzle puzzle,
        Random rng,
        double resampleChance = 0.15)
    {
        var conflictedRows = FindConflictedRows(grid);
        var conflictedColumns = FindConflictedColumns(grid);
        var conflictedBoxes = FindConflictedBoxes(grid);

        var weights = new int[puzzle.Cages.Count];
        int totalWeight = 0;

        for (int cageIndex = 0; cageIndex < puzzle.Cages.Count; cageIndex++)
        {
            var cage = puzzle.Cages[cageIndex];
            int weight = 0;

            foreach (var cell in cage.Cells)
            {
                if (puzzle.IsFixed(cell.Row, cell.Col))
                    continue;

                // Keep a baseline chance for exploration; increase weight
                // for each movable cell involved in a Sudoku conflict.
                weight++;
                if (conflictedRows[cell.Row]) weight++;
                if (conflictedColumns[cell.Col]) weight++;
                if (conflictedBoxes[cell.Row / 3 * 3 + cell.Col / 3]) weight++;
            }

            weights[cageIndex] = weight;
            totalWeight += weight;
        }

        if (totalWeight == 0)
            return grid.Clone();

        int selection = rng.Next(totalWeight);
        int selectedIndex = 0;

        for (; selectedIndex < weights.Length; selectedIndex++)
        {
            if (selection < weights[selectedIndex])
                break;

            selection -= weights[selectedIndex];
        }

        var selectedCage = puzzle.Cages[selectedIndex];
        var swappable = selectedCage.Cells
            .Where(cell => !puzzle.IsFixed(cell.Row, cell.Col))
            .ToList();

        if (swappable.Count < 2 || rng.NextDouble() < resampleChance)
            return ResampleCage(grid, puzzle, selectedCage, rng);

        return RepositionWithinCage(grid, swappable, rng);
    }

    private static Grid RepositionWithinCage(
        Grid grid,
        List<(int Row, int Col)> swappable,
        Random rng)
    {
        var newGrid = grid.Clone();

        int firstIndex = rng.Next(swappable.Count);
        int secondIndex;
        do
        {
            secondIndex = rng.Next(swappable.Count);
        } while (secondIndex == firstIndex);

        var first = swappable[firstIndex];
        var second = swappable[secondIndex];

        (newGrid[first.Row, first.Col], newGrid[second.Row, second.Col]) =
            (newGrid[second.Row, second.Col], newGrid[first.Row, first.Col]);

        return newGrid;
    }

    private static Grid ResampleCage(
        Grid grid,
        Puzzle puzzle,
        Cage cage,
        Random rng)
    {
        var newGrid = grid.Clone();
        InitialStateGenerator.AssignCage(newGrid, puzzle, cage, rng);
        return newGrid;
    }

    private static bool[] FindConflictedRows(Grid grid)
    {
        var conflicted = new bool[Grid.Size];
        var counts = new int[10];

        for (int row = 0; row < Grid.Size; row++)
        {
            Array.Clear(counts, 0, counts.Length);

            for (int col = 0; col < Grid.Size; col++)
                counts[grid[row, col]]++;

            conflicted[row] = HasDuplicate(counts);
        }

        return conflicted;
    }

    private static bool[] FindConflictedColumns(Grid grid)
    {
        var conflicted = new bool[Grid.Size];
        var counts = new int[10];

        for (int col = 0; col < Grid.Size; col++)
        {
            Array.Clear(counts, 0, counts.Length);

            for (int row = 0; row < Grid.Size; row++)
                counts[grid[row, col]]++;

            conflicted[col] = HasDuplicate(counts);
        }

        return conflicted;
    }

    private static bool[] FindConflictedBoxes(Grid grid)
    {
        var conflicted = new bool[Grid.Size];
        var counts = new int[10];

        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                Array.Clear(counts, 0, counts.Length);

                for (int row = boxRow * 3; row < boxRow * 3 + 3; row++)
                {
                    for (int col = boxCol * 3; col < boxCol * 3 + 3; col++)
                        counts[grid[row, col]]++;
                }

                conflicted[boxRow * 3 + boxCol] = HasDuplicate(counts);
            }
        }

        return conflicted;
    }

    private static bool HasDuplicate(int[] counts)
    {
        for (int digit = 1; digit <= 9; digit++)
        {
            if (counts[digit] > 1)
                return true;
        }

        return false;
    }
}
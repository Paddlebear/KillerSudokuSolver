using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v3;

public static class MoveGenerator
{
    public static Grid Move(Grid grid, Puzzle puzzle, Random rng, double resampleChance = 0.15)
    {
        return rng.NextDouble() < resampleChance
            ? ResampleCage(grid, puzzle, rng)
            : RepositionWithinCage(grid, puzzle, rng);
    }

    private static Grid RepositionWithinCage(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();
        var cage = puzzle.Cages[rng.Next(puzzle.Cages.Count)];
        var swappable = cage.Cells.Where(c => !puzzle.IsFixed(c.Row, c.Col)).ToList();

        if (swappable.Count < 2) return newGrid;

        int i = rng.Next(swappable.Count);
        int j;
        do { j = rng.Next(swappable.Count); } while (j == i);

        var (r1, c1) = swappable[i];
        var (r2, c2) = swappable[j];
        (newGrid[r1, c1], newGrid[r2, c2]) = (newGrid[r2, c2], newGrid[r1, c1]);

        return newGrid;
    }

    private static Grid ResampleCage(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();
        var cage = puzzle.Cages[rng.Next(puzzle.Cages.Count)];
        InitialStateGenerator.AssignCage(newGrid, puzzle, cage, rng);
        return newGrid;
    }
}
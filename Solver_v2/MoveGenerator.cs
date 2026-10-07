using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v2;

public static class MoveGenerator
{
    public static Grid Move(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();

        var swappable = new List<(int Row, int Col)>();
        for (int r = 0; r < Grid.Size; r++)
            for (int c = 0; c < Grid.Size; c++)
                if (!puzzle.IsFixed(r, c))
                    swappable.Add((r, c));

        if (swappable.Count < 2) return newGrid;

        int i = rng.Next(swappable.Count);
        int j;
        do { j = rng.Next(swappable.Count); } while (j == i);

        var (r1, c1) = swappable[i];
        var (r2, c2) = swappable[j];
        (newGrid[r1, c1], newGrid[r2, c2]) = (newGrid[r2, c2], newGrid[r1, c1]);

        return newGrid;
    }
}
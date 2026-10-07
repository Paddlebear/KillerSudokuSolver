using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v2;

public static class InitialStateGenerator
{
    public static Grid Generate(Puzzle puzzle, Random rng)
    {
        var grid = new Grid();

        for (int r = 0; r < Grid.Size; r++)
        {
            for (int c = 0; c < Grid.Size; c++)
            {
                if (puzzle.IsFixed(r, c))
                    grid[r, c] = puzzle.Givens[r, c];
                else
                    grid[r, c] = rng.Next(1, 10); // random 1-9, no structure guaranteed
            }
        }

        return grid;
    }
}
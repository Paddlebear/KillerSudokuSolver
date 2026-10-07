using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v1;

public static class InitialStateGenerator
{
    public static Grid Generate(Puzzle puzzle, Random rng)
    {
        var grid = new Grid();

        // iterate over each of the 9 boxes (3x3 blocks)
        for (int boxRow = 0; boxRow < 3; boxRow++)
        {
            for (int boxCol = 0; boxCol < 3; boxCol++)
            {
                FillBox(grid, puzzle, boxRow, boxCol, rng);
            }
        }

        return grid;
    }

    private static void FillBox(Grid grid, Puzzle puzzle, int boxRow, int boxCol, Random rng)
    {
        int startRow = boxRow * 3;
        int startCol = boxCol * 3;

        var fixedCells = new List<(int Row, int Col)>();
        var usedDigits = new HashSet<int>();

        // first pass: place any givens, track which cells are fixed and which digits are taken
        for (int r = startRow; r < startRow + 3; r++)
        {
            for (int c = startCol; c < startCol + 3; c++)
            {
                if (puzzle.IsFixed(r, c))
                {
                    grid[r, c] = puzzle.Givens[r, c];
                    usedDigits.Add(puzzle.Givens[r, c]);
                    fixedCells.Add((r, c));
                }
            }
        }

        // remaining digits = 1-9 minus whatever's already used by givens
        var remainingDigits = Enumerable.Range(1, 9)
            .Where(d => !usedDigits.Contains(d))
            .OrderBy(_ => rng.Next())
            .ToList();

        int digitIndex = 0;
        for (int r = startRow; r < startRow + 3; r++)
        {
            for (int c = startCol; c < startCol + 3; c++)
            {
                if (!fixedCells.Contains((r, c)))
                {
                    grid[r, c] = remainingDigits[digitIndex];
                    digitIndex++;
                }
            }
        }
    }
}
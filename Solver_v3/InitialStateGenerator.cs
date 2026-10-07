using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v3;

public static class InitialStateGenerator
{
    public static Grid Generate(Puzzle puzzle, Random rng)
    {
        var grid = new Grid();

        foreach (var cage in puzzle.Cages)
        {
            AssignCage(grid, puzzle, cage, rng);
        }

        return grid;
    }

    public static void AssignCage(Grid grid, Puzzle puzzle, Cage cage, Random rng)
    {
        var nonFixed = cage.Cells.Where(c => !puzzle.IsFixed(c.Row, c.Col)).ToList();
        var fixedCells = cage.Cells.Where(c => puzzle.IsFixed(c.Row, c.Col)).ToList();

        foreach (var (r, c) in fixedCells)
            grid[r, c] = puzzle.Givens[r, c];

        if (nonFixed.Count == 0) return;

        int fixedSum = fixedCells.Sum(c => puzzle.Givens[c.Row, c.Col]);
        var fixedDigits = fixedCells.Select(c => puzzle.Givens[c.Row, c.Col]).ToHashSet();

        var combos = CageCombinations.FindCombinations(nonFixed.Count, cage.Sum - fixedSum)
            .Where(combo => combo.All(d => !fixedDigits.Contains(d))) // no clash with givens
            .ToList();

        if (combos.Count == 0)
        {
            // fallback: shouldn't normally happen on a well-formed puzzle, but guard anyway
            foreach (var (r, c) in nonFixed) grid[r, c] = rng.Next(1, 10);
            return;
        }

        var chosen = combos[rng.Next(combos.Count)];
        var shuffled = chosen.OrderBy(_ => rng.Next()).ToList();

        for (int k = 0; k < nonFixed.Count; k++)
            grid[nonFixed[k].Row, nonFixed[k].Col] = shuffled[k];
    }
}
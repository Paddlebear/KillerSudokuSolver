namespace KillerSudokuSolver.Model;

public class Puzzle
{
    public const int Size = 9;
    public List<Cage> Cages { get; }

    // 0 = no given clue, 1-9 = fixed clue value
    public int[,] Givens { get; }

    public Puzzle(List<Cage> cages, int[,]? givens = null)
    {
        Cages = cages;
        Givens = givens ?? new int[Size, Size]; // defaults to all zeros = no givens
    }

    public bool IsFixed(int row, int col) => Givens[row, col] != 0;

    public int[,]? Solution { get; set; } // null if the file didn't include answers

    public List<string> ValidateCoverage()
    {
        var errors = new List<string>();
        var seen = new int[Size, Size]; // counts how many cages claim each cell

        foreach (var cage in Cages)
        {
            foreach (var (row, col) in cage.Cells)
            {
                if (row < 0 || row >= Size || col < 0 || col >= Size)
                {
                    errors.Add($"Cage sum={cage.Sum} has out-of-bounds cell ({row},{col})");
                    continue;
                }
                seen[row, col]++;
            }
        }

        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
            {
                if (seen[r, c] == 0)
                    errors.Add($"Cell ({r},{c}) is not covered by any cage");
                else if (seen[r, c] > 1)
                    errors.Add($"Cell ({r},{c}) is covered by {seen[r, c]} cages (overlap)");
            }
        }

        if (Solution != null)
        {
            foreach (var cage in Cages)
            {
                var values = cage.Cells.Select(c => Solution[c.Row, c.Col]).ToList();
                if (values.Sum() != cage.Sum)
                    errors.Add($"Cage sum={cage.Sum} actual solution sum={values.Sum()}");
                if (values.Distinct().Count() != values.Count)
                    errors.Add($"Cage sum={cage.Sum} has duplicate values in solution");
            }
        }

        return errors;
    }

    public List<string> ValidateCages(Grid grid)
{
    var errors = new List<string>();

    foreach (var cage in Cages)
    {
        var values = cage.Cells.Select(c => grid[c.Row, c.Col]).ToList();
        int sum = values.Sum();

        if (sum != cage.Sum)
            errors.Add($"Cage target={cage.Sum} has actual sum={sum} (cells={string.Join(",", cage.Cells)})");

        var dupes = values.GroupBy(v => v).Where(g => g.Count() > 1).Select(g => g.Key);
        foreach (var d in dupes)
            errors.Add($"Cage target={cage.Sum} has duplicate digit {d} (cells={string.Join(",", cage.Cells)})");
    }

    return errors;
}
}
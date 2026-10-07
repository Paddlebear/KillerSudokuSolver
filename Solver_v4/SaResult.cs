using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v4;

public class SaResult
{
    public Grid BestGrid { get; set; } = null!;
    public int BestCost { get; set; }
    public int Iterations { get; set; }

    public bool ValidationPassed { get; set; }
    public bool KnownAnswerMatches { get; set; }

    public bool Solved => ValidationPassed;
}
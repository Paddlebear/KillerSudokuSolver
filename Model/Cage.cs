namespace KillerSudokuSolver.Model;

public class Cage
{
    public int Sum { get; }
    public List<(int Row, int Col)> Cells { get; }

    public Cage(int sum, List<(int Row, int Col)> cells)
    {
        Sum = sum;
        Cells = cells;
    }
}
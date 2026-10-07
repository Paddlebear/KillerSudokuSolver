namespace KillerSudokuSolver.Model;

public class Grid
{
    public const int Size = 9;
    private readonly int[,] _cells;

    public Grid()
    {
        _cells = new int[Size, Size];
    }

    public int this[int row, int col]
    {
        get => _cells[row, col];
        set => _cells[row, col] = value;
    }

    public Grid Clone()
    {
        var copy = new Grid();
        Array.Copy(_cells, copy._cells, _cells.Length);
        return copy;
    }

    public void Print()
    {
        for (int r = 0; r < Size; r++)
        {
            for (int c = 0; c < Size; c++)
                Console.Write(_cells[r, c] == 0 ? ". " : $"{_cells[r, c]} ");
            Console.WriteLine();
        }
    }

    public static Grid FromArray(int[,] values)
    {
        var grid = new Grid();
        for (int r = 0; r < Size; r++)
            for (int c = 0; c < Size; c++)
                grid[r, c] = values[r, c];
        return grid;
    }

//     public List<string> ValidateBoxes()
// {
//     var errors = new List<string>();

//     for (int boxRow = 0; boxRow < 3; boxRow++)
//     {
//         for (int boxCol = 0; boxCol < 3; boxCol++)
//         {
//             var seen = new HashSet<int>();
//             int startRow = boxRow * 3;
//             int startCol = boxCol * 3;

//             for (int r = startRow; r < startRow + 3; r++)
//             {
//                 for (int c = startCol; c < startCol + 3; c++)
//                 {
//                     int val = _cells[r, c];
//                     if (val == 0)
//                     {
//                         errors.Add($"Box ({boxRow},{boxCol}) has empty cell ({r},{c})");
//                     }
//                     else if (!seen.Add(val))
//                     {
//                         errors.Add($"Box ({boxRow},{boxCol}) has duplicate value {val}");
//                     }
//                 }
//             }
//         }
//     }

//     return errors;
// }
}
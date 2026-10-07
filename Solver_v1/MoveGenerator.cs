using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v1;

public static class MoveGenerator
{
    public static Grid Swap(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();
        var cells = GetSwappableCells(puzzle, rng.Next(3), rng.Next(3));

        if (cells.Count < 2) return newGrid;

        int i = rng.Next(cells.Count);
        int j;
        do { j = rng.Next(cells.Count); } while (j == i);

        var (r1, c1) = cells[i];
        var (r2, c2) = cells[j];
        (newGrid[r1, c1], newGrid[r2, c2]) = (newGrid[r2, c2], newGrid[r1, c1]);

        return newGrid;
    }

    public static Grid Rotate3(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();
        var cells = GetSwappableCells(puzzle, rng.Next(3), rng.Next(3));

        if (cells.Count < 3) return newGrid;

        // pick 3 distinct indices
        var indices = Enumerable.Range(0, cells.Count).OrderBy(_ => rng.Next()).Take(3).ToList();
        var (ra, ca) = cells[indices[0]];
        var (rb, cb) = cells[indices[1]];
        var (rc, cc) = cells[indices[2]];

        // A -> B -> C -> A
        int temp = newGrid[ra, ca];
        newGrid[ra, ca] = newGrid[rc, cc];
        newGrid[rc, cc] = newGrid[rb, cb];
        newGrid[rb, cb] = temp;

        return newGrid;
    }

    // picks swap or rotation at random; weight toward swap since it's cheaper/more common
public static Grid Move(Grid grid, Puzzle puzzle, Random rng,
    double rotateChance = 0.2, double rowSwapChance = 0.1, double colSwapChance = 0.1)
{
    double roll = rng.NextDouble();
    if (roll < rowSwapChance) return SwapRowsInBand(grid, puzzle, rng);
    if (roll < rowSwapChance + colSwapChance) return SwapColsInStack(grid, puzzle, rng);
    if (roll < rowSwapChance + colSwapChance + rotateChance) return Rotate3(grid, puzzle, rng);
    return Swap(grid, puzzle, rng);
}

// public static Grid Move(
//     Grid grid,
//     Puzzle puzzle,
//     Random rng,
//     double rotateChance = 0.2,
//     double rowSwapChance = 0.1,
//     double colSwapChance = 0.1,
//     double cageRepairChance = 0.1)
// {
//     double roll = rng.NextDouble();

//     if (roll < rowSwapChance)
//         return SwapRowsInBand(grid, puzzle, rng);

//     if (roll < rowSwapChance + colSwapChance)
//         return SwapColsInStack(grid, puzzle, rng);

//     if (roll < rowSwapChance + colSwapChance + rotateChance)
//         return Rotate3(grid, puzzle, rng);

//     if (roll < rowSwapChance + colSwapChance + rotateChance + cageRepairChance)
//         return RepairCageSum(grid, puzzle, rng) ?? Swap(grid, puzzle, rng);

//     return Swap(grid, puzzle, rng);
// }

    private static List<(int Row, int Col)> GetSwappableCells(Puzzle puzzle, int boxRow, int boxCol)
    {
        var cells = new List<(int Row, int Col)>();
        int startRow = boxRow * 3;
        int startCol = boxCol * 3;

        for (int r = startRow; r < startRow + 3; r++)
            for (int c = startCol; c < startCol + 3; c++)
                if (!puzzle.IsFixed(r, c))
                    cells.Add((r, c));

        return cells;
    }

    public static Grid SwapRowsInBand(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();

        int band = rng.Next(3); // which band of 3 rows (0,1,2 -> rows 0-2, 3-5, 6-8)
        int rowOffsetA = rng.Next(3);
        int rowOffsetB;
        do { rowOffsetB = rng.Next(3); } while (rowOffsetB == rowOffsetA);

        int rowA = band * 3 + rowOffsetA;
        int rowB = band * 3 + rowOffsetB;

        for (int c = 0; c < Grid.Size; c++)
        {
            // only swap cells that are non-fixed in BOTH rows at this column
            if (!puzzle.IsFixed(rowA, c) && !puzzle.IsFixed(rowB, c))
            {
                (newGrid[rowA, c], newGrid[rowB, c]) = (newGrid[rowB, c], newGrid[rowA, c]);
            }
        }

        return newGrid;
    }

    public static Grid SwapColsInStack(Grid grid, Puzzle puzzle, Random rng)
    {
        var newGrid = grid.Clone();

        int stack = rng.Next(3); // which stack of 3 columns (0,1,2 -> cols 0-2, 3-5, 6-8)
        int colOffsetA = rng.Next(3);
        int colOffsetB;
        do { colOffsetB = rng.Next(3); } while (colOffsetB == colOffsetA);

        int colA = stack * 3 + colOffsetA;
        int colB = stack * 3 + colOffsetB;

        for (int r = 0; r < Grid.Size; r++)
        {
            if (!puzzle.IsFixed(r, colA) && !puzzle.IsFixed(r, colB))
            {
                (newGrid[r, colA], newGrid[r, colB]) = (newGrid[r, colB], newGrid[r, colA]);
            }
        }

        return newGrid;
    }

//     private static Grid? RepairCageSum(Grid grid, Puzzle puzzle, Random rng)
// {
//     var cageByCell = new Dictionary<(int Row, int Col), Cage>();

//     foreach (var cage in puzzle.Cages)
//     {
//         foreach (var cell in cage.Cells)
//             cageByCell[cell] = cage;
//     }

//     var candidates =
//         new List<((int Row, int Col) Inside, (int Row, int Col) Outside)>();

//     foreach (var cage in puzzle.Cages)
//     {
//         int currentSum = cage.Cells.Sum(cell => grid[cell.Row, cell.Col]);
//         if (currentSum == cage.Sum)
//             continue;

//         foreach (var inside in cage.Cells)
//         {
//             if (puzzle.IsFixed(inside.Row, inside.Col))
//                 continue;

//             int boxRow = inside.Row / 3 * 3;
//             int boxCol = inside.Col / 3 * 3;

//             for (int row = boxRow; row < boxRow + 3; row++)
//             {
//                 for (int col = boxCol; col < boxCol + 3; col++)
//                 {
//                     var outside = (Row: row, Col: col);

//                     if (puzzle.IsFixed(row, col) || cage.Cells.Contains(outside))
//                         continue;

//                     var otherCage = cageByCell[outside];
//                     int newSum = currentSum
//                         - grid[inside.Row, inside.Col]
//                         + grid[outside.Row, outside.Col];

//                     if (Math.Abs(newSum - cage.Sum) >= Math.Abs(currentSum - cage.Sum))
//                         continue;

//                     int oldPenalty =
//                         CagePenalty(cage, grid) +
//                         CagePenalty(otherCage, grid);

//                     int newPenalty =
//                         CagePenalty(cage, grid, inside, outside) +
//                         CagePenalty(otherCage, grid, inside, outside);

//                     if (newPenalty < oldPenalty)
//                         candidates.Add((inside, outside));
//                 }
//             }
//         }
//     }

//     if (candidates.Count == 0)
//         return null;

//     var (inCage, outsideCage) = candidates[rng.Next(candidates.Count)];
//     var newGrid = grid.Clone();

//     (newGrid[inCage.Row, inCage.Col], newGrid[outsideCage.Row, outsideCage.Col]) =
//         (newGrid[outsideCage.Row, outsideCage.Col], newGrid[inCage.Row, inCage.Col]);

//     return newGrid;
// }

// private static int CagePenalty(
//     Cage cage,
//     Grid grid,
//     (int Row, int Col)? swapA = null,
//     (int Row, int Col)? swapB = null)
// {
//     int sum = 0;
//     var counts = new int[10];

//     foreach (var cell in cage.Cells)
//     {
//         int value = grid[cell.Row, cell.Col];

//         if (swapA.HasValue && swapB.HasValue)
//         {
//             if (cell == swapA.Value)
//                 value = grid[swapB.Value.Row, swapB.Value.Col];
//             else if (cell == swapB.Value)
//                 value = grid[swapA.Value.Row, swapA.Value.Col];
//         }

//         sum += value;
//         counts[value]++;
//     }

//     int penalty = Math.Abs(sum - cage.Sum);

//     for (int digit = 1; digit <= 9; digit++)
//     {
//         if (counts[digit] > 1)
//             penalty += (counts[digit] - 1) * 3;
//     }

//     return penalty;
// }
}
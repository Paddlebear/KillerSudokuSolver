namespace KillerSudokuSolver.Solver_v4;

public static class CageCombinations
{
    // returns all distinct N-digit combinations from 1-9 summing to target
    public static List<List<int>> FindCombinations(int size, int target)
    {
        var results = new List<List<int>>();
        FindRecursive(1, size, target, new List<int>(), results);
        return results;
    }

    private static void FindRecursive(int start, int remainingSize, int remainingSum,
        List<int> current, List<List<int>> results)
    {
        if (remainingSize == 0)
        {
            if (remainingSum == 0) results.Add(new List<int>(current));
            return;
        }

        for (int d = start; d <= 9; d++)
        {
            if (d > remainingSum) break;
            current.Add(d);
            FindRecursive(d + 1, remainingSize - 1, remainingSum - d, current, results);
            current.RemoveAt(current.Count - 1);
        }
    }
}
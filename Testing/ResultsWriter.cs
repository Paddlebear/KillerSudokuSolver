namespace KillerSudokuSolver.Testing;

public static class ResultsWriter
{
    public static void WriteCsv(List<TestRecord> records, string path)
    {
        using var writer = new StreamWriter(path);
        writer.WriteLine("PuzzleFile,DifficultyTier,CoolingRate,Seed,Solved,BestCost,Iterations,TimeMs");

        foreach (var r in records)
        {
            writer.WriteLine($"{r.PuzzleFile},{r.DifficultyTier},{r.CoolingRate},{r.Seed}," +
                              $"{r.Solved},{r.BestCost},{r.Iterations},{r.TimeMs}");
        }
    }
}

// using System.Globalization;

// namespace KillerSudokuSolver.Testing;

// public static class ResultsWriter
// {
//     public static void WriteCsv(
//         List<TestRecord> records,
//         string path)
//     {
//         using var writer = new StreamWriter(path);

//         writer.WriteLine(
//             "PuzzleFile,DifficultyTier,CoolingRate,Seed,Solved,ValidationPassed,BestCost,Iterations,TimeMs");

//         foreach (var record in records)
//         {
//             writer.WriteLine(
//                 $"{record.PuzzleFile}," +
//                 $"{record.DifficultyTier}," +
//                 $"{record.CoolingRate.ToString(CultureInfo.InvariantCulture)}," +
//                 $"{record.Seed}," +
//                 $"{record.Solved}," +
//                 $"{record.ValidationPassed}," +
//                 $"{record.BestCost}," +
//                 $"{record.Iterations}," +
//                 $"{record.TimeMs}");
//         }
//     }
// }
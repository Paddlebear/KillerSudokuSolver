using System.Globalization;

namespace KillerSudokuSolver.Testing;

public static class ResultsWriter
{
    public static void WriteCsv(List<TestRecord> records, string path)
    {
        using var writer = new StreamWriter(path);
        writer.WriteLine(
            "PuzzleFile,DifficultyTier,CoolingRate,Seed,Solved,ValidationPassed,BestCost,Iterations,TimeMs,KnownAnswerMatches");

        foreach (var r in records)
        {
            writer.WriteLine(string.Join(",",
                Escape(r.PuzzleFile),
                Escape(r.DifficultyTier),
                r.CoolingRate.ToString(CultureInfo.InvariantCulture),
                r.Seed.ToString(CultureInfo.InvariantCulture),
                r.Solved.ToString(),
                r.ValidationPassed.ToString(),
                r.BestCost.ToString(CultureInfo.InvariantCulture),
                r.Iterations.ToString(CultureInfo.InvariantCulture),
                r.TimeMs.ToString(CultureInfo.InvariantCulture),
                r.KnownAnswerMatches.ToString()));
        }
    }

    private static string Escape(string value)
    {
        if (!value.Contains(',') && !value.Contains('"') &&
            !value.Contains('\r') && !value.Contains('\n'))
        {
            return value;
        }

        return $"\"{value.Replace("\"", "\"\"")}\"";
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
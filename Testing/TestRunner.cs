// using System.Diagnostics;
// using KillerSudokuSolver.IO;
// using KillerSudokuSolver.Model;
// using KillerSudokuSolver.Solver_v3;

// namespace KillerSudokuSolver.Testing;

// public class TestRecord
// {
//     public string PuzzleFile { get; set; } = "";
//     public string DifficultyTier { get; set; } = "";
//     public double CoolingRate { get; set; }
//     public int Seed { get; set; }
//     public bool Solved { get; set; }
//     public int BestCost { get; set; }
//     public int Iterations { get; set; }
//     public long TimeMs { get; set; }
// }

// public static class TestRunner
// {
//     public static List<TestRecord> RunAll(
//         string subsetRoot,
//         int[] seeds,
//         double[] coolingRates)
//     {
//         var results = new List<TestRecord>();
//         var files = Directory.GetFiles(subsetRoot, "*.killer", SearchOption.AllDirectories);

//         //var files = new[] { "data/subset/2/1263.killer", "data/subset/4/1374.killer", "data/subset/9/996.killer" };
//         //var files = new[] { "data/subset/9/996.killer" };
//         //var file = "data/subset/9/996.killer";

//         foreach (var file in files)
//     {
//         string tier = Path.GetFileName(Path.GetDirectoryName(file)!);
//         var puzzle = PuzzleLoader.Load(file);

//         foreach (var coolingRate in coolingRates)
//         {
//             foreach (var seed in seeds)
//             {
//                 var rng = new Random(seed);
//                 var sw = Stopwatch.StartNew();

//                 var result = SimulatedAnnealing.Solve(
//                     puzzle, rng, coolingRate: coolingRate, reheatTemp: 1.0, maxReheats: 25, maxIterations: 2_500_000);
//                 sw.Stop();

//                 results.Add(new TestRecord
//                 {
//                     PuzzleFile = Path.GetFileName(file),
//                     DifficultyTier = tier,
//                     CoolingRate = coolingRate,
//                     Seed = seed,
//                     Solved = result.Solved,
//                     BestCost = result.BestCost,
//                     Iterations = result.Iterations,
//                     TimeMs = sw.ElapsedMilliseconds
//                 });

//                 Console.WriteLine($"{tier}/{Path.GetFileName(file)} seed={seed} cr={coolingRate}: " +
//                                    $"solved={result.Solved} cost={result.BestCost} iters={result.Iterations} time={sw.ElapsedMilliseconds}ms");
//             }
//         }
//     }

//     return results;
//     }
// }

using System.Diagnostics;
using System.Globalization;
using KillerSudokuSolver.IO;
using KillerSudokuSolver.Model;
using KillerSudokuSolver.Solver_v4;

namespace KillerSudokuSolver.Testing;

public class TestRecord
{
    public string PuzzleFile { get; set; } = "";
    public string DifficultyTier { get; set; } = "";
    public double CoolingRate { get; set; }
    public int Seed { get; set; }
    public bool Solved { get; set; }
    public bool ValidationPassed { get; set; }
    public int BestCost { get; set; }
    public int Iterations { get; set; }
    public long TimeMs { get; set; }
    public bool KnownAnswerMatches { get; set; }
}

public static class TestRunner
{
    public static List<TestRecord> RunAll(
        string subsetRoot,
        int[] seeds,
        double[] coolingRates)
    {
        var results = new List<TestRecord>();
        var files = Directory.GetFiles(subsetRoot, "*.killer", SearchOption.AllDirectories);

        //var files = new[] { "data/subset/2/1263.killer", "data/subset/4/1374.killer", "data/subset/9/996.killer" };
        //var files = new[] { "data/subset/2/1263.killer" };

        foreach (var file in files)
        {
            string tier = Path.GetFileName(
                Path.GetDirectoryName(file)!);

            var puzzle = PuzzleLoader.Load(file);

            foreach (var coolingRate in coolingRates)
            {
                foreach (var seed in seeds)
                {
                    var rng = new Random(seed);
                    var stopwatch = Stopwatch.StartNew();

                    var result = SimulatedAnnealing.Solve(
                        puzzle,
                        rng,
                        coolingRate: coolingRate,
                        reheatTemp: 1.0,
                        maxReheats: 25,
                        maxIterations: 2_500_000);

                    stopwatch.Stop();

                    var answerPath = Path.ChangeExtension(file, ".ans");
                    // Console.WriteLine(
                    //     $"Loading answer: {answerPath} " +
                    //     $"exists={File.Exists(answerPath)}");

                    var knownAnswer = File.Exists(answerPath)
                        ? AnswerFileLoader.Load(answerPath)
                        : null;

                    result.ValidationPassed = FullGridValidator.IsValid(
                    result.BestGrid,
                    puzzle,
                    knownAnswer,
                    out var knownAnswerMatches);

                    result.KnownAnswerMatches = knownAnswerMatches;

                    results.Add(new TestRecord
                    {
                        PuzzleFile = Path.GetFileName(file),
                        DifficultyTier = tier,
                        CoolingRate = coolingRate,
                        Seed = seed,
                        Solved = result.Solved,
                        ValidationPassed = result.ValidationPassed,
                        KnownAnswerMatches = result.KnownAnswerMatches,
                        BestCost = result.BestCost,
                        Iterations = result.Iterations,
                        TimeMs = stopwatch.ElapsedMilliseconds
                    });

                    Console.WriteLine(
                        $"{tier}/{Path.GetFileName(file)} " +
                        $"seed={seed} coolingRate={coolingRate.ToString(CultureInfo.InvariantCulture)} " +
                        $"solved={result.Solved} " +
                        $"validation={result.ValidationPassed} " +
                        $"knownAnswer={result.KnownAnswerMatches} " +
                        $"cost={result.BestCost} " +
                        $"iterations={result.Iterations} " +
                        $"timeMs={stopwatch.ElapsedMilliseconds}");
                }
            }
        }

        return results;
    }
}
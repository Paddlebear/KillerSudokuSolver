using KillerSudokuSolver.Testing;
using KillerSudokuSolver.IO;

int[] seeds = [1, 42, 15, 82, 46, 74, 7, 23, 99, 5]; // 10 seeds per puzzle per cooling rate
//int[] seeds = [1, 42, 15];
double[] coolingRates = [0.9999, 0.99995];

//ValidatorTests.RunAll();

var results = TestRunner.RunAll("data/subset", seeds, coolingRates);

var resultFile = "results-v4-2.csv";
ResultsWriter.WriteCsv(results, resultFile);

Console.WriteLine($"\nDone. {results.Count} runs written to {resultFile}.");

//var puzzle = PuzzleLoader.Load("data/subset/9/996.killer");

// foreach (var seed in seeds) {
// var rng = new Random(seed);
// var result = SimulatedAnnealing.Solve(puzzle, rng, coolingRate: 0.9999, reheatTemp: 1.0, maxReheats: 30);
// Console.WriteLine($"Seed: {seed} Final cost: {result.BestCost}\n");

// foreach (var cage in puzzle.Cages)
// {
//     var values = cage.Cells.Select(c => result.BestGrid[c.Row, c.Col]).ToList();
//     int sum = values.Sum();
//     bool hasDupes = values.Distinct().Count() != values.Count;

//     if (sum != cage.Sum || hasDupes)
//     {
//         var cellStr = string.Join(", ", cage.Cells.Select(c => $"({c.Row},{c.Col})"));
//         Console.WriteLine($"VIOLATED: target={cage.Sum}, actual={sum}, dupes={hasDupes}, cells=[{cellStr}], values=[{string.Join(",", values)}]");
//     }
// }
// }

// using KillerSudokuSolver.IO;
// using KillerSudokuSolver.Model;
// using KillerSudokuSolver.Solver_v1;

// var puzzle = PuzzleLoader.Load("data/subset/9/996.killer");
// var rng = new Random(42);

// var grid = InitialStateGenerator.Generate(puzzle, rng);

// int trials = 1000;
// int noOpCount = 0;
// int boxBreakCount = 0;

// for (int i = 0; i < trials; i++)
// {
//     var result = MoveGenerator.SwapWithinCage(grid, puzzle, rng);

//     // did the move actually change anything?
//     bool changed = false;
//     for (int r = 0; r < Grid.Size && !changed; r++)
//         for (int c = 0; c < Grid.Size && !changed; c++)
//             if (result[r, c] != grid[r, c])
//                 changed = true;

//     if (!changed) noOpCount++;

//     var errors = result.ValidateBoxes();
//     if (errors.Count > 0)
//     {
//         boxBreakCount++;
//         Console.WriteLine($"Trial {i}: box validity broken!");
//         foreach (var e in errors) Console.WriteLine($"  - {e}");
//     }

//     grid = result; // carry forward so later trials act on an evolving grid, closer to real SA usage
// }

// Console.WriteLine($"\nOut of {trials} attempted cross-box swaps:");
// Console.WriteLine($"  No-op (no valid candidate found): {noOpCount}");
// Console.WriteLine($"  Box validity broken: {boxBreakCount}");
// Console.WriteLine($"  Successful valid swaps: {trials - noOpCount}");
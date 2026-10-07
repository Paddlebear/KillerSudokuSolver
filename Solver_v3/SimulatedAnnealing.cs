using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v3;

public class SaResult
{
    public Grid BestGrid { get; set; } = null!;
    public int BestCost { get; set; }
    public int Iterations { get; set; }
    public bool Solved => BestCost == 0;
}

public static class SimulatedAnnealing
{
    public static SaResult Solve(
    Puzzle puzzle, Random rng,
    double startTemp = 2.0, double coolingRate = 0.9999, double minTemp = 0.01,
    int maxIterations = 2_000_000,
    int stagnationLimit = 20_000,
    double reheatTemp = 1.0,       // how hot to jump back to
    int maxReheats = 30)           // cap total reheats so it can't loop forever
{
    var current = InitialStateGenerator.Generate(puzzle, rng);
    int currentCost = CostFunction.Calculate(current, puzzle);
    var best = current.Clone();
    int bestCost = currentCost;

    double temp = startTemp;
    int iteration = 0;
    int sinceImprovement = 0;
    int reheats = 0;

    while (bestCost > 0 && iteration < maxIterations)
    {
        var neighbor = MoveGenerator2.Move(current, puzzle, rng);
        int neighborCost = CostFunction.Calculate(neighbor, puzzle);
        int delta = neighborCost - currentCost;

        if (delta < 0 || rng.NextDouble() < Math.Exp(-delta / temp))
        {
            current = neighbor;
            currentCost = neighborCost;

            if (currentCost < bestCost)
            {
                best = current.Clone();
                bestCost = currentCost;
                sinceImprovement = 0;
            }
            else sinceImprovement++;
        }
        else sinceImprovement++;

        temp *= coolingRate;

        // instead of exiting on stagnation, reheat from the BEST state found so far
        if (sinceImprovement >= stagnationLimit && temp < minTemp)
        {
            if (reheats >= maxReheats) break;

            current = best.Clone();
            // apply a few random kicks to diversify from the exact same stuck state
            //v3.01 - increase resample chance to 0.6 to make it more likely to jump out of local minima
            for (int k = 0; k < 3; k++)
                current = MoveGenerator2.Move(current, puzzle, rng, resampleChance:0.6);

            currentCost = CostFunction.Calculate(current, puzzle);
            temp = reheatTemp;
            sinceImprovement = 0;
            reheats++;
        }

        iteration++;
    }

     return new SaResult { BestGrid = best, BestCost = bestCost, Iterations = iteration };
 }
//     public static SaResult SolveWithRestarts(
//     Puzzle puzzle, Random rng,
//     int maxRestarts = 5,
//     double startTemp = 2.0, double coolingRate = 0.9999, double minTemp = 0.01,
//     int maxIterations = 500_000,
//     int stagnationLimit = 20_000) // pass through
//     {
//         SaResult best = Solve(puzzle, rng, startTemp, coolingRate, minTemp, maxIterations, stagnationLimit);
//         int totalIterations = best.Iterations;

//         int attempt = 1;
//         while (!best.Solved && attempt < maxRestarts)
//         {
//             var next = Solve(puzzle, rng, startTemp, coolingRate, minTemp, maxIterations, stagnationLimit);
//             totalIterations += next.Iterations;
//             if (next.BestCost < best.BestCost)
//                 best = next;
//             attempt++;
//         }

//         best.Iterations = totalIterations;
//         return best;
//     }
}
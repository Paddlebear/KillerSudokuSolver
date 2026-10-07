using KillerSudokuSolver.Model;

namespace KillerSudokuSolver.Solver_v4;

public static class SimulatedAnnealing
{
    public static SaResult Solve(
        Puzzle puzzle,
        Random rng,
        double startTemp = 2.0,
        double coolingRate = 0.9999,
        double minTemp = 0.01,
        int maxIterations = 2_000_000,
        int stagnationLimit = 20_000,
        double reheatTemp = 1.0,
        int maxReheats = 30)
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

            if (delta < 0 ||
                rng.NextDouble() < Math.Exp(-delta / temp))
            {
                current = neighbor;
                currentCost = neighborCost;

                if (currentCost < bestCost)
                {
                    best = current.Clone();
                    bestCost = currentCost;
                    sinceImprovement = 0;
                }
                else
                {
                    sinceImprovement++;
                }
            }
            else
            {
                sinceImprovement++;
            }

            temp *= coolingRate;

            if (sinceImprovement >= stagnationLimit &&
                temp < minTemp)
            {
                if (reheats >= maxReheats)
                    break;

                current = best.Clone();

                for (int kick = 0; kick < 3; kick++)
                {
                    current = MoveGenerator2.Move(
                        current,
                        puzzle,
                        rng,
                        resampleChance: 0.6);
                }

                currentCost = CostFunction.Calculate(current, puzzle);
                temp = reheatTemp;
                sinceImprovement = 0;
                reheats++;
            }

            iteration++;
        }

        bool validationPassed = FullGridValidator.IsValid(best, puzzle);

        return new SaResult
        {
            BestGrid = best,
            BestCost = bestCost,
            Iterations = iteration,
            ValidationPassed = validationPassed
        };
    }
}
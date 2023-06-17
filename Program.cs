
using System.Collections.Generic;

namespace ConstraintSatisfactionProblem
{
    using System;
    using ConstraintSatisfactionProblem.Problems;

    public class Program
    {
        public static void Main(string[] args)
        {
            RunAndDisplayTime(() => new Einstein());

            for (int n = 1; n <= 10; ++n)
            {
                int size = n;
                RunAndDisplayTime(() => new NQueens(size));
            }

            RunAndDisplayTime(() => new Sudoku(new Dictionary<int, Dictionary<int, int>>()));
            RunAndDisplayTime(() => new Sudoku(GetSudoku()));

            Console.ReadLine();
        }

        private static void RunAndDisplayTime(Func<AbstractProblem> getProblem)
        {
            DateTime start = DateTime.UtcNow;
            AbstractProblem p = getProblem();
            p.Solve();
            DateTime end = DateTime.UtcNow;
            TimeSpan time = end - start;
            p.DisplaySolution();
            Console.WriteLine($"Time: {time.Milliseconds} millisecs");
            Console.WriteLine();
        }

        private static Dictionary<int, Dictionary<int, int>> GetSudoku()
        {
            return new Dictionary<int, Dictionary<int, int>>
            {
                [1] = new Dictionary<int, int> { [1] = 8 },
                [2] = new Dictionary<int, int> { [3] = 3, [4] = 6 },
                [3] = new Dictionary<int, int> { [2] = 7, [5] = 9, [7] = 2 },
                [4] = new Dictionary<int, int> { [2] = 5, [6] = 7 },
                [5] = new Dictionary<int, int> { [5] = 4, [6] = 5, [7] = 7 },
                [6] = new Dictionary<int, int> { [4] = 1, [8] = 3 },
                [7] = new Dictionary<int, int> { [3] = 1, [8] = 6, [9] = 8 },
                [8] = new Dictionary<int, int> { [3] = 8, [4] = 5, [8] = 1 },
                [9] = new Dictionary<int, int> { [2] = 9, [7] = 4 }
            };
        }
    }
}

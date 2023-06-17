
namespace ConstraintSatisfactionProblem.Problems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConstraintSatisfactionProblem.Solver;

    public class NQueens : AbstractProblem
    {
        public NQueens(int size)
        {
            if (size <= 0)
            {
                throw new Exception("ERROR! n must be positive!");
            }

            this.Size = size;
        }

        // Variables.
        private int Size { get; }

        private List<Variable> Queens { get; set; }
        
        public override void DisplaySolution()
        {
            if (!this.Solver.Solved)
            {
                Console.WriteLine("Problem have not been solved yet.");
            }
            else if (!this.Solver.FoundSolution)
            {
                Console.WriteLine("No solution found for n=" + this.Size);
            }
            else
            {
                Console.WriteLine("Found solution for n=" + this.Size);
                for (int i = 0; i < this.Size; ++i)
                {
                    for (int j = 0; j < this.Size; ++j)
                    {
                        Console.Write(this.Queens[i].Value == j ? "*" : ".");
                    }

                    Console.WriteLine();
                }
            }
        }

        protected override void InitializeSolver()
        {
            List<int> domain = Enumerable.Range(0, this.Size).ToList();
            this.Queens = this.Solver.AddVariables(this.Size, domain, i => $"Queen #{i}");

            for (int i = 0; i < this.Size; ++i)
            {
                for (int j = i + 1; j < this.Size; ++j)
                {
                    this.Solver.Constraints.AddQueenAlignedConstraint(this.Queens[i], this.Queens[j], j - i);
                }
            }
        }
    }
}

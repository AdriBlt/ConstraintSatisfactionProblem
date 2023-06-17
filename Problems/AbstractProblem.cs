
namespace ConstraintSatisfactionProblem.Problems
{
    using ConstraintSatisfactionProblem.Solver;

    public abstract class AbstractProblem
    {
        protected AbstractProblem()
        {
            this.Solver = new Solver();
        }

        protected Solver Solver { get; }

        public bool Solve()
        {
            this.InitializeSolver();
            return this.Solver.Solve();
        } 

        public bool HasBeenSolved() => this.Solver.Solved;

        public bool HasFoundSolution() => this.Solver.FoundSolution;

        public abstract void DisplaySolution();

        protected abstract void InitializeSolver();
    }
}

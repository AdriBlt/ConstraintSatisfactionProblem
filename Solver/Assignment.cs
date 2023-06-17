
namespace ConstraintSatisfactionProblem.Solver
{
    public class Assignment
    {
        public Assignment(Variable var, int val)
        {
            this.Variable = var;
            Value = val;
        }

        public Variable Variable { get; }

        public int Value { get; }
    }
}

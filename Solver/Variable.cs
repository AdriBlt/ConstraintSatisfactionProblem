
namespace ConstraintSatisfactionProblem.Solver
{
    using System.Collections.Generic;

    public class Variable
    {
        public List<int> Domain { get; set; }

        public int? Value { get; set; }

        public string Name { get; set; }
    }
}

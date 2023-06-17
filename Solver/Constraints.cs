
namespace ConstraintSatisfactionProblem.Solver
{
    using System.Collections.Generic;
    using System.Linq;

    public partial class Constraints
    {
        public Constraints()
        {
            Values = new Dictionary<Variable, Dictionary<Variable, BinaryConstraintSet>>();
        }

        private Dictionary<Variable, Dictionary<Variable, BinaryConstraintSet>> Values { get; }

        internal BinaryConstraintSet GetIfExistsConstraint(Variable var1, Variable var2)
        {
            Dictionary<Variable, BinaryConstraintSet> innerDictionary;
            if (this.Values.TryGetValue(var1, out innerDictionary))
            {
                BinaryConstraintSet constraint;
                if (innerDictionary.TryGetValue(var2, out constraint))
                {
                    return constraint;
                }
            }

            return null;
        }

        internal IEnumerable<KeyValuePair<Variable, BinaryConstraintSet>> GetNeighbors(Variable variable)
        {
            Dictionary<Variable, BinaryConstraintSet> innerDictionary;
            if (!this.Values.TryGetValue(variable, out innerDictionary))
            {
                return Enumerable.Empty<KeyValuePair<Variable, BinaryConstraintSet>>();
            }

            return this.Values[variable].AsEnumerable();
        }

        internal void InitializeCurrentSupportSize()
        {
            foreach (Dictionary<Variable, BinaryConstraintSet> dictionary in this.Values.Values)
            {
                foreach (BinaryConstraintSet constraint in dictionary.Values)
                {
                    constraint.InitializeCurrentSupportSize();
                }
            }
        }
    }
}


namespace ConstraintSatisfactionProblem.Solver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    class BinaryConstraintSet
    {
        public BinaryConstraintSet(Variable var1, Variable var2)
        {
            this.Variable1 = var1;
            this.Variable2 = var2;
            this.Constraints = new List<Func<int, int, bool>>();
        }

        private Variable Variable1 { get; }

        private Variable Variable2 { get; }

        private List<Func<int, int, bool>> Constraints { get; }

        // CurrentSupportSize[vi] == #{vj \in domain[j] | (vi,vj) \in R{i,j}}
        private Dictionary<int, int> CurrentSupportSize { get; set; }

        private BinaryConstraintValues Values { get; set; }

        internal void AddConstraint(Func<int, int, bool> constraint)
        {
            this.Constraints.Add(constraint);
        }

        internal IEnumerable<int> GetSupport(int value)
        {
            return Values.GetSupport(value);
        }

        // support(i,j,vi) += var
        internal int ChangeCurrentSupportSize(int value, int delta)
        {
            return this.CurrentSupportSize[value] = this.CurrentSupportSize[value] + delta;
        }

        internal int GetCurrentSupportSize(int value)
        {
            return this.CurrentSupportSize[value];
        }

        internal void InitializeCurrentSupportSize()
        {
            this.Values = new BinaryConstraintValues();
            foreach(int value1 in this.Variable1.Domain)
            {
                foreach(int value2 in this.Variable2.Domain)
                {
                    if (!this.Constraints.Any() || this.Constraints.All(func => func(value1, value2)))
                    {
                        this.Values.Add(value1, value2);
                    }
                }
            }

            this.CurrentSupportSize = new Dictionary<int, int>();
            foreach (int value1 in this.Variable1.Domain)
            {
                this.CurrentSupportSize.Add(value1, this.GetSupport(value1).Count());
            }
        }
    }
}

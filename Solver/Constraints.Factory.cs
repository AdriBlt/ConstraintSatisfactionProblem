
namespace ConstraintSatisfactionProblem.Solver
{
    using System;
    using System.Collections.Generic;

    public partial class Constraints
    {
        public void AddAllDifferentConstraints(List<Variable> variables)
        {
            for (int i = 0; i < variables.Count; i++)
            {
                Variable variable = variables[i];
                for (int j = i + 1; j < variables.Count; j++)
                {
                    this.AddDifferentConstraint(variable, variables[j]);
                }
            }
        }

        public void AddDifferentConstraint(Variable var1, Variable var2)
        {
            this.AddConstraint(var1, var2, (i, j) => i != j);
        }

        public void AddEqualConstraint(Variable var1, Variable var2)
        {
            this.AddConstraint(var1, var2, (i, j) => i == j);
        }

        public void AddNeighborsConstraint(Variable var1, Variable var2)
        {
            this.AddConstraint(var1, var2, (i, j) => i == j + 1 || i == j - 1);
        }

        public void AddDeltaValueConstraint(Variable var1, Variable var2, int delta)
        {
            this.AddConstraint(var1, var2, (i, j) => j == i + delta);
        }

        public void AddQueenAlignedConstraint(Variable var1, Variable var2, int absDelta)
        {
            this.AddConstraint(var1, var2, (i, j) => i != j && i - j != absDelta && j - i != absDelta);
        }

        private void AddConstraint(Variable var1, Variable var2, Func<int, int, bool> allow)
        {
            if (var1 == null || var2 == null)
            {
                throw new Exception("ERROR! Invalid variable index.");
            }
            this.GetOrCreateConstraint(var1, var2).AddConstraint(allow);
            this.GetOrCreateConstraint(var2, var1).AddConstraint((i, j) => allow(j, i));
        }

        private BinaryConstraintSet GetOrCreateConstraint(Variable var1, Variable var2)
        {
            Dictionary<Variable, BinaryConstraintSet> innerDictionary;
            if (!this.Values.TryGetValue(var1, out innerDictionary))
            {
                innerDictionary = new Dictionary<Variable, BinaryConstraintSet>();
                this.Values[var1] = innerDictionary;
            }

            BinaryConstraintSet constraint;
            if (!innerDictionary.TryGetValue(var2, out constraint))
            {
                constraint = new BinaryConstraintSet(var1, var2);
                innerDictionary[var2] = constraint;
            }

            return constraint;
        }

        class ConstraintValue
        {
            public ConstraintValue(int value1, int value2)
            {
                this.First = value1;
                this.Second = value2;
            }

            public int First { get; }

            public int Second { get; }
        }
    }
}

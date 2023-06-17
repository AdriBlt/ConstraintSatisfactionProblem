
namespace ConstraintSatisfactionProblem.Solver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class Solver
    {
        public Solver()
        {
            this.Solved = false;
            this.Constraints = new Constraints();
            this.FreeVariables = new List<Variable>();
            this.Variables = new List<Variable>();
        }

        public List<Variable> Variables { get; set; }

        // Binary constraints.
        public Constraints Constraints { get; set; }

        // Has the solve() function been called?
        public bool Solved { get; set; }

        // If solve() has been called, has a solution been found?
        public bool FoundSolution { get; set; }
        
        // Free variables.
        private List<Variable> FreeVariables { get; }

        // Public functions.
        public List<Variable> AddVariables(int number, List<int> domain, Func<int, string> getVariableName = null)
        {
            List<Variable> variables = new List<Variable>();
            for (int i = 0; i < number; i++)
            {
                variables.Add(new Variable
                {
                    Domain = new List<int>(domain),
                    Name = getVariableName != null ? getVariableName(i) : string.Empty
                });
            }

            this.Variables.AddRange(variables);
            return variables;
        }

        // Solve the problem. Return true iff a solution is found.
        public bool Solve()
        {
            if (this.Solved)
            {
                throw new Exception("ERROR! The problem has already been solved.");
            }

            this.FreeVariables.Clear();
            this.FreeVariables.AddRange(this.Variables);

            this.Constraints.InitializeCurrentSupportSize();

            this.FoundSolution = this.Backtrack();
            this.Solved = true;
            return this.FoundSolution;
        }

        // Private functions.

        // Remove all arc inconsistencies. Return a list of forbiden assignments.
        private LinkedList<Assignment> Ac4()
        {
            LinkedList<Assignment> forbidenAssigments = this.GetForbidenAssigments();
            LinkedList<Assignment> assignments = new LinkedList<Assignment>();
            while (forbidenAssigments.Any())
            {
                Assignment assignment = forbidenAssigments.First.Value;
                forbidenAssigments.RemoveFirst();

                Variable variable = assignment.Variable;
                int value = assignment.Value;

                if (variable.Domain.Contains(value))
                {
                    assignments.AddLast(assignment);
                    variable.Domain.Remove(value);

                    foreach (KeyValuePair<Variable, BinaryConstraintSet> neighbor in this.Constraints.GetNeighbors(variable))
                    {
                        if (this.FreeVariables.Contains(neighbor.Key))
                        {
                            foreach (int vj in neighbor.Value.GetSupport(value))
                            {
                                var currentSize = this.Constraints
                                    .GetIfExistsConstraint(neighbor.Key, variable)
                                    ?.ChangeCurrentSupportSize(vj, -1);
                                if (currentSize == 0)
                                {
                                    forbidenAssigments.AddLast(new Assignment(neighbor.Key, vj));
                                }
                            }
                        }
                    }
                }
            }

            return assignments;
        }

        private LinkedList<Assignment> GetForbidenAssigments()
        {
            LinkedList<Assignment> assignments = new LinkedList<Assignment>();
            foreach (Variable freeVariable in this.FreeVariables)
            {
                foreach (KeyValuePair<Variable, BinaryConstraintSet> neighbor in this.Constraints.GetNeighbors(freeVariable))
                {
                    foreach (int value in freeVariable.Domain)
                    {
                        if (neighbor.Value.GetCurrentSupportSize(value) == 0)
                        {
                            assignments.AddLast(new Assignment(freeVariable, value));
                        }
                    }
                }
            }

            return assignments;
        }

        // Try to extend the partial assignment.
        // Return true iff a solution is found.
        private bool Backtrack()
        {
            if (!this.FreeVariables.Any())
            {
                return true;
            }

            LinkedList<Assignment> removedAssignments = this.Ac4();
            Variable freeVariable = this.GetFreeVariable();
            foreach (int vi in freeVariable.Domain)
            {
                this.Assign(freeVariable, vi);
                if (this.Backtrack())
                {
                    return true;
                }
                else
                {
                    this.Unassign(freeVariable);
                }
            }

            this.RecoverAssignments(removedAssignments);
            return false;
        }
        
        private void RecoverAssignments(LinkedList<Assignment> assignments)
        {
            foreach (Assignment a in assignments)
            {
                Variable variable = a.Variable;
                int value = a.Value;
                variable.Domain.Add(value);
                foreach (KeyValuePair<Variable, BinaryConstraintSet> neighbor in this.Constraints.GetNeighbors(variable))
                {
                    if (this.FreeVariables.Contains(neighbor.Key))
                    {
                        foreach (int vj in neighbor.Value.GetSupport(value))
                        {
                            this.Constraints.GetIfExistsConstraint(neighbor.Key, variable)
                                ?.ChangeCurrentSupportSize(vj, +1);
                        }
                    }
                }
            }
        }

        // x[i] = vi
        private void Assign(Variable variable, int value)
        {
            variable.Value = value;
            this.FreeVariables.Remove(variable);
            foreach (int value1 in variable.Domain)
            {
                if (value1 != value)
                {
                    foreach (KeyValuePair<Variable, BinaryConstraintSet> neighbor in this.Constraints.GetNeighbors(variable))
                    {
                        if (this.FreeVariables.Contains(neighbor.Key))
                        {
                            BinaryConstraintSet constraint = this.Constraints.GetIfExistsConstraint(neighbor.Key, variable);
                            if (constraint == null)
                            {
                                continue;
                            }

                            foreach (int value2 in neighbor.Key.Domain)
                            {
                                if (constraint.GetSupport(value2).Contains(value1))
                                {
                                    constraint.ChangeCurrentSupportSize(value2, -1);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void Unassign(Variable variable)
        {
            this.FreeVariables.Add(variable);
            foreach (int value in variable.Domain)
            {
                if (value != variable.Value)
                {
                    foreach (KeyValuePair<Variable, BinaryConstraintSet> neighbor in this.Constraints.GetNeighbors(variable))
                    {
                        if (this.FreeVariables.Contains(neighbor.Key))
                        {
                            BinaryConstraintSet constraint = this.Constraints.GetIfExistsConstraint(neighbor.Key, variable);
                            if (constraint == null)
                            {
                                continue;
                            }

                            foreach (int value2 in variable.Domain)
                            {
                                if (constraint.GetSupport(value2).Contains(value))
                                {
                                    constraint.ChangeCurrentSupportSize(value2, +1);
                                }
                            }
                        }
                    }
                }
            }
        }

        private Variable GetFreeVariable()
        {
            Variable bestVariable = null;
            int bestSize = -1;
            foreach (Variable variable in this.FreeVariables)
            {
                if (variable.Domain.Count > bestSize)
                {
                    bestVariable = variable;
                    bestSize = variable.Domain.Count;
                }
            }

            return bestVariable;
        }
    }
}
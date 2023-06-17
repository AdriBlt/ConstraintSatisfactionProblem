
using System.Globalization;

namespace ConstraintSatisfactionProblem.Problems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConstraintSatisfactionProblem.Solver;

    public class Sudoku : AbstractProblem
    {
        public Sudoku(Dictionary<int, Dictionary<int, int>> initialValues)
        {
            this.InitialValues = initialValues;
        }

        public Dictionary<int, Dictionary<int, int>> InitialValues { get; set; }

        private List<Variable> Variables { get; set; }

        public Variable GetVariable(int i, int j)
        {
            if (i < 1 || i > 9 || j < 1 || j > 9)
            {
                throw new IndexOutOfRangeException($"GetVariable({i}, {j})");
            }

            return this.Variables[9 * (i - 1) + j - 1];
        }

        public override void DisplaySolution()
        {
            if (!this.Solver.Solved)
            {
                Console.WriteLine("Sudoku: Problem have not been solved yet.");
            }
            else if (!this.Solver.FoundSolution)
            {
                Console.WriteLine("Sudoku: No solution found.");
            }
            else
            {
                Console.WriteLine("Found solution!");
                for (int i = 1; i < 10; ++i)
                {
                    string line = string.Empty;
                    for (int j = 1; j < 10; j++)
                    {
                        line += this.GetVariable(i, j).Value?.ToString() ?? " ";
                        if (j == 3 || j == 6)
                        {
                            line += "|";
                        }
                    }

                    Console.WriteLine(line);
                    if (i == 3 || i == 6)
                    {
                        Console.WriteLine("---+---+---");
                    }
                }
            }
        }

        protected override void InitializeSolver()
        {
            this.Variables =this.Solver.AddVariables(81, Enumerable.Range(1, 9).ToList());
            this.AddConstraints();
            this.SetInitialValues();
        }

        private void AddConstraints()
        {
            for (int i = 1; i < 10; i++)
            {
                for (int j = 1; j < 10; j++)
                {
                    Variable variable = this.GetVariable(i, j);

                    // Row Constraint
                    for (int jj = j + 1; jj < 10; jj++)
                    {
                        this.Solver.Constraints.AddDifferentConstraint(variable, this.GetVariable(i, jj));
                    }

                    // Column Constraint
                    for (int ii = i + 1; ii < 10; ii++)
                    {
                        this.Solver.Constraints.AddDifferentConstraint(variable, this.GetVariable(ii, j));
                    }

                    // Block Constraint
                    int blockRow = (i - 1) / 3; // 0, 1 or 2
                    int blockColumn = (j - 1) / 3; // 0, 1 or 2
                    int blockColumnIndex = 3 * blockColumn + 1; // 1, 4 or 7
                    int nextBlockRowIndex = 3 * (blockRow + 1) + 1; // 4, 7 or 10
                    int nextBlockColumnIndex = 3 * (blockColumn + 1) + 1; // 4, 7 or 10

                    for (int jj = j + 1; jj < nextBlockColumnIndex; jj++)
                    {
                        this.Solver.Constraints.AddDifferentConstraint(variable, this.GetVariable(i, jj));
                    }

                    for (int ii = i + 1; ii < nextBlockRowIndex; ii++)
                    {
                        for (int jj = blockColumnIndex; jj < nextBlockColumnIndex; jj++)
                        {
                            this.Solver.Constraints.AddDifferentConstraint(variable, this.GetVariable(ii, jj));
                        }
                    }
                }
            }
        }

        private bool AreConnected(int i, int j, int ii, int jj)
        {
            if (i < 1 || i > 9 || j < 1 || j > 9 || ii < 1 || ii > 9 || jj < 1 || jj > 9)
            {
                throw new IndexOutOfRangeException($"AreConnected({i}, {j}, {ii}, {jj})");
            }

            return i == ii || j == jj
                || (((i - 1) / 3 == (ii - 1) / 3) && ((j - 1) / 3 == (jj - 1) / 3));
        }

        private void SetInitialValues()
        {
            foreach (KeyValuePair<int, Dictionary<int, int>> row in this.InitialValues.AsEnumerable())
            {
                foreach (KeyValuePair<int, int> column in row.Value.AsEnumerable())
                {
                    this.SetValue(row.Key, column.Key, column.Value);
                }
            }
        }

        private void SetValue(int row, int column, int value)
        {
            if (value < 1 || value > 9)
            {
                throw new ArgumentOutOfRangeException($"SetValue({row}, {column}, {value})");
            }

            this.GetVariable(row, column).Domain = new List<int>(1) { value };
        }

        private List<Variable> GetRow(int row)
        {
            if (row < 1 || row > 9)
            {
                throw new IndexOutOfRangeException($"GetRow({row})");
            }

            List<Variable> variables = new List<Variable>(9);
            for (int j = 1; j < 10; j++)
            {
                variables.Add(this.GetVariable(row, j));
            }

            return variables;
        }

        private List<Variable> GetColumn(int column)
        {
            if (column < 1 || column > 9)
            {
                throw new IndexOutOfRangeException($"GetColumn({column})");
            }

            List<Variable> variables = new List<Variable>(9);
            for (int i = 1; i < 10; i++)
            {
                variables.Add(this.GetVariable(i, column));
            }

            return variables;
        }

        private List<Variable> GetBlock(int block)
        {
            if (block < 1 || block > 9)
            {
                throw new IndexOutOfRangeException($"GetBlock({block})");
            }

            int blockLine = 1 + (block - 1) / 3;
            int blockColumn = 1 + (block - 1) % 3;
            List<Variable> variables = new List<Variable>(9);
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    variables.Add(this.GetVariable(blockLine + i, blockColumn + j));
                }
            }

            return variables;
        }
    }
}


namespace ConstraintSatisfactionProblem.Problems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using ConstraintSatisfactionProblem.Solver;

    // There are five houses.
    // 1. The Englishman lives in the red house.
    // 2. The Spaniard owns the dog.
    // 3. Coffee is drunk in the green house.
    // 4. The Ukrainian drinks tea.
    // 5. The green house is immediately to the right of the ivory house.
    // 6. The Old Gold smoker owns snails.
    // 7. Kools are smoked in the yellow house.
    // 8. Milk is drunk in the middle house.
    // 9. The Norwegian lives in the first house.
    // 10. The man who smokes Chesterfields lives in the house next to the man with the fox.
    // 11. Kools are smoked in the house next to the house where the horse is kept.
    // 12. The Lucky Strike smoker drinks orange juice.
    // 13. The Japanese smokes Parliaments.
    // 14. The Norwegian lives next to the blue house.
    // Now, who drinks water? Who owns the zebra?
    public class Einstein : AbstractProblem
    {
        public List<Variable> Nationalities { get; set; }

        public List<Variable> Colors { get; set; }

        public List<Variable> Drinks { get; set; }

        public List<Variable> Smokes { get; set; }

        public List<Variable> Pets { get; set; }

        private Dictionary<string, Variable> Variables { get; set; }

        public Variable GetVariable(string name)
        {
            Variable variable;
            if (this.Variables.TryGetValue(name, out variable))
            {
                return variable;
            }

            throw new KeyNotFoundException(name);
        }

        public override void DisplaySolution()
        {
            if (!this.Solver.Solved)
            {
                Console.WriteLine("Problem have not been solved yet.");
            }
            else if (!this.Solver.FoundSolution)
            {
                Console.WriteLine("No solution found.");
            }
            else
            {
                Console.WriteLine("Found solution!");
                for (int i = 0; i < 5; ++i)
                {
                    int maison = i;
                    Console.WriteLine("House #" + i);
                    Console.WriteLine("Nationality: " + this.Nationalities.Single(var => var.Value == maison).Name);
                    Console.WriteLine("Color: " + this.Colors.Single(var => var.Value == maison).Name);
                    Console.WriteLine("Drink: " + this.Drinks.Single(var => var.Value == maison).Name);
                    Console.WriteLine("Smoke: " + this.Smokes.Single(var => var.Value == maison).Name);
                    Console.WriteLine("Pet: " + this.Pets.Single(var => var.Value == maison).Name);
                    Console.WriteLine();
                }
            }
        }

        protected override void InitializeSolver()
        {
            this.Nationalities = this.AddVariables(new[] { "Norwegian", "Englishman", "Spaniard", "Ukrainian", "Japanese" });
            this.Colors = this.AddVariables(new[] { "Blue", "Red", "Green", "Ivory", "Yellow" });
            this.Drinks = this.AddVariables(new[] { "Milk", "Coffee", "Tea", "Wine", "Orange Juice" });
            this.Smokes = this.AddVariables(new[] { "Kools", "Lucky Strike", "Old Gold", "Parliaments", "Chesterfields" });
            this.Pets = this.AddVariables(new[] { "Dog", "Snails", "Fox", "Horse", "Zebra" });

            List<Variable> variables = new List<Variable>();
            variables.AddRange(this.Nationalities);
            variables.AddRange(this.Colors);
            variables.AddRange(this.Drinks);
            variables.AddRange(this.Smokes);
            variables.AddRange(this.Pets);
            this.Variables = variables.ToDictionary(var => var.Name);

            // All Different
            this.Solver.Constraints.AddAllDifferentConstraints(this.Nationalities);
            this.Solver.Constraints.AddAllDifferentConstraints(this.Colors);
            this.Solver.Constraints.AddAllDifferentConstraints(this.Drinks);
            this.Solver.Constraints.AddAllDifferentConstraints(this.Smokes);
            this.Solver.Constraints.AddAllDifferentConstraints(this.Pets);

            // 1. The Englishman lives in the red house.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Englishman"), this.GetVariable("Red"));

            // 2. The Spaniard owns the dog.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Spaniard"), this.GetVariable("Dog"));

            // 3. Coffee is drunk in the green house.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Coffee"), this.GetVariable("Green"));

            // 4. The Ukrainian drinks tea.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Ukrainian"), this.GetVariable("Tea"));

            // 5. The green house is immediately to the right of the ivory house.
            this.Solver.Constraints.AddDeltaValueConstraint(this.GetVariable("Green"), this.GetVariable("Ivory"), 1);

            // 6. The Old Gold smoker owns snails.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Old Gold"), this.GetVariable("Snails"));

            // 7. Kools are smoked in the yellow house.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Kools"), this.GetVariable("Yellow"));

            // 8. Milk is drunk in the middle house.
            this.GetVariable("Milk").Domain = new List<int>(1) { 2 };

            // 9. The Norwegian lives in the first house.
            this.GetVariable("Norwegian").Domain = new List<int>(1) { 0 };

            // 10. The man who smokes Chesterfields lives in the house next to the man with the fox.
            this.Solver.Constraints.AddNeighborsConstraint(this.GetVariable("Chesterfields"), this.GetVariable("Fox"));

            // 11. Kools are smoked in the house next to the house where the horse is kept.
            this.Solver.Constraints.AddNeighborsConstraint(this.GetVariable("Kools"), this.GetVariable("Horse"));

            // 12. The Lucky Strike smoker drinks orange juice.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Lucky Strike"), this.GetVariable("Orange Juice"));

            // 13. The Japanese smokes Parliaments.
            this.Solver.Constraints.AddEqualConstraint(this.GetVariable("Japanese"), this.GetVariable("Parliaments"));

            // 14. The Norwegian lives next to the blue house.
            this.Solver.Constraints.AddNeighborsConstraint(this.GetVariable("Norwegian"), this.GetVariable("Blue"));
        }

        private List<Variable> AddVariables(string[] names)
        {
            return this.Solver.AddVariables(5, Enumerable.Range(0, 5).ToList(), i => names[i]);
        }
    }
}

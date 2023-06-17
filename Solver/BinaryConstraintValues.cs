
namespace ConstraintSatisfactionProblem.Solver
{
    using System.Collections.Generic;

    public class BinaryConstraintValues
    {
        public BinaryConstraintValues()
        {
            this.Values = new Dictionary<int, List<int>>();
        }

        private Dictionary<int, List<int>> Values { get; }

        public void Add(int vi, int vj)
        {
            List<int> list;
            if (!this.Values.TryGetValue(vi, out list))
            {
                list = new List<int>();
                this.Values.Add(vi, list);
            }

            list.Add(vj);
        }

        public bool HasSupport(int vi)
        {
            return this.Values.ContainsKey(vi);
        }

        public int GetSupportSize(int vi)
        {
            List<int> list;
            if (!this.Values.TryGetValue(vi, out list))
            {
                return 0;
            }

            return list.Count;
        }

        public List<int> GetSupport(int vi)
        {
            List<int> list;
            if (!this.Values.TryGetValue(vi, out list))
            {
                return new List<int>();
            }

            return list;
        }
    }
}

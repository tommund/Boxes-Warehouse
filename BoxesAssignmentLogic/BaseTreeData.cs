using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{
    class BaseTreeData : IComparable<BaseTreeData>
    {
        public double Base { get; internal set; }
        public Bst<DataForInnerHeightTree> InnerTree { get; internal set; }

        public BaseTreeData(double @base)
        {
            Base = @base;
        }
        public int CompareTo(BaseTreeData other)
        {
            if (other != null) return this.Base.CompareTo(other.Base);
            throw new ArgumentNullException("the box object is null!");
        }
    }
}

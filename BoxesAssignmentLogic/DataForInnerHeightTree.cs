using BoxesAssignmentLogic.dataStructres;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{

    // class to store data of a box without the base, and give the connection to the time linklist

    // has to implement icomparble so we will be able to navigate in the binary tree
    class DataForInnerHeightTree : IComparable<DataForInnerHeightTree>
    {
        public double Height { get; internal set; }
        public int CurrentQuantity { get;  internal set;}
        public DoublyLinkList<TimeData>.Node DoublyLinkListNodeRef { get;internal set; }

        public DataForInnerHeightTree(double height) 
        {
            Height = height;
        }

        public DataForInnerHeightTree(double height,int quantity)
        {
            Height = height;
            CurrentQuantity = quantity;
        }

        public int CompareTo(DataForInnerHeightTree other)
        {
            if (other != null) return this.Height.CompareTo(other.Height);
            throw new ArgumentNullException("the box object is null!");
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{
    class TimeData
    {
        // class that holds data for the time LinkList.. for the delete due time action
        public TimeData(double @base, double height)
        {
            Base = @base;
            Height = height;
            LastPurchaseDate = DateTime.Now;
        }

        internal double Base { get; set; }
        internal double Height { get; set; }
        internal DateTime LastPurchaseDate { get; set;}
    }
}

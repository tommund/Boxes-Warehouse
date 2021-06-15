using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{
    internal class Box 
    {
        public Box(BaseTreeData baseandTree, DataForInnerHeightTree heightData)
        {
            BaseandTree = baseandTree;
            HeightData = heightData;
        }

        public BaseTreeData BaseandTree { get; set; }
        public DataForInnerHeightTree HeightData { get; set; }
        

       

        
    }
}

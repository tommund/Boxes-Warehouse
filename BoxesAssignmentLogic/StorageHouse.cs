using BoxesAssignmentLogic.dataStructres;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BoxesAssignmentLogic
{
    class StorageHouse
    {
        private Bst<BaseTreeData> _dataTree;
        internal Bst<BaseTreeData> Bst
        {
            get { return _dataTree; }
        }
        public StorageHouse()
        {
            _dataTree = new Bst<BaseTreeData>();
        }
        
        // add item ar update data when needed using IsExistAndUpdate function
        internal bool IsInStockHandleSupply(ref BaseTreeData baseTreeData, ref DataForInnerHeightTree heightTree, int quantityToAdd)
        {
            bool isbaseExist = _dataTree.IsExistAndUpdate(ref baseTreeData);
            if (baseTreeData.InnerTree == null) baseTreeData.InnerTree = new Bst<DataForInnerHeightTree>();

            bool isHeightExist = baseTreeData.InnerTree.IsExistAndUpdate(ref heightTree);
            // if item in storage and allready exist fill the quantity
            if (isbaseExist && isHeightExist) return true;
            
            return false;
        }

        internal bool IsExist(ref BaseTreeData baseTreeData, ref DataForInnerHeightTree heightTree)
        {
            //data is not exist
            if (!_dataTree.IsExistAndShow(ref baseTreeData)) return false;

            //if base and height exist return data 
            return baseTreeData.InnerTree.IsExistAndShow(ref heightTree); 
        }

        internal bool IsFindMatchingBox( BaseTreeData customerBase, DataForInnerHeightTree customerHeight,ref BaseTreeData returnBestBase, ref DataForInnerHeightTree returnBestHeight)
        {
            while (true)
            {
                if (!_dataTree.IsFindBestMatch( customerBase,ref returnBestBase)) return false;
                if (returnBestBase.InnerTree.IsFindBestMatch( customerHeight,ref returnBestHeight)) return true;
               
                //this is how we itirate the next inner tree by saving the last place we were visited when didnt find a match
                customerBase.Base = Configuration.DeltaSentitivity + returnBestBase.Base;
            }
        }

        internal void GetSuitableBoxesForPresent(BaseTreeData customerBase, DataForInnerHeightTree customerHeightData,ref LinkedList<Box> Bestboxes)
        {
            BaseTreeData baseInStorage = null;
            DataForInnerHeightTree heightDataInStorage = null;

            //keep origin height from user to itirate foreach relevant base
            double originHeight = customerHeightData.Height; 
            while(true)
            {
                // try to find the best exist base in storage,stop when didnt find suitable base
                if (!_dataTree.IsFindBestMatch(customerBase, ref baseInStorage  )) return;   
                customerHeightData.Height = originHeight;

                // try to find the best exist height in storage
                while (baseInStorage.InnerTree.IsFindBestMatch(customerHeightData, ref heightDataInStorage)) 
                {
                    if (heightDataInStorage.CurrentQuantity <= customerHeightData.CurrentQuantity)
                    {
                        Configuration.CurrentSplits--;
                        customerHeightData.CurrentQuantity -= heightDataInStorage.CurrentQuantity;

                    }
                    else customerHeightData.CurrentQuantity = 0;

                    // add to linklist relevant boxes acording to user input 
                    Bestboxes.AddLast(new Box(baseInStorage, heightDataInStorage)); 

                    //condition when to stop consider quantity and amount of splits
                    if (customerHeightData.CurrentQuantity == 0 || Configuration.CurrentSplits == 0) return;

                    //help itirate through the inner hieght tree  for more relevant options
                    customerHeightData.Height = Configuration.DeltaSentitivity + heightDataInStorage.Height; 
                }
                //help itirate through the base tree for other relevant options
                customerBase.Base = Configuration.DeltaSentitivity + baseInStorage.Base; 
            }
        }
    }
}

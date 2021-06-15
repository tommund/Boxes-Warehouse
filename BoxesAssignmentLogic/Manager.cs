using BoxesAssignmentLogic.dataStructres;
using BoxesAssignmentLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace BoxesAssignmentLogic
{
    public class Manager
    {
        private DoublyLinkList<TimeData> _DoublyLinkTimeList;
        private StorageHouse _myStorageHouse;
        private IUserCommunication _user;
        private Idesigner _Idesigner;
        private const int frequencyCheckTimeBYHour = 12;
        private const int TimeDiffrenceToDeleteBoxesBYDayas = 30;
        private bool _isDuringDeal = false;
        
        //constructor
        public Manager(IUserCommunication user, Idesigner Idesigner)
        {
            _Idesigner = Idesigner;
            _user = user;
            _myStorageHouse = new StorageHouse();
            _DoublyLinkTimeList = new DoublyLinkList<TimeData>();
            TimeHandler();
        }

        //delay time event if user didnt give an answer yet
        private void WaitForUserAction()
        {
            while(_isDuringDeal)
            {
                Thread.Sleep(5);
            }
        }

        // initizliaze timer and connection to event
        private void TimeHandler()
        {
            Timer aTimer = new Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            // make the interval elapse every "frequencyCheckTimeBYHour" hours
            //  1000 milliseconds * 60 seconds * 60 minuets = one hour
            aTimer.Interval = 1000 * 60 * 60 * frequencyCheckTimeBYHour;
            aTimer.Enabled = true;
        }
        //Design Initizliazer
        public void DesignInitizliazer(bool isCursorVisible, string title)
        {
            _Idesigner.DesignInitizliazer(isCursorVisible,title);
        }

        // time event
        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            // wait until user give his decision
            Task.WaitAll(Task.Run(WaitForUserAction));
            // check if linklist is empty, and check if last value in linklist need to be deleted(fifo) 
            while (_DoublyLinkTimeList.Start != null && e.SignalTime.Subtract(_DoublyLinkTimeList.Last._data.LastPurchaseDate).TotalDays > TimeDiffrenceToDeleteBoxesBYDayas)
            {
                //delete last value from linklist
                _DoublyLinkTimeList.RemoveLast(out TimeData dataToRemove);
                BaseTreeData baseTreeData = new BaseTreeData(dataToRemove.Base);
                DataForInnerHeightTree dataForInnerHeight = new DataForInnerHeightTree(dataToRemove.Height);

                if(_myStorageHouse.IsExist(ref baseTreeData, ref dataForInnerHeight))
                {
                    //delete from tree
                    baseTreeData.InnerTree.Delete(dataForInnerHeight);
                    if (baseTreeData.InnerTree.IsEmptyTree()) _myStorageHouse.Bst.Delete(baseTreeData);
                    //update user about the delete
                    _user.InformUserdeleteDueTime(dataToRemove.Base, dataToRemove.Height);
                }
            }
        }
        // add supply or update quantity if needed
        public void UpdateStock(double @base, double height, int quantity)
        {
            //validate and iniziliaze data
            try
            {
                Configuration.ValidatePositiveNumbers(@base, height, quantity);
            }
            catch(ArgumentException e)
            {
                throw e;
            }
            
            Configuration.DeltaValidation(ref @base, ref height);
            BaseTreeData baseTreeData = new BaseTreeData(@base);
            DataForInnerHeightTree dataForInnerHeight = new DataForInnerHeightTree(height, quantity);

           
            if (!_myStorageHouse.IsInStockHandleSupply(ref baseTreeData, ref dataForInnerHeight, quantity))
            {
                // new item has created, initialze as first item in link list to syncranize the time issue and connect to inner tree
                SyncranizeTreeAndTimeList(baseTreeData.Base, dataForInnerHeight);

                if(quantity > Configuration.MaxQuantity)
                {
                    // notify user
                    _user.TellUserWeReachedMaxQuantity(Configuration.MaxQuantity, Configuration.MaxQuantity, quantity - Configuration.MaxQuantity);
                    // change current quantity
                    dataForInnerHeight.CurrentQuantity = Configuration.MaxQuantity;
                }
       
            }
            // item is allreasy exist in our storage
            else FillQuantity(dataForInnerHeight, quantity);
            _user.TellUserActionCompleted();
            
        }

        // if a box allready exist,add quantity
        private void FillQuantity(DataForInnerHeightTree heightTree, int quantityToAdd)
        {
            int originQuantityToAdd = quantityToAdd;
            heightTree.CurrentQuantity += quantityToAdd;
            if (heightTree.CurrentQuantity > Configuration.MaxQuantity)
            {
                int addToStock = originQuantityToAdd - (heightTree.CurrentQuantity - Configuration.MaxQuantity);
                _user.TellUserWeReachedMaxQuantity(addToStock, Configuration.MaxQuantity, heightTree.CurrentQuantity - Configuration.MaxQuantity);
                heightTree.CurrentQuantity = Configuration.MaxQuantity;
            }
        }

        // show data of a specifiec box if exist
        public void FindBox(double @base, double height)
        {
            //validate and iniziliaze data
            try
            {
                Configuration.ValidatePositiveNumbers(@base, height);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            BaseTreeData baseTreeData = new BaseTreeData(@base);
            DataForInnerHeightTree dataForInnerHeight = new DataForInnerHeightTree(height);

            // show box data!  
            if (_myStorageHouse.IsExist(ref baseTreeData, ref dataForInnerHeight))
            {
                _user.TellUserItemWasFound();
                _user.PresentDataOfBox(baseTreeData.Base, dataForInnerHeight.Height, dataForInnerHeight.CurrentQuantity, dataForInnerHeight.DoublyLinkListNodeRef._data.LastPurchaseDate);
            }
            else _user.TellUserNoBoxFound();
            _user.TellUserActionCompleted();
        }

        public void GetBestBoxForPresent(double @base, double height) // get a match for a present
        {
            //validate and iniziliaze data
            try
            {
                Configuration.ValidatePositiveNumbers(@base, height);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            BaseTreeData baseTreeData = new BaseTreeData(@base);
            DataForInnerHeightTree dataForInnerHeight = new DataForInnerHeightTree(height);
            BaseTreeData baseDataPresent = null;
            DataForInnerHeightTree HeightDataPresent = null;

            // get a box using IsFindMatchingBox function 
            if (_myStorageHouse.IsFindMatchingBox(baseTreeData, dataForInnerHeight, ref baseDataPresent, ref HeightDataPresent))
            {
                // after purchase make node to be first,last to be deleted by the timer  the user set, fifo
                UpdateQuantities(HeightDataPresent,1);
                _user.PresentDataOfBoxAfterPurchase(baseDataPresent.Base, HeightDataPresent.Height, HeightDataPresent.CurrentQuantity, DateTime.Now);
                ManageShortageAfterPurchase(baseDataPresent, HeightDataPresent);
            }
            else _user.TellUserNoBoxFound();
            _user.TellUserActionCompleted();
        }

        // incharge of consequences of the the purchase when needed
        private void ManageShortageAfterPurchase(BaseTreeData baseTreeDataPresent, DataForInnerHeightTree heightTreeDataPresent)
        {

            
            if (heightTreeDataPresent.CurrentQuantity < Configuration.MinQuantity)
                _user.InformUserAboutShortage(baseTreeDataPresent.Base, heightTreeDataPresent.Height, heightTreeDataPresent.CurrentQuantity);

            if (heightTreeDataPresent.CurrentQuantity == 0)
            {
                _user.TellUserNoBoxLeft();
                _DoublyLinkTimeList.DeleteNode(heightTreeDataPresent.DoublyLinkListNodeRef);
                baseTreeDataPresent.InnerTree.Delete(heightTreeDataPresent);
            }
            else
            {
                _DoublyLinkTimeList.DeleteNodeAndMakeFirst(heightTreeDataPresent.DoublyLinkListNodeRef);
                heightTreeDataPresent.DoublyLinkListNodeRef._data.LastPurchaseDate = DateTime.Now;
            }
            if (baseTreeDataPresent.InnerTree.IsEmptyTree()) _myStorageHouse.Bst.Delete(baseTreeDataPresent);
        }

        private void UpdateQuantities(DataForInnerHeightTree heightTreeDataPresent, int qunatityTosubstract)
        {
            if (qunatityTosubstract >= heightTreeDataPresent.CurrentQuantity) heightTreeDataPresent.CurrentQuantity = 0;
            else heightTreeDataPresent.CurrentQuantity -= qunatityTosubstract;
        }

        // get collection of boxes using GetSuitableBoxesForPresent function 
        public void GetBestBoxesForPresents(double @base, double height, int quantityToPurchase)
        {
            //validate and iniziliaze data
            try
            {
                Configuration.ValidatePositiveNumbers(@base, height,quantityToPurchase);
            }
            catch (ArgumentException e)
            {
                throw e;
            }
            BaseTreeData baseTreeData = new BaseTreeData(@base);
            DataForInnerHeightTree dataForInnerHeight = new DataForInnerHeightTree(height, quantityToPurchase);

            int quantity = quantityToPurchase;
            // dinamic collection to store requested boxes
            LinkedList<Box> boxes = new LinkedList<Box>();   

            // get a collection of best boxes option
            _myStorageHouse.GetSuitableBoxesForPresent(baseTreeData, dataForInnerHeight, ref boxes);

            // means we didnt find even one box macth with user's request,end function
            if (boxes.Count == 0)
            {
                _user.TellUserNoBoxFound();
                _user.TellUserActionCompleted();
                return;
            }

            // deal is on and waiting for user respond
            _isDuringDeal = true;
            // ask user if agree to offer, if didnt agree .. end function
            if (_user.IsUserAgreeToDeal(DataForUser(ref boxes, quantityToPurchase)))
            {
                // user agree to offer, means that we need to substract quantities  
                foreach (Box item in boxes)
                {
                    if (item.HeightData.CurrentQuantity - quantity < 0)
                    {
                        quantity -= item.HeightData.CurrentQuantity;
                        item.HeightData.CurrentQuantity = 0;
                    }
                    else item.HeightData.CurrentQuantity -= quantity;

                    // incharge of consequences of the the purchase when needed
                    ManageShortageAfterPurchase(item.BaseandTree, item.HeightData);
                }
            }
            //enable time event again
            _isDuringDeal = false;

            _user.TellUserActionCompleted();
        }

        // tostring of list of boxes to show the user
        private string DataForUser(ref LinkedList<Box> boxes, int quantityToPurchase)
        {
            string boxesData = "";
            int indexPerPresent = 1;
            boxesData += "best item/s that we have in storage suits to your rerquest :\n";
            foreach (Box item in boxes)
            {
                if (quantityToPurchase >= item.HeightData.CurrentQuantity)
                {
                    boxesData += _user.PresentDataOfBoxes(item.BaseandTree.Base, item.HeightData.Height, item.HeightData.CurrentQuantity, indexPerPresent);
                    quantityToPurchase -= item.HeightData.CurrentQuantity;
                }
                else boxesData += _user.PresentDataOfBoxes(item.BaseandTree.Base, item.HeightData.Height, quantityToPurchase, indexPerPresent);

                indexPerPresent++;
            }
            return boxesData;
        }
     
        // initialze as first item in link list to syncranize the time issue and connect to inner tree
        private void SyncranizeTreeAndTimeList(double @base, DataForInnerHeightTree heightData)
        {
            _DoublyLinkTimeList.AddFirstNewNode(new TimeData(@base, heightData.Height));
            //the connection between inner tree to the time link list
            heightData.DoublyLinkListNodeRef = _DoublyLinkTimeList.Start;
        }
    }
}

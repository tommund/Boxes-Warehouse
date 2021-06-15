using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic.Interfaces
{

    // interface to communicate with user
    public interface IUserCommunication
    {
        void TellUserNoBoxFound();
        void TellUserNoBoxLeft(); 
        void TellUserItemWasFound();
        void InformUserAboutShortage(double @base, double height,int quantity);
        void InformUserdeleteDueTime(double @base, double height);
        bool IsUserAgreeToDeal(string data);
        void TellUserWeReachedMaxQuantity(int originQuantityToAdd, int currentQuantity, int ConfigurationMaxQuantity);
        void TellUserActionCompleted();
        void PresentDataOfBox(double @base, double height, int quantity, DateTime purchaseDate);  // tostring of a box
        void PresentDataOfBoxAfterPurchase(double @base, double height, int quantity, DateTime purchaseDate);
       string PresentDataOfBoxes(double @base, double height, int quantity,int boxNumber);






    }
}

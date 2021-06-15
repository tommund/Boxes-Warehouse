using BoxesAssignmentLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{

    // implemention of the "IUserCommunication" interface, for notifing and communicate with user 
    public class ConsoleUser : IUserCommunication
    {
        public bool IsUserAgreeToDeal(string data)
        {
            Console.WriteLine(data);
           
            Console.WriteLine("if you are intersted with this purchase please type y,to decline press something else..");
            switch (Console.ReadLine())
            {
                case "y":
                    Console.WriteLine("thank u and have a good day..");
                    return true;
                   
                default:
                    Console.WriteLine("deal is off..");
                    return false;
            }   
        }
        public void InformUserAboutShortage(double @base, double height,int quantity)
        {
            Console.WriteLine($"box with base: {@base}, height:{height}\n"+
                $"current quantity is:{quantity} and it below the minimum in configuration => {Configuration.MinQuantity}");
        }
        public void InformUserdeleteDueTime(double @base, double height)
        {
            Console.WriteLine($"box has been removed because of time, base :{ @base},height :{ height} ");
        }
        public void TellUserItemWasFound() => Console.WriteLine("item is in store!"); 
        public void TellUserNoBoxFound() => Console.WriteLine("sorry,we coudn't find suatible box/es in storage for your request"); 
        public void TellUserNoBoxLeft() =>  Console.WriteLine("after purchase the box is out of stock!");
        public void TellUserWeReachedMaxQuantity(int quantityToAdd ,int ConfigurationMaxQuantity , int remainingstock)
        {
            Console.WriteLine($"we are adding to stock only {quantityToAdd} unit/s" +
                     $"because the limit that the storage can hold is {ConfigurationMaxQuantity}, the remaining {remainingstock} boxes are yours");
        }
        public void TellUserActionCompleted()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("action is over");
            Console.ForegroundColor = ConsoleColor.White;

        }
        public void PresentDataOfBox(double @base, double height, int quantity, DateTime purchaseDateOrUpdate)
        {
            Console.WriteLine($"information about box is :\n" +
                $"base :{@base}\n" +
                $"height :{height}\n" +
                $"available quantity in storage is :{quantity}\n" +
                $"last date that got into storage/ was bought buy a customer is :" +
                $"{purchaseDateOrUpdate}");
        }
        public string PresentDataOfBoxes(double @base, double height, int quantity,int numberOfAbox)
        {
            return $"information about box number {numberOfAbox} is :\n" +
                  $"base :{@base}\n" +
                  $"height :{height}\n" +
                  $"quantity to buy :{quantity}\n";
        }
        public void PresentDataOfBoxAfterPurchase(double @base, double height, int quantity, DateTime purchaseDate)
        {
            Console.WriteLine($"best box we found is :\n" +
                $"base :{@base}\n" +
                $"height :{height}\n" +
                $"available quantity in storage after purchase is :{quantity}\n" +
                $"date of purchase is:" +
                $"{purchaseDate}"); 
        }
    }
}

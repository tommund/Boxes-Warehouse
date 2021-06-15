using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{
    internal static class Configuration
    {
        private const int MAX_QUANTITY = 100;
        private const int MIN_QUANTITY = 3;
        private const int DIGIT_SENTITIVITY = 2;
        private const int MaxSplits = 3;
        public const int BOX_DAY_LIFE_TIME = 30;



         
        private static int SplitHolder;
        private static int minQuantity;
        private static int maxQuantity;
        public static int MaxQuantity { get => maxQuantity; set { maxQuantity = value; } }
        public static int MinQuantity { get => minQuantity; set { minQuantity = value; } }
        
        public static int CurrentSplits
        {
            get => SplitHolder;
            set
            {
                if (CurrentSplits <= SplitHolder && CurrentSplits > 0) SplitHolder = value;
                else throw new ArgumentException("current split can not be more than max split or negative number");
            }
        }

        // making sure delata will be more sensative than DIGIT_SENTITIVITY 
        public static double DeltaSentitivity { get => DIGIT_SENTITIVITY <= 0 ? 1 : Math.Pow(10, -DIGIT_SENTITIVITY); }



        static Configuration()
        {
            minQuantity = MIN_QUANTITY;
            maxQuantity = MAX_QUANTITY;
            SplitHolder = MaxSplits;

            

        }

        //make sure to get numbers after transction 
        public static void DeltaValidation(ref double @base, ref double height)
        {
            @base = Math.Round(@base, DIGIT_SENTITIVITY);
            height = Math.Round(height, DIGIT_SENTITIVITY);
        }

        public static bool ValidatePositiveNumbers(double @base, double height)
        {
            if (@base > 0 && height > 0) return true;
            else throw new ArgumentException("base or height must be positive numbers,action has failed!");
        }

        public static bool ValidatePositiveNumbers(double @base, double height, int quantity)
        {
            if (@base > 0 && height > 0 && quantity > 0) return true;
            else throw new ArgumentException("size and quantity must be positive numbers,action has failed!");
        }



    }
}

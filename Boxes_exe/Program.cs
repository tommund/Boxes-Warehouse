using BoxesAssignmentLogic;
using System;


namespace Boxes_exe
{
    class Program
    {
        static Manager manager = new Manager(new ConsoleUser(), new ConsoleDesigner());
        static void Main(string[] args)
        {
            manager.DesignInitizliazer(true, "my  boxes store");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("welcome to my store\n");
            Console.ForegroundColor = ConsoleColor.White;

            Menu();
        }


        static void Menu()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("please choose your action");
            Console.ForegroundColor = ConsoleColor.White;

            Console.WriteLine("1.update stock");
            Console.WriteLine("2.find box");
            Console.WriteLine("3.get matchig box for a present");
            Console.WriteLine("4.get matchig boxes for a presents");
            Console.WriteLine("5.clear application");
            Console.WriteLine("6.exit application");

            Console.WriteLine("");


            switch (Console.ReadLine())
            {
                case "1":
                    userAction(1);
                    TryUpdateStorage();
                    break;
                case "2":
                    userAction(2);
                    TryFindBox();
                    break;
                case "3":
                    userAction(3);
                    TryGetPresent();
                    break;
                case "4":
                    userAction(4);
                    TryGetPresents();
                    break;
                case "5":
                    userAction(5);
                    Console.Clear();
                    break;
                case "6":
                    userAction(6);
                    Environment.Exit(0);
                    break;


                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("please choose a valid option..\n");
                    Console.ForegroundColor = ConsoleColor.White;
                    Menu();
                    break;
            }
            Menu();
        }

        static void userAction(int num)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine($"you choose option number: {num}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        static void TryUpdateStorage()
        {
            try
            {
                double @base = GetDValue("base");
                double height = GetDValue("height");
                int quantity = GetIValue("quantity");
                manager.UpdateStock(@base, height, quantity);
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;

            }
            finally
            {
                Console.WriteLine("\n");
            }
        }
        static void TryGetPresent()
        {
            try
            {
                double @base = GetDValue("base");
                double height = GetDValue("height");
                manager.GetBestBoxForPresent(@base, height);
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Console.WriteLine("\n");
            }
        }

        static void TryGetPresents()
        {
            try
            {
                double @base = GetDValue("base");
                double height = GetDValue("height");
                int quantity = GetIValue("quantity");
                manager.GetBestBoxesForPresents(@base, height, quantity);
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Console.WriteLine("\n");
            }
        }

        static void TryFindBox()
        {
            try
            {
                double @base = GetDValue("base");
                double height = GetDValue("height");
                manager.FindBox(@base, height);
            }
            catch (ArgumentException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(e.Message);
                Console.ForegroundColor = ConsoleColor.White;
            }
            finally
            {
                Console.WriteLine("\n");
            }
        }
        static double GetDValue(string a)
        {
            Console.Write($"please enter requseted {a}:");
            string result = Console.ReadLine();
            if (double.TryParse(result, out double res)) return res;
            else throw new ArgumentException($"{result} is not a number!");
        }
        static int GetIValue(string a)
        {
            Console.Write($"please enter requseted {a}:");
            string result = Console.ReadLine();
            if (int.TryParse(result, out int res)) return res;
            else throw new ArgumentException($"{result} is not a number!");
        }
    }
}

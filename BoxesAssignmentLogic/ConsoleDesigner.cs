using BoxesAssignmentLogic.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic
{
    // implemention of the "Idesigner" interface, desiging the console application
    public class ConsoleDesigner : Idesigner
    {
        public void CursorStatus(bool isCursorVisible)
        {
            Console.CursorVisible = isCursorVisible;
        }
        public void ProgramTitle(string title)
        {
            Console.Title = title;
        }
        public void DesignInitizliazer(bool isCursorVisible, string title)
        {
            Console.CursorVisible = isCursorVisible;
            Console.Title = title;
        }




    }
}

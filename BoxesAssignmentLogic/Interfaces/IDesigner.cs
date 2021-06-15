using System;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic.Interfaces
{
    public interface Idesigner
    {
        void ProgramTitle(string title);
        void CursorStatus(bool isCursorVisible);
        void DesignInitizliazer(bool isCursorVisible, string title);
    }
}

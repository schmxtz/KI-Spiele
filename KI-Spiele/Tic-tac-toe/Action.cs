using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Tic_tac_toe
{
    class Action : IAction
    { 
        public Action(byte row, byte column) 
        {
            Move = (row, column);
        }

        public (byte Row, byte Column) Move { get; set; }
    }
}

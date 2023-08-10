using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Tic_tac_toe
{
    class Action : IAction
    {
        public Action() { }

        public Player Player { get; set; }

        /// <summary>
        /// Move as piece placement with (column, row) tuple 
        /// </summary>
        public (byte, byte) Move { get; set; }
    }
}

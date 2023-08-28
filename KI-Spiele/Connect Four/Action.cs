using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Connect_Four
{
    class Action : IAction
    {
        public Action(byte column)
        {
            Move = column;
        }

        public byte Move { get; set; }
    }
}

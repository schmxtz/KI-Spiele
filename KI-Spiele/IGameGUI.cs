using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI_Spiele
{
    /// <summary>
    /// Interface for a two-player board game GUI defining all necessary methods.
    /// </summary>
    interface IGameGUI
    {
        /// <summary>
        /// Initializes the board GUI on a given panel.
        /// </summary>
        void InitializeBoard();

        /// <summary>
        /// Updates the current board with its given action.
        /// </summary>
        /// <param name="action"></param>
        void UpdateBoard(IAction action);
    }
}

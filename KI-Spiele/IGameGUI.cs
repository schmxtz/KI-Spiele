using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

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
        void InitializeBoard(IGame game, MainWindow window);

        /// <summary>
        /// Updates the current board with its given action.
        /// </summary>
        /// <param name="action"></param>
        void UpdateBoard(IAction action);

        void ResetBoard();

        void BindUICallback();
        void UnbindUICallback();
    }
}

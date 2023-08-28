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
        /// Initializes the board GUI on a given panel (window).
        /// </summary>
        // TODO: Create an own interface for a MainWindow implementation and pass it as interface instead. Forces one to use the MainWindow-implementation, limiting the use of other GUIs.
        void InitializeBoard(IGame game, MainWindow window);

        /// <summary>
        /// Updates the current board with its given action.
        /// </summary>
        /// <param name="player"></param>
        /// <param name="action"></param>
        void UpdateBoard(Player player, IAction action);

        /// <summary>
        /// Resets the visual representation back to an empty GameBoard.
        /// </summary>
        void ResetBoard();

        /// <summary>
        /// Binds the function that's called on the MouseDown-Event.
        /// </summary>
        void BindUICallback();

        /// <summary>
        /// Unbinds the function that's called on the MouseDown-Event.
        /// </summary>
        void UnbindUICallback();
    }
}

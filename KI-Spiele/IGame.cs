using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KI_Spiele
{
    /// <summary>
    /// Interface for a two-player board game defining all necessary methods.
    /// </summary>
    interface IGame
    {
        /// <summary>
        /// Initializes the board and its necessary member variables.
        /// </summary>
        void InitializeBoard();

        /// <summary>
        /// Resets the game.
        /// </summary>
        void ResetGame();

        /// <summary>
        /// Makes a move with the given action.
        /// </summary>
        /// <param name="action"></param>
        void MakeMove(IAction action);

        /// <summary>
        /// Updates the current player index.
        /// </summary>
        void NextPlayer();
    }

    enum Player : byte
    {
        Zero = 0,
        One,
        Undefined
    }

    enum GameResult : byte
    {
        PlayerZero = 0,
        PlayerOne,
        Draw,
        NotFinished
    }
}

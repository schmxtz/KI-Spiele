using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
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
        double MakeMove(IAction action);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        List<IAction> GetMoves();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        BigInteger GetGameStateId();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        GameResult GetGameResult();
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

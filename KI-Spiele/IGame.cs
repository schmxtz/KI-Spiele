using KI_Spiele.AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace KI_Spiele
{
    /// <summary>
    /// Interface for a two-player board game defining all necessary methods.
    /// </summary>
    interface IGame
    {
        Player StartingPlayer { get; set; }
        QLearning QLearningAIZero { get; set; }
        QLearning QLearningAIOne { get; set; }

        /// <summary>
        /// Initializes the board and its necessary member variables.
        /// </summary>
        void InitializeBoard(MainWindow window);

        /// <summary>
        /// Resets the game.
        /// </summary>
        void ResetGame();

        /// <summary>
        /// Makes a move with the given action.
        /// </summary>
        /// <param name="action"></param>
        GameResult MakeMove(IAction action, bool updateGUI);

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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Player GetNextPlayer();
        void BindUICallback();
        void UnbindUICallback();
        IAction GetAction(byte row, byte column);
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

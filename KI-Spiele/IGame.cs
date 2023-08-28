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

        /// <summary>
        /// Initializes the visual representation of the board.
        /// </summary>
        void InitializeBoard(MainWindow window);

        /// <summary>
        /// Resets game to an empty GameBoard.
        /// <param name="updateGUI"></param>
        /// </summary>
        void ResetGame(bool updateGUI);

        /// <summary>
        /// <inheritdoc cref="IGameState.ExecuteAction(IAction)"/>   
        /// </summary>
        /// <param name="action"> Action defining the move to make. </param>
        /// <param name="updateGUI"> Flag, whether to update the GUI after making a move. </param>
        /// <returns> The GameResult after the action was performed. Is not necessarily an end-state. </returns>
        GameResult MakeMove(IAction action, bool updateGUI);

        /// <summary>
        /// <inheritdoc cref="IGameState.PossibleActions"/>       
        /// </summary>
        /// <returns> A list of valid moves that can be made in the current state. </returns>
        List<IAction> GetMoves();

        /// <summary>
        /// <inheritdoc cref="IGameState.Id"/>       
        /// </summary>
        /// <returns> The current state of the game formatted as a number. </returns>
        BigInteger GetGameStateId();

        /// <summary>
        /// <inheritdoc cref="IGameState.GetGameState"/>   
        /// </summary>
        /// <returns> The GameResult for the current on-going game. </returns>
        GameResult GetGameResult();

        /// <summary>
        /// <inheritdoc cref="IGameState.GetNextPlayer"/>   
        /// </summary>
        /// <returns> Player that is to make the next move. </returns>
        Player GetNextPlayer();

        /// <summary>
        /// <inheritdoc cref="IGameGUI.BindUICallback"/>  
        /// </summary>
        void BindUICallback();

        /// <summary>
        /// <inheritdoc cref="IGameGUI.UnbindUICallback"/>       
        /// </summary>
        void UnbindUICallback();

        /// <summary>
        /// <inheritdoc cref="IGameState.GetAction"/>       
        /// </summary>
        /// <param name="row"> Depending on the implementation can be left empty/passed in with a 0. </param>
        /// <param name="column"> Depending on the implementation can be left empty/passed in with a 0. </param>
        /// <returns> Re-use of the same object, when given the same row/column. </returns>
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

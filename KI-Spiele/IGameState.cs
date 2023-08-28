using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows.Media.Animation;

namespace KI_Spiele
{
    /// <summary>
    /// Interface for a two-player board game game-state defining all necessary methods.
    /// </summary>
    interface IGameState
    {
        /// <summary>
        /// The GameState is encoded as number where each tile on the GameBoard is encoded in 2 Bits.
        /// 2 Bits are needed for a 2-Player game because a tile can either be occupied by PlayerZero, 
        /// PlayerOne or Undefined.
        /// </summary>
        /// <returns> The current state of the game formatted as a number. </returns>
        BigInteger Id { get; }

        /// <summary>
        /// Needed for RL, so that a bot can only make valid moves. 
        /// </summary>
        /// <returns> A list of valid moves that can be made in the current state. </returns>
        List<IAction> PossibleActions { get; }

        /// <summary>
        /// Performs the given action and updates the GameBoard if it was a valid move. Does not return a reward for the action. That is decided inside the <see cref="AI.QLearning"/> class.
        /// </summary>
        /// <param name="action"></param>
        /// <returns> The GameResult after the action was performed. Is not necessarily an end-state. </returns>
        GameResult ExecuteAction(IAction action);

        /// <summary>
        /// Returns the current state of the game (Finished, etc.).
        /// </summary>
        /// <returns>
        /// The corresponding enum value.
        /// </returns>
        GameResult GetGameState();

        /// <summary>
        /// Resets current state of the GameBoard.
        /// </summary>
        /// <param name="startingPlayer"> The player that is to make the first move in the next game. </param>
        void ResetBoard(Player startingPlayer);

        /// <summary>
        /// 
        /// </summary>
        /// <returns> The player that is to do the next move. </returns>
        Player GetNextPlayer();

        /// <summary>
        /// Needed for RL as the moves are used as keys in ValueTable.
        /// </summary>
        /// <param name="row"> Depending on the implementation can be left empty/passed in with a 0. </param>
        /// <param name="column"> Depending on the implementation can be left empty/passed in with a 0. </param>
        /// <returns> Re-use of the same object, when given the same row/column. </returns>
        // TODO: Needs to be reevaluted whether it's a good idea to do it this way, as not all games have a row and column. See Tic-tac-toe vs. Connect Four (move is only characterized with a column).
        IAction GetAction(byte row, byte column);
    }
}

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
        BigInteger Id { get; }

        /// <summary>
        /// List of valid actions that can performed on the current game state.
        /// </summary>
        List<IAction> PossibleActions { get; }

        /// <summary>
        /// Performs the given action.
        /// </summary>
        /// <param name="action"></param>
        /// <returns>
        /// Returns a numerical reward.
        /// </returns>
        GameResult ExecuteAction(IAction action);

        /// <summary>
        /// Returns the current state of the game (Finished, etc.).
        /// </summary>
        /// <returns>
        /// Returns the corresponding enum value.
        /// </returns>
        GameResult GetGameState();

        /// <summary>
        /// Resets current game-state without creating a new object
        /// </summary>
        /// <param name="startingPlayer"></param>
        void ResetBoard(Player startingPlayer);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Player GetNextPlayer();

        IAction GetAction(byte row, byte column);
    }
}

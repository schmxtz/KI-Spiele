﻿using System;
using System.Collections.Generic;
using System.Linq;
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
        double ExecuteAction(IAction action);

        /// <summary>
        /// Returns the current state of the game (Finished, etc.).
        /// </summary>
        /// <returns>
        /// Returns the corresponding enum value.
        /// </returns>
        GameResult GetGameState();
    }
}

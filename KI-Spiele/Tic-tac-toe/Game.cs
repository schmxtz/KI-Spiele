using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace KI_Spiele.Tic_tac_toe
{
    class Game : IGame
    {
        #region --- Constructor ---
        #endregion

        #region --- Public Properties ---
        public byte CurrentPlayer { get; }
        // Reward for winning a game
        public double Reward { get; set; } 
        // Penalty for losing a game
        public double Penalty { get; set; }

        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameState Interface Implementation ----
        public void InitializeBoard()
        {
            throw new NotImplementedException();
        }

        public void MakeMove(IAction action)
        {
            throw new NotImplementedException();
        }

        public void NextPlayer()
        {
            throw new NotImplementedException();
        }

        public void ResetGame()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}

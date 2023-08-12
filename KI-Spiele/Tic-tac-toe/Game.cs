using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;

namespace KI_Spiele.Tic_tac_toe
{
    class Game : IGame
    {
        #region --- Constructor ---
        public Game(Player startingPlayer = Player.Undefined) 
        { 
            GameState = new GameState(startingPlayer);
            StartingPlayer = startingPlayer;
        }
        #endregion

        #region --- Public Properties ---
        Player StartingPlayer { get; set; }
        long PlayerZeroWins { get; set; } = 0;
        long PlayerOneWins { get; set; } = 0;
        long Draws { get; set; } = 0;
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameState Interface Implementation ----
        public void InitializeBoard()
        {
            throw new NotImplementedException();
        }

        public double MakeMove(IAction action)
        {
            return GameState.ExecuteAction(action);
        }

        public void ResetGame()
        {
            GameState.ResetBoard(StartingPlayer);
        }

        public List<IAction> GetMoves()
        {
            return GameState.PossibleActions;
        }

        public BigInteger GetGameStateId()
        {
            return GameState.Id;
        }

        public GameResult GetGameResult()
        {
            return GameState.GetGameState();
        }
        #endregion


        #region --- Private Members ---
        IGameState GameState;
        IGameGUI GameGUI;
        #endregion
    }
}

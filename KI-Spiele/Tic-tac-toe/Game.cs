using System.Collections.Generic;
using System.Numerics;
using System.Windows.Controls;

namespace KI_Spiele.Tic_tac_toe
{
    class Game : IGame
    {
        #region --- Constructor ---
        public Game(Player startingPlayer = Player.Undefined) 
        { 
            GameState = new GameState(startingPlayer);
            GameGUI = new GameGUI();
            StartingPlayer = startingPlayer;
        }
        #endregion

        #region --- Public Properties ---
        public Player StartingPlayer { get; set; }
        public long PlayerZeroWins { get; set; } = 0;
        public long PlayerOneWins { get; set; } = 0;
        public long Draws { get; set; } = 0;
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameState Interface Implementation ----
        public void InitializeBoard(MainWindow window)
        {
            GameGUI.InitializeBoard(this, window);
        }

        public double MakeMove(IAction action)
        {
            double result = GameState.ExecuteAction(action);
            GameGUI.UpdateBoard(action); 
            return result;
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

        public Player GetNextPlayer()
        {
            return GameState.GetNextPlayer();
        }
        #endregion


        #region --- Private Members ---
        IGameState GameState;
        IGameGUI GameGUI;
        #endregion
    }
}

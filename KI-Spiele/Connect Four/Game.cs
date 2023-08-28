using KI_Spiele.AI;
using KI_Spiele.Tic_tac_toe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Connect_Four
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
        public Player StartingPlayer { get; set; }

        #region --- IGameState Interface Implementation ----
        public void BindUICallback()
        {
            GameGUI.BindUICallback();
        }

        public IAction GetAction(byte row, byte column)
        {
           return GameState.GetAction(row, column);
        }

        public GameResult GetGameResult()
        {
            return GameState.GetGameState();
        }

        public BigInteger GetGameStateId()
        {
            return GameState.Id;
        }

        public List<IAction> GetMoves()
        {
            return GameState.PossibleActions;
        }

        public Player GetNextPlayer()
        {
            return GameState.GetNextPlayer();
        }

        public void InitializeBoard(MainWindow window)
        {
            GameGUI.InitializeBoard(this, window);
        }

        public GameResult MakeMove(IAction action, bool updateGUI)
        {
            Player nextPlayer = GameState.GetNextPlayer();
            GameResult result = GameState.ExecuteAction(action);
            if (updateGUI)
            {
                GameGUI.UpdateBoard(nextPlayer, action);
            }

            if (result != GameResult.NotFinished)
            {
                ResetGame();
            }

            return result;
        }

        public void ResetGame()
        {
            GameState.ResetBoard(StartingPlayer);
            GameGUI.ResetBoard();
        }

        public void UnbindUICallback()
        {
            GameGUI.UnbindUICallback();
        }
        #endregion

        #region --- Private Members ---
        IGameState GameState;
        IGameGUI GameGUI;
        #endregion
    }
}

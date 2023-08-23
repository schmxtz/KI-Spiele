using KI_Spiele.AI;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
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
        public QLearning QLearningAIZero { get; set; }
        public QLearning QLearningAIOne { get; set; }
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameState Interface Implementation ----
        public void InitializeBoard(MainWindow window)
        {
            GameGUI.InitializeBoard(this, window);
        }

        public GameResult MakeMove(IAction action, bool updateGUI = true)
        {
            GameResult result = GameState.ExecuteAction(action);
            if (updateGUI)
            {
                GameGUI.UpdateBoard(action);
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

        public void BindUICallback()
        {
            GameGUI.BindUICallback();
        }

        public void UnbindUICallback()
        {
            GameGUI.UnbindUICallback();
        }

        public IAction GetAction(byte row, byte column)
        {
            return GameState.GetAction(row, column);
        }
        #endregion

        #region --- Private Helper Functions ---
        #endregion

        #region --- Private Members ---
        IGameState GameState;
        IGameGUI GameGUI;
        #endregion
    }
}

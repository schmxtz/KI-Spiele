using System.Collections.Generic;
using System.Numerics;

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
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGame Interface Implementation ---
        /// <summary>
        /// Implements <see cref="IGame.InitializeBoard(MainWindow)"/>
        /// </summary>
        /// <param name="window"></param>
        public void InitializeBoard(MainWindow window)
        {
            GameGUI.InitializeBoard(this, window);
        }

        /// <summary>
        /// Implements <see cref="IGame.MakeMove(IAction, bool)"/>
        /// </summary>
        /// <param name="action"></param>
        /// <param name="updateGUI"></param>
        /// <returns></returns>
        public GameResult MakeMove(IAction action, bool updateGUI = true)
        {
            Player nextPlayer = GameState.GetNextPlayer();
            // Update the GameState
            GameResult result = GameState.ExecuteAction(action);
            if (updateGUI)
            {
                // Update the GameGUI if needed, e.g. this is disabled during training,
                // to speed up performance
                GameGUI.UpdateBoard(nextPlayer, action);
            }            

            // Reset Game if it's finished
            if (result != GameResult.NotFinished)
            {
                ResetGame(updateGUI);
            }

            return result;
        }

        /// <summary>
        /// Implements <see cref="IGame.ResetGame"/>
        /// <param name="updateGUI"></param>
        /// </summary>
        public void ResetGame(bool updateGUI = true)
        {
            GameState.ResetBoard(StartingPlayer);
            if (updateGUI)
            {
                GameGUI.ResetBoard();
            }
        }

        /// <summary>
        /// Implements <see cref="IGame.GetMoves"/>
        /// </summary>
        public List<IAction> GetMoves()
        {
            return GameState.PossibleActions;
        }

        /// <summary>
        /// Implements <see cref="IGame.GetGameStateId"/>
        /// </summary>
        public BigInteger GetGameStateId()
        {
            return GameState.Id;
        }

        /// <summary>
        /// Implements <see cref="IGame.GetGameResult"/>
        /// </summary>
        public GameResult GetGameResult()
        {
            return GameState.GetGameState();
        }

        /// <summary>
        /// Implements <see cref="IGame.GetNextPlayer"/>
        /// </summary>
        public Player GetNextPlayer()
        {
            return GameState.GetNextPlayer();
        }

        /// <summary>
        /// Implements <see cref="IGame.BindUICallback"/>
        /// </summary>
        public void BindUICallback()
        {
            GameGUI.BindUICallback();
        }

        /// <summary>
        /// Implements <see cref="IGame.UnbindUICallback"/>
        /// </summary>
        public void UnbindUICallback()
        {
            GameGUI.UnbindUICallback();
        }

        /// <summary>
        /// Implements <see cref="IGame.GetAction"/>
        /// </summary>
        public IAction GetAction(byte row, byte column)
        {
            return GameState.GetAction(row, column);
        }
        #endregion

        #region --- Private Helper Functions ---
        #endregion

        #region --- Private Members ---
        private IGameState GameState;
        private IGameGUI GameGUI;
        #endregion
    }
}

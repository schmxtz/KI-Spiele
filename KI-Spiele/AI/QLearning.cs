using AI.Util;
using KI_Spiele.Tic_tac_toe;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.AI
{
    class QLearning
    {
        // Credit: KI-Spiele QLearning2 Handout - Prof. Dr.-Ing. Christof Rezk-Salama
        #region --- Constructor ---
        public QLearning(Player player)
        {
            ExplorationRate = 0.8f;
            DiscountRate = 0.9f;
            LearningRate = 0.3f;
            LearnedTable = new QTable();
            Reward = 1.0;
            Penalty = -1.0;
            Player = player;
            // MoveHistory to improve learning rate and convergance
            MoveHistory = new List<(BigInteger, IAction, List<IAction>)>();
            DefensiveLearning = false;
        }
        #endregion 

        #region  --- Public Properties ---
        public IGame Game { get; set; }
        public double ExplorationRate { get; set; }
        public double DiscountRate { get; set; }
        public double LearningRate { get; set; }
        public double Reward { get; set; }
        public double Penalty { get; set; }
        public QLearning OtherAI { get; set; }
        public Player Player { get; set; }
        public bool DefensiveLearning { get; set; }
        #endregion

        #region --- Public Member Functions ---
        /// <summary>
        /// Calculates the number of entries inside the LearnedTable for all states : + moves in given state
        /// </summary>
        /// <returns></returns>
        public long GetTableSize()
        {
            return LearnedTable.GetTableSize();
        }

        public void ResetLearnedTable()
        {
            LearnedTable = new QTable();
        }

        public GameResult MakeMove(bool updateQTable = true, bool updateGUI = false)
        {
            // Get Id of current GameState
            BigInteger currentState = Game.GetGameStateId();
            IAction a;

            // Only update if flag is set to true (during training), when evaluating the bots
            // performance you typically don't wanna update the LearnedTable unless you want
            // Online-Learning
            if (updateQTable)
            {
                // Select an action based on the current state, chooses randomly (ExplorationRate) the best move or a random move
                a = SelectAction(Game, currentState);
                MoveHistory.Add((currentState, a, Game.GetMoves()));
                GameResult result = Game.MakeMove(a, updateGUI);                

                // If move finished the game propagate the reward/penalty throughout the MoveHistory with the QLearning rule
                if (result != GameResult.NotFinished)
                {
                    //var (r1, r2) = (Reward, OtherAI.Reward);
                    var (r1, r2) = (0.5, 0.5);

                    // Give out reward if one of the parties won
                    if (result == GameResult.PlayerOne || result == GameResult.PlayerZero) { (r1, r2) = (Reward, OtherAI.Penalty); };

                    // Update the LearnedTable for both bots. This is needed so that the other bot learns to not lose. If only the one who made the last move is
                    // updated, they performed way worse and made questionable moves. Updating both fixed this issue.
                    UpdateTable(r1);
                    OtherAI.UpdateTable(r2);
                }
                return result;
            }

            // Perform the best move possible for the given state. If the state is not contained inside the LearnedTable,
            // a random action from the list of possible moves is taken.
            // TODO: This leaves room for improvement, instead choosing a move randomly one can either think of a strategy for the specific game or train a third LearnedTable where all games start with this given state, learning which move is best in the current scenario
            a = LearnedTable.BestActionNoLearning(currentState, Game);
            return Game.MakeMove(a, updateGUI);
        }


        #endregion

        #region --- Private Helper Functions ----
        private IAction SelectAction(IGame game, BigInteger stateId)
        {

            if (Rand.NextDouble() < ExplorationRate)
            {
                return RandomList<IAction>.RandomEntry(game.GetMoves());
            }
            else
            {
                return LearnedTable.BestAction(stateId, game.GetMoves());
            }
        }

        private double QLearningFormula(double oldQuality, double followStateQuality, double reward) 
        {
            double newQuality = (1.0 - LearningRate) * oldQuality +
                (LearningRate) * (reward + DiscountRate * followStateQuality);

            return newQuality;
        }

        private void UpdateTable(double reward)
        {
            // Reverse the move history in order to propagate the reward/QLearning formula from end to start
            MoveHistory.Reverse();

            // Set last move to winning/losing move
            if (!DefensiveLearning)
            {
                LearnedTable.SetValue(MoveHistory[0].Item1, MoveHistory[0].Item2, reward);
            }
            else
            {
                // Avoid at all cost to give reward if AI lost with the move before
                double oldValue = LearnedTable.ValueFor(MoveHistory[0].Item1, MoveHistory[0].Item2);
                if (oldValue != 0.0 && oldValue >= reward || oldValue == 0.0)
                {
                    LearnedTable.SetValue(MoveHistory[0].Item1, MoveHistory[0].Item2, reward);
                }
            }
            
            double followStateQuality = LearnedTable.ValueFor(MoveHistory[0].Item1, MoveHistory[0].Item3);

            for (int i = 1; i < MoveHistory.Count; i++)
            {
                double oldQuality = LearnedTable.ValueFor(MoveHistory[i].Item1, MoveHistory[i].Item2);
                double newQuality = QLearningFormula(oldQuality, followStateQuality, reward);
                followStateQuality = LearnedTable.ValueFor(MoveHistory[i].Item1, MoveHistory[i].Item3);

                if (newQuality != oldQuality)
                {
                    LearnedTable.SetValue(MoveHistory[i].Item1, MoveHistory[i].Item2, newQuality);
                }                
            }

            MoveHistory.Clear();
        }
        #endregion

        #region --- Private Member Variables ---
        private QTable LearnedTable;
        private Random Rand = new Random();
        private List<(BigInteger, IAction, List<IAction>)> MoveHistory;
        #endregion
    }
}

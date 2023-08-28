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
            MoveHistory = new List<(BigInteger, IAction, List<IAction>)>();
            DefensiveLearning = true;
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
        public long GetTableSize()
        {
            return LearnedTable.GetTableSize();
        }

        public GameResult MakeMove(bool updateQTable = true)
        {
            BigInteger currentState = Game.GetGameStateId();
            IAction a;

            if (updateQTable)
            {
                a = SelectAction(Game, currentState);
                MoveHistory.Add((currentState, a, Game.GetMoves()));
                GameResult result = Game.MakeMove(a, false);                

                if (result != GameResult.NotFinished)
                {
                    var (r1, r2) = (1.0, 1.0);
                    if (result == GameResult.PlayerOne || result == GameResult.PlayerZero) { (r1, r2) = (1.0, -1.0); };
                    UpdateTable(r1);
                    OtherAI.UpdateTable(r2);
                }
                return result;
            }

            a = LearnedTable.BestActionNoLearning(currentState, Game);
            return Game.MakeMove(a, true);
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

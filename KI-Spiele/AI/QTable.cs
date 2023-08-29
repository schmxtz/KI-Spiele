using AI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.AI
{
    // Credit: KI-Spiele QLearning2 Handout - Prof. Dr.-Ing. Christof Rezk-Salama
    class QTable
    {
        #region --- Constructor ---
        public QTable()
        {
            // Lazy initialization: At startup, the dictionary is empty, which means
            // that whenever we find a state or an action not contained in the dictionary
            // we set it's value to neutral (0.0) in the access functions below
            ValueTable = new Dictionary<BigInteger, Dictionary<IAction, double>>();
        }
        #endregion

        #region --- Access functions ---
        public double ValueFor(BigInteger stateId, IAction a)
        {
            // Lazy Initialization
            if (!ValueTable.ContainsKey(stateId))
            {
                // We have not visited this state before
                ValueTable[stateId] = new Dictionary<IAction, double>();
            }

            if (!ValueTable[stateId].ContainsKey(a))
            {
                // We have never performed this action in the state before
                ValueTable[stateId][a] = 0.0;
            }

            return ValueTable[stateId][a];
        }

        public double ValueFor(BigInteger stateId, List<IAction> availableActions)
        {
            if (ValueTable[stateId].Count > availableActions.Count)
            {
                Console.WriteLine(ValueTable[stateId].Count + " " + availableActions.Count);
            }

            // Lazy Initialization
            if (!ValueTable.ContainsKey(stateId))
            {
                // We have not visited this state before
                ValueTable[stateId] = new Dictionary<IAction, double>();
                // set all available actions to neutral (0.0) in the table
                foreach (var a in availableActions)
                {
                    ValueTable[stateId][a] = 0.0;
                }
                return 0.0;
            }


            // the quality of a state is defined as the quality of 
            // the best action available in this state
            double MaxValue = Double.MinValue;
            foreach (var a in availableActions)
            {
                if (!ValueTable[stateId].ContainsKey(a))
                {
                    ValueTable[stateId][a] = 0.0;
                }

                double value = ValueTable[stateId][a];
                if (MaxValue < value)
                {
                    MaxValue = value;
                }

            }
            return MaxValue;
        }

        public IAction BestAction(BigInteger stateId, List<IAction> availableActions)
        {
            // Lazy initialization
            if (!ValueTable.ContainsKey(stateId))
            {
                // we have not visited this state before
                ValueTable[stateId] = new Dictionary<IAction, double>();
                // set all available actions to neutral (0.0) in the table
                foreach (var a in availableActions)
                {
                    ValueTable[stateId][a] = 0.0;
                }
                return RandomList<IAction>.RandomEntry(availableActions);
            }

            // It is very important here to not only find one of the 
            // best action! If we have several actions which are have
            // the same quality, we need to chose one randomly! If we
            // don't do this, we will end up in only very few states.

            List<IAction> bestActions = new List<IAction>();

            double maxValue = double.MinValue;
            foreach (var a in availableActions)
            {
                if (!ValueTable[stateId].ContainsKey(a))
                {
                    ValueTable[stateId][a] = 0.0;
                }

                double value = ValueTable[stateId][a];
                if (maxValue < value)
                {
                    maxValue = value;
                    bestActions.Clear();
                    bestActions.Add(a);
                }
                if (maxValue == value)
                {
                    bestActions.Add(a);
                }
            }

            // this is a generic function, which choses a random action from
            // the given list (see Util.cs for the definition)
            return RandomList<IAction>.RandomEntry(bestActions);

        }

        public long GetTableSize()
        {
            return ValueTable.Sum(state => state.Value.Count);
        }

        public IAction BestActionNoLearning(BigInteger stateId, IGame game)
        {
            if (ValueTable.ContainsKey(stateId))
            {
                List<IAction> bestActions = new List<IAction>();

                double maxValue = double.MinValue;
                var actions = ValueTable[stateId];

                foreach (var a in actions)
                {
                    if (maxValue < a.Value)
                    {
                        maxValue = a.Value;
                        bestActions.Clear();
                        bestActions.Add(a.Key);
                    }
                    else if (maxValue == a.Value)
                    {
                        bestActions.Add(a.Key);
                    }
                }
                return RandomList<IAction>.RandomEntry(bestActions);
            }
            return RandomList<IAction>.RandomEntry(game.GetMoves());
        }
        #endregion

        #region --- Private helper functions ---

        #endregion

        #region --- Setter functions ----
        public void SetValue(BigInteger stateId, IAction a, double newValue)
        {
            if (!ValueTable.ContainsKey(stateId))
            {
                // We have not visited this state before
                ValueTable[stateId] = new Dictionary<IAction, double>();
            }

            // store the value in the table
            ValueTable[stateId][a] = newValue;
        }
        #endregion

        #region --- Private Members ---
        // this is the big lookup table, which stores a quality value (double)
        // for each action (IAction) in each state (IGameState.Id)
        private Dictionary<BigInteger, Dictionary<IAction, double>> ValueTable;
        #endregion
    }
}

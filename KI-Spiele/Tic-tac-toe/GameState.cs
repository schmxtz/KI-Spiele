using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Tic_tac_toe
{
    class GameState : IGameState
    {
        #region --- Constructor ---
        /// <summary>
        /// Instantiates the game board and its corresponding variables. Also can be used to control
        /// which player makes the first move.
        /// </summary>
        /// <param name="startingPlayer"> Controls which player starts first. If set
        /// to Undefined, it's chosen randomly.</param>
        public GameState(Player startingPlayer) 
        {
            ResetBoard(startingPlayer);
            InitActions();
        }
        #endregion

        #region --- Public Properties ---
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameState Interface Implementation ---
        /// <summary>
        /// Implements <see cref="IGameState.Id"/>
        /// </summary>
        public BigInteger Id
        {
            get
            {
                // Leading one, so that leading zeros are not truncated
                BigInteger result = 1;
                for (int i = 0; i < GameBoard.Length; i++)
                {
                    for (int j = 0; j < GameBoard.Length; j++)
                    {
                        switch (GameBoard[i][j])
                        {
                            // Allocate next two bits and add the bit-value of the tile
                            case Player.Zero:
                                result <<= 2;
                                result += (byte)Player.Zero;
                                break;
                            case Player.One:
                                result <<= 2;
                                result += (byte)Player.One;
                                break;
                            case Player.Undefined:
                                result <<= 2;
                                result += (byte)Player.Undefined;
                                break;
                        }
                    }
                }
                return result;
            }
        }

        /// <summary>
        /// Implements <see cref="IGameState.PossibleActions"/>
        /// </summary>
        public List<IAction> PossibleActions
        {
            get
            {
                List<IAction> actions = new List<IAction>();
                for (byte i = 0; i < GameBoard.Length; i++)
                {
                    for (byte j = 0; j < GameBoard[i].Length; j++)
                    { 
                        // If tile is empty, valid move can be made
                        if (GameBoard[i][j] == Player.Undefined)
                        {
                            actions.Add(AllActions[i][j]);
                        }
                    }
                }
                return actions;
            } 
        }

        /// <summary>
        /// Implements <see cref="IGameState.ExecuteAction(IAction)"/>
        /// </summary>
        /// <param name="a"> Action that is to be performed. </param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"> Thrown if player tries to make an invalid move. </exception>
        public GameResult ExecuteAction(IAction a)
        {
            Action action = (Action)a;
            // Check if tile is already in use.
            if (GameBoard[action.Move.Item1][action.Move.Item2] != Player.Undefined) 
            {
                throw new ArgumentException("Not a valid action! Tile is already in use!");
            }
            // Check if player wants to place inside a valid tile.
            if (action.Move.Item1 < 0 ||  action.Move.Item1 > 2 || action.Move.Item2 < 0 || action.Move.Item2 > 2)
            {
                throw new ArgumentException("Not a valid action!");
            }
            // Check if game is finished
            if (GameResult != GameResult.NotFinished)
            {
                throw new ArgumentException("Game is already finished!");
            }

            // Update the GameBoard to the current action.
            GameBoard[action.Move.Row][action.Move.Column] = NextPlayer;

            // Update move number
            MoveNumber++;

            // Update next player
            NextPlayer = (Player)((1 + (byte)NextPlayer) % 2);

            // Check for GameResult and return it
            GameResult = CheckForWinner();

            return GameResult;
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetGameState"/>
        /// </summary>
        public GameResult GetGameState()
        {
            return GameResult;
        }

        /// <summary>
        /// Implements <see cref="IGameState.ResetBoard(Player)"/>
        /// </summary>
        public void ResetBoard(Player startingPlayer)
        {
            // Reset GameBoard and corresponding member variables
            GameBoard = new Player[][]
            {
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
            };
            GameResult = GameResult.NotFinished;
            MoveNumber = 0;

            NextPlayer = startingPlayer;
            // If startingPlayer is not to a specific player, choose one randomly.
            if (NextPlayer == Player.Undefined)
            {                
                NextPlayer = (Player)random.Next(0, 2);
            }
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetNextPlayer"/>
        /// </summary>
        /// <returns> Player that is to make the next move. </returns>
        public Player GetNextPlayer()
        {
            return NextPlayer;
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetAction(byte, byte)"/>
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns> Re-used action for given row and column stored inside AllActions. </returns>
        public IAction GetAction(byte row, byte column)
        {
            return AllActions[row][column];
        }
        #endregion

        #region --- Private Init Functions ---
        /// <summary>
        /// Initializes all valid actions and stores them inside a 2D-Array.
        /// </summary>
        private void InitActions()
        {
            AllActions = new IAction[][]
            {
                new IAction[] { new Action(0, 0), new Action(0, 1), new Action(0, 2) },
                new IAction[] { new Action(1, 0), new Action(1, 1), new Action(1, 2) },
                new IAction[] { new Action(2, 0), new Action(2, 1), new Action(2, 2) },
            };
        }
        #endregion

        #region --- Private Helper Functions ---
        private GameResult CheckForWinner()
        {
            // Check for all possible ways a game can end.
            GameResult columnResult = CheckColumn();
            if (columnResult != GameResult.NotFinished)
            {
                return columnResult;
            }

            GameResult rowResult = CheckRow();
            if (rowResult != GameResult.NotFinished)
            {
                return rowResult;
            }

            GameResult diagonalResult = CheckDiagonal();
            if (diagonalResult != GameResult.NotFinished)
            {
                return diagonalResult;
            }

            // If no winner can be determined and all possible moves have been made, it's a draw.
            if (MoveNumber == 9)
            {
                return GameResult.Draw;
            }

            return GameResult.NotFinished;
        }
        private GameResult CheckColumn()
        {
            GameResult result = GameResult.NotFinished;
            for (byte i = 0; i < GameBoard.Length; i++)
            {
                if (GameBoard[0][i] == GameBoard[1][i] && GameBoard[1][i] == GameBoard[2][i] && GameBoard[0][i] != Player.Undefined)
                {
                    // Is castable since it can't be Player.Undefined and GameResult.PlayerZero/GameResult.PlayerOne have the same index as
                    // Player.Zero/Player.One
                    return (GameResult)GameBoard[0][i];
                }
            }
            return result;  
        }
        private GameResult CheckRow()
        {
            GameResult result = GameResult.NotFinished;
            for (byte i = 0; i < GameBoard.Length; i++)
            {
                if (GameBoard[i][0] == GameBoard[i][1] && GameBoard[i][1] == GameBoard[i][2] && GameBoard[i][0] != Player.Undefined)
                {
                    // Is castable since it can't be Player.Undefined and GameResult.PlayerZero/GameResult.PlayerOne have the same index as
                    // Player.Zero/Player.One
                    return (GameResult)GameBoard[i][0];
                }
            }
            return result;
        }
        private GameResult CheckDiagonal()
        {
            GameResult result = GameResult.NotFinished;
            // Top left to bottom right diagonal
            if (GameBoard[0][0] == GameBoard[1][1] && GameBoard[1][1] == GameBoard[2][2] && GameBoard[1][1] != Player.Undefined)
            {
                return (GameResult)GameBoard[1][1];
            }
            // Bottom left to top right diagonal
            if (GameBoard[2][0] == GameBoard[1][1] && GameBoard[1][1] == GameBoard[0][2] && GameBoard[1][1] != Player.Undefined)
            {
                return (GameResult)GameBoard[1][1];
            }
            return result;
        }
        #endregion

        #region --- Private Members ---
        // GameBoard is 3x3 2D-array with first index specifying the row and second index specifying the column
        private Player[][] GameBoard;
        private byte MoveNumber;
        private GameResult GameResult;
        // Player who made the last move
        private Player NextPlayer;
        private IAction[][] AllActions;
        private Random random = new Random();
        #endregion
    }
}

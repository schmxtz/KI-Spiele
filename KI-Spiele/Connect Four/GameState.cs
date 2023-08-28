using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Connect_Four
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

        #region --- IGameState Interface Implementation ----
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
                        // Allocate next two bits and add the bit-value of the tile
                        switch (GameBoard[i][j])
                        {
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
                for (int i = 0; i < GameBoard[0].Length; i++)
                {
                    // If the tile in the top row is empty, valid move can be made inside column
                    if (GameBoard[0][i] == Player.Undefined)
                    {
                        actions.Add(AllActions[i]);
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
            // Check if column is already full
            if (GameBoard[0][action.Move] != Player.Undefined)
            {
                throw new ArgumentException("Not a valid action! Column is already full!");
            }
            // Check if player wants to place inside a valid tile.
            if (action.Move < 0 || action.Move > 6)
            {
                throw new ArgumentException("Not a valid action!");
            }
            // Check if game is finished
            if (GameResult != GameResult.NotFinished)
            {
                throw new ArgumentException("Game is already finished!");
            }

            // Save in which row the tile was placed
            int row = 0;

            // Update the GameBoard to the current action.
            // Need to loop through rows from bottom to top
            for (int i = GameBoard.Length - 1; i >= 0; i--)
            {
                if (GameBoard[i][action.Move] == Player.Undefined)
                {
                    GameBoard[i][action.Move] = NextPlayer;
                    row = i;
                    break;
                }
            }

            // Update move number
            MoveNumber++;

            // Update next player
            NextPlayer = (Player)((1 + (byte)NextPlayer) % 2);

            // Check for GameResult and return it
            GameResult = CheckForWinner(row, action.Move);

            return GameResult;
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetAction(byte, byte)"/>
        /// </summary>
        /// <param name="row"> Is not used here because an action is only characterized by column. </param>
        /// <param name="column"></param>
        /// <returns> Re-used action for given row and column stored inside AllActions. </returns>
        public IAction GetAction(byte row, byte column)
        {
            return AllActions[column];
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetGameState"/>
        /// </summary>
        public GameResult GetGameState()
        {
            return GameResult;
        }

        /// <summary>
        /// Implements <see cref="IGameState.GetNextPlayer"/>
        /// </summary>
        /// <returns> Player that is to make the next move. </returns>
        public Player GetNextPlayer()
        {
            return NextPlayer;
        }

        public void ResetBoard(Player startingPlayer)
        {
            // Reset GameBoard and corresponding member variables
            GameBoard = new Player[][]
            {
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined, Player.Undefined },
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
        #endregion

        #region --- Private Init Functions ---
        /// <summary>
        /// Initializes all valid actions and stores them inside a 1D-Array.
        /// </summary>
        private void InitActions()
        {
            AllActions = new IAction[]
            {
                new Action(0), new Action(1), new Action(2), new Action(3),
                new Action(4), new Action(5), new Action(6),
            };
        }
        #endregion

        #region --- Private Helper Functions ---
        private GameResult CheckForWinner(int row, byte column)
        {
            // Check for all possible ways a game can end.
            GameResult rowResult = CheckRow(row, column);
            if (rowResult != GameResult.NotFinished)
            {
                return rowResult;
            }

            GameResult columnResult = CheckColumn(row, column);
            if (columnResult != GameResult.NotFinished)
            {
                return columnResult;
            }

            GameResult diagonalResult = CheckDiagonal(row, column);
            if (diagonalResult != GameResult.NotFinished)
            {
                return diagonalResult;
            }

            // If no winner can be determined and all possible moves have been made, it's a draw.
            if (MoveNumber == 42)
            {
                return GameResult.Draw;
            }

            return GameResult.NotFinished;
        }

        private GameResult CheckRow(int row, byte column)
        {
            for (int i = 0; i < GameBoard[0].Length - 3; i++)
            {
                if (GameBoard[row][i] == GameBoard[row][i + 1] &&
                    GameBoard[row][i + 1] == GameBoard[row][i + 2] &&
                    GameBoard[row][i + 2] == GameBoard[row][i + 3] &&
                    GameBoard[row][i] != Player.Undefined)
                {
                    // Is castable since it can't be Player.Undefined and GameResult.PlayerZero/GameResult.PlayerOne have the same index as
                    // Player.Zero/Player.One
                    return (GameResult)GameBoard[row][i];
                }
            }
            return GameResult.NotFinished;
        }

        private GameResult CheckColumn(int row, byte column)
        {
            for (int i = 0; i < GameBoard.Length - 3; i++)
            {
                if (GameBoard[i][column] == GameBoard[i + 1][column] &&
                    GameBoard[i + 1][column] == GameBoard[i + 2][column] &&
                    GameBoard[i + 2][column] == GameBoard[i + 3][column] &&
                    GameBoard[i][column] != Player.Undefined)
                {
                    // Is castable since it can't be Player.Undefined and GameResult.PlayerZero/GameResult.PlayerOne have the same index as
                    // Player.Zero/Player.One
                    return (GameResult)GameBoard[i][column];
                }
            }
            return GameResult.NotFinished;
        }

        private GameResult CheckDiagonal(int row, byte column)
        {
            int distance = Math.Min(column, row);
            int startColumn = column - distance;
            int startRow = row - distance;

            // Check for top left to bottom right
            for (int i = startRow, j = startColumn ; i < GameBoard.Length - 3 && j < GameBoard[0].Length - 3; i++, j++)
            {
                if (GameBoard[i][j] == GameBoard[i + 1][j + 1] &&
                    GameBoard[i + 1][j + 1] == GameBoard[i + 2][j + 2] &&
                    GameBoard[i + 2][j + 2] == GameBoard[i + 3][j + 3] &&
                    GameBoard[i][j] != Player.Undefined) 
                {
                    return (GameResult)GameBoard[i][j];
                }
            }

            distance = Math.Min(GameBoard.Length - 1 - row, column);
            startColumn = column - distance;
            startRow = row + distance;

            // Check from bottom left to top right
            for (int i = startRow, j = startColumn; i - 3 >= 0 && j < GameBoard[0].Length - 3; i--, j++)
            {
                if (GameBoard[i][j] == GameBoard[i - 1][j + 1] &&
                    GameBoard[i - 1][j + 1] == GameBoard[i - 2][j + 2] &&
                    GameBoard[i - 2][j + 2] == GameBoard[i - 3][j + 3] &&
                    GameBoard[i][j] != Player.Undefined)
                {
                    return (GameResult)GameBoard[i][j];
                }
            }
            return GameResult.NotFinished;
        }
        #endregion

        #region --- Private Members ---
        // GameBoard is 6x7 2D-array with first index specifying the row and second index specifying the column
        // Low index specifies top rows, high index lower rows
        private Player[][] GameBoard;
        private byte MoveNumber;
        private GameResult GameResult;
        // Player who made the last move
        private Player NextPlayer;
        private IAction[] AllActions;
        private Random random = new Random();
        #endregion
    }
}

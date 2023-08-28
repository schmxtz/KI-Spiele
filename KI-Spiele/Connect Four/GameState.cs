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
        IGame Game { get; set; }
        #endregion

        #region --- IGameState Interface Implementation ----
        public BigInteger Id
        {
            get
            {
                BigInteger result = 1;
                for (int i = 0; i < GameBoard.Length; i++)
                {
                    for (int j = 0; j < GameBoard.Length; j++)
                    {
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

        public List<IAction> PossibleActions
        {
            get
            {
                List<IAction> actions = new List<IAction>();
                for (int i = 0; i < GameBoard[0].Length; i++)
                {
                    if (GameBoard[0][i] == Player.Undefined)
                    {
                        actions.Add(AllActions[i]);
                    }
                }
                return actions;
            }
        }

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
            int row = 0;
            // Update the GameBoard to the current action.
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

            NextPlayer = (Player)((1 + (byte)NextPlayer) % 2);

            GameResult = CheckForWinner(row, action.Move);

            return GameResult;
        }

        public IAction GetAction(byte row, byte column)
        {
            return AllActions[column];
        }

        public GameResult GetGameState()
        {
            return GameResult;
        }

        public Player GetNextPlayer()
        {
            return NextPlayer;
        }

        public void ResetBoard(Player startingPlayer)
        {
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
            if (NextPlayer == Player.Undefined)
            {
                NextPlayer = (Player)random.Next(0, 2);
            }
        }
        #endregion

        #region --- Private Init Functions ---
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

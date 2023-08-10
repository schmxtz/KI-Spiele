﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KI_Spiele.Tic_tac_toe
{
    class GameState : IGameState
    {
        #region --- Constructor ---
        public GameState() 
        {
            GameBoard = new Player[][] 
            { 
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
                new Player[]{ Player.Undefined, Player.Undefined, Player.Undefined },
            };
            GameResult = GameResult.NotFinished;
            MoveNumber = 0;
        }
        #endregion

        #region --- Public Properties ---
        #endregion
        public List<IAction> PossibleActions
        {
            get
            {
                List<IAction> actions = new List<IAction>();
                for (byte i = 0; i < GameBoard.Length; i++)
                {
                    for (byte j = 0; j < GameBoard[i].Length; j++)
                    { 
                        if (GameBoard[i][j] == Player.Undefined)
                        {
                            actions.Add(new Action() 
                            {
                                Player = Player.Undefined,
                                Move = (i, j)
                            });
                        }
                    }
                }
                return actions;
            } 
        }

        public double ExecuteAction(IAction a)
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
            GameBoard[action.Move.Item1][action.Move.Item1] = action.Player;
            MoveNumber++;

            GameResult = CheckForWinner();
        }

        public GameResult GetGameState()
        {
            return GameResult;
        }

        #region --- Private Helper Functions ---
        private GameResult CheckForWinner()
        {
            GameResult columnResult = CheckColumn();
            GameResult rowResult = CheckRow();
            GameResult diagonalResult = CheckDiagonal();

            if (columnResult != GameResult.NotFinished)
            {
                return columnResult;
            }

            if (rowResult != GameResult.NotFinished)
            {
                return rowResult;
            }

            if (diagonalResult != GameResult.NotFinished)
            {
                return diagonalResult;
            }

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
                if (GameBoard[i][0] == GameBoard[i][1] && GameBoard[i][1] == GameBoard[i][2] && GameBoard[0][i] != Player.Undefined)
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
        private Player[][] GameBoard;
        private byte MoveNumber;
        private GameResult GameResult;
        #endregion
    }
}
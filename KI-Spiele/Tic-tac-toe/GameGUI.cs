using KI_Spiele.Connect_Four;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KI_Spiele.Tic_tac_toe
{
    class GameGUI : IGameGUI
    {
        #region --- Public Properties ---
        public double CellSize { get; set; } = 300;
        public double CellInnerPad { get; set; } = 10;
        #endregion

        #region --- IGameGUI Interface Implementation ---
        /// <summary>
        /// Implements <see cref="IGameGUI.InitializeBoard(IGame, MainWindow)"/>
        /// </summary>
        /// <param name="game"></param>
        /// <param name="window"></param>
        public void InitializeBoard(IGame game, MainWindow window)
        {
            MainWindow = window;
            Game = game;

            // Calculate GameBoard GUI based on CellSize
            window.GameGrid.Width = 3 * CellSize;
            window.GameGrid.Height = 3 * CellSize;

            // Set background
            window.GameGrid.Background = new ImageBrush((ImageSource)window.FindResource("TicBoard"));

            // Initialize the 3x3 grid in which tiles are placed
            for (int i = 0; i < 3; i++)
            {
                var columnDefinition = new ColumnDefinition();
                var rowDefinition = new RowDefinition();
                window.GameGrid.ColumnDefinitions.Add(columnDefinition);
                window.GameGrid.RowDefinitions.Add(rowDefinition);
            }

            // Update GUI, which player is to make the next move
            Player startingPlayer = game.GetNextPlayer();
            window.NextPlayer.Text = startingPlayer.ToString();

            // Set the image previews for PlayerZero and PlayerOne
            MainWindow.Player0Preview.Fill = new ImageBrush((ImageSource)window.FindResource("TicPZero"));
            MainWindow.Player1Preview.Fill = new ImageBrush((ImageSource)window.FindResource("TicPOne"));
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.UpdateBoard(IAction)"/>
        /// </summary>
        /// <param name="player"></param>
        /// <param name="action"></param>
        /// <exception cref="ArgumentException"></exception>
        public void UpdateBoard(Player player, IAction action)
        {
            Action a = (Action)action;

            // Setup image 
            Rectangle rect = new Rectangle();
            rect.Width = CellSize - CellInnerPad;
            rect.Height = CellSize - CellInnerPad;
            ImageBrush imageBrush;

            // Choose the tile-image based on the next player
            switch (player)
            {
                case Player.Zero:
                    imageBrush = new ImageBrush((ImageSource)MainWindow.FindResource("TicPZero"));
                    rect.Fill = imageBrush;
                    break;
                case Player.One:
                    imageBrush = new ImageBrush((ImageSource)MainWindow.FindResource("TicPOne"));
                    rect.Fill = imageBrush;
                    break;
                default:
                    throw new ArgumentException("Could not place correct tile.");
            }

            // Place tile in the given row/column
            Grid.SetRow(rect, a.Move.Row);
            Grid.SetColumn(rect, a.Move.Column);
            MainWindow.GameGrid.Children.Add(rect);

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.ResetBoard"/>
        /// </summary>
        public void ResetBoard()
        {
            // Clears all placed tiles
            MainWindow.GameGrid.Children.Clear();

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.BindUICallback"/>
        /// </summary>
        public void BindUICallback()
        {
            MainWindow.MouseDown += GameGrid_MouseDown;
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.UnbindUICallback"/>
        /// </summary>
        public void UnbindUICallback()
        {
            MainWindow.MouseDown -= GameGrid_MouseDown;

        }
        #endregion

        #region --- Callbacks ---
        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Gets mouse position relative to the GameBoard, meaning that the most top left tile has the position (0, 0) and
            // the most bottom right one has the position (3 * CellSize, 3 * CellSize).
            // With this, one can abuse this to convert the mouse position to the actual index of row/column.
            var mouseXY = e.GetPosition(MainWindow.GameGrid);
            byte row = (byte)(mouseXY.Y / CellSize);
            byte col = (byte)(mouseXY.X / CellSize);
            
            // Check whether user has clicked inside the bounding box
            if (mouseXY.X < 0 || mouseXY.X > 3 * CellSize || mouseXY.Y < 0 || mouseXY.Y > 3 * CellSize)
            {
                return;
            }

            try
            {
                var result = Game.MakeMove(Game.GetAction(row, col), true);
                if (result != GameResult.NotFinished)
                {
                    MessageBox.Show(result.ToString() + " won!");
                }
            }
            catch (ArgumentException error)
            {
                // Display error if user tried to make invalid move
                MessageBox.Show(error.Message);
            }
        }
        #endregion

        #region --- Private Members ---
        private MainWindow MainWindow;
        private IGame Game;
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace KI_Spiele.Connect_Four
{
    class GameGUI : IGameGUI
    {
        #region --- Constructor ---
        public GameGUI() 
        {
            // Array to store row index of last placed tile
            LastRowMoves = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        }
        #endregion

        #region --- Public Properties ---
        public double CellSize { get; set; } = 150;
        public double CellInnerPad { get; set; } = 10;
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameGUI Interface Implementation ---

        /// <summary>
        /// Implements <see cref="IGameGUI.BindUICallback"/>
        /// </summary>
        public void BindUICallback()
        {
            MainWindow.MouseDown += GameGrid_MouseDown;
        }

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
            window.GameGrid.Width = 7 * CellSize;
            window.GameGrid.Height = 6 * CellSize;

            // Set background
            window.GameGrid.Background = new ImageBrush((ImageSource)window.FindResource("Connect4Board"));

            // Initialize the 6x7 grid in which tiles are placed
            window.GameGrid.ColumnDefinitions.Add(new ColumnDefinition());
            for (int i = 0; i < 6; i++)
            {
                var columnDefinition = new ColumnDefinition();
                var rowDefinition = new RowDefinition();
                window.GameGrid.ColumnDefinitions.Add(columnDefinition);
                window.GameGrid.RowDefinitions.Add(rowDefinition);
            }

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();

            // Set the image previews for PlayerZero and PlayerOne
            MainWindow.Player0Preview.Fill = new ImageBrush((ImageSource)window.FindResource("Connect4PZero"));
            MainWindow.Player1Preview.Fill = new ImageBrush((ImageSource)window.FindResource("Connect4POne"));
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.ResetBoard"/>
        /// </summary>
        public void ResetBoard()
        {
            // Reset indices of last placed rows
            LastRowMoves = new int[] { 0, 0, 0, 0, 0, 0, 0 };

            // Clears all placed tiles
            MainWindow.GameGrid.Children.Clear();

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();
        }

        /// <summary>
        /// Implements <see cref="IGameGUI.UnbindUICallback"/>
        /// </summary>
        public void UnbindUICallback()
        {
            MainWindow.MouseDown -= GameGrid_MouseDown;
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
            ImageBrush imageBrush;

            // Choose the tile-image based on the next player
            switch (player)
            {
                case Player.Zero:
                    imageBrush = new ImageBrush((ImageSource)MainWindow.FindResource("Connect4PZero"));
                    rect.Fill = imageBrush;
                    break;
                case Player.One:
                    imageBrush = new ImageBrush((ImageSource)MainWindow.FindResource("Connect4POne"));
                    rect.Fill = imageBrush;
                    break;
                default:
                    throw new ArgumentException("Could not place correct tile.");
            }

            // Place tile in the given row/column
            // Increment row index of last placed tile
            Grid.SetRow(rect, 5 - LastRowMoves[a.Move]++);
            Grid.SetColumn(rect, a.Move);
            MainWindow.GameGrid.Children.Add(rect);

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();
        }
        #endregion

        #region --- Callbacks ---
        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // Gets mouse position relative to the GameBoard, meaning that the most top left tile has the x-coordinate 0 and
            // the most right one has the x-coordinate 7 * CellSize.
            // With this, one can abuse this to convert the mouse position to the actual index of row/column.
            var mouseXY = e.GetPosition(MainWindow.GameGrid);
            byte col = (byte)(mouseXY.X / CellSize);

            // Check whether user has clicked inside the bounding box
            if (mouseXY.X < 0 || mouseXY.X > 7 * CellSize)
            {
                return;
            }

            try
            {
                Game.MakeMove(Game.GetAction(0, col), true);
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
        // TODO: Don't save the used row inside GUI to avoid out-of-snyc between GameState and GUI
        private int[] LastRowMoves;
        #endregion
    }
}

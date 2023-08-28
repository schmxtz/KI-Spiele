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
        public GameGUI() 
        {
            LastRowMoves = new int[] { 0, 0, 0, 0, 0, 0, 0 };
        }

        #region --- Public Properties ---
        public double CellSize { get; set; } = 150;
        public double CellInnerPad { get; set; } = 10;
        #endregion

        #region --- Public Member Functions ---
        #endregion

        #region --- IGameGUI Interface Implementation ---
        public void BindUICallback()
        {
            MainWindow.MouseDown += GameGrid_MouseDown;
        }

        public void InitializeBoard(IGame game, MainWindow window)
        {
            MainWindow = window;
            Game = game;
            window.GameGrid.Width = 7 * CellSize;
            window.GameGrid.Height = 6 * CellSize;
            window.GameGrid.Background = new ImageBrush((ImageSource)window.FindResource("Connect4Board"));
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

        public void ResetBoard()
        {
            LastRowMoves = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            MainWindow.GameGrid.Children.Clear();

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();
        }

        public void UnbindUICallback()
        {
            MainWindow.MouseDown -= GameGrid_MouseDown;
        }

        public void UpdateBoard(Player player, IAction action)
        {
            Action a = (Action)action;
            Rectangle rect = new Rectangle();
            //rect.Width = CellSize - CellInnerPad;
            //rect.Height = CellSize - CellInnerPad;
            ImageBrush imageBrush;

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

            Grid.SetRow(rect, 5 - LastRowMoves[a.Move]++);
            Grid.SetColumn(rect, a.Move);

            MainWindow.GameGrid.Children.Add(rect);

            // Update GUI, which player is to make the next move
            Player startingPlayer = Game.GetNextPlayer();
            MainWindow.NextPlayer.Text = startingPlayer.ToString();

            GameResult result = Game.GetGameResult();
            if (result != GameResult.NotFinished)
            {
                switch (result)
                {
                    case GameResult.PlayerZero:
                        MainWindow.PlayerZeroWins.Content = long.Parse(MainWindow.PlayerZeroWins.Content.ToString()) + 1;
                        break;
                    case GameResult.PlayerOne:
                        MainWindow.PlayerOneWins.Content = long.Parse(MainWindow.PlayerOneWins.Content.ToString()) + 1;
                        break;
                    case GameResult.Draw:
                        MainWindow.Draws.Content = long.Parse(MainWindow.Draws.Content.ToString()) + 1;
                        break;
                }
            }
        }
        #endregion

        #region --- Private Helper Functions ---
        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseXY = e.GetPosition(MainWindow.GameGrid);
            byte col = (byte)(mouseXY.X / CellSize);

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
                MessageBox.Show(error.Message);
            }
        }
        #endregion

        private MainWindow MainWindow;
        private IGame Game;
        
        // TODO: Don't save the used row inside GUI to avoid out-of-snyc between GameState and GUI
        private int[] LastRowMoves;
    }
}

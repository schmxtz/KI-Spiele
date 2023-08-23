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
        public void InitializeBoard(IGame game, MainWindow window)
        {
            MainWindow = window;
            Game = game;
            window.GameGrid.Width = 3 * CellSize;
            window.GameGrid.Height = 3 * CellSize;
            window.GameGrid.Background = new ImageBrush((ImageSource)window.FindResource("TicBoard"));
            for (int i = 0; i < 3; i++)
            {
                var columnDefinition = new ColumnDefinition();
                var rowDefinition = new RowDefinition();
                window.GameGrid.ColumnDefinitions.Add(columnDefinition);
                window.GameGrid.RowDefinitions.Add(rowDefinition);
            }
        }
        #endregion

        public void UpdateBoard(IAction action)
        {
            Player player = Game.GetNextPlayer();
            Action a = (Action)action;
            Rectangle rect = new Rectangle();
            rect.Width = CellSize - CellInnerPad;
            rect.Height = CellSize - CellInnerPad;
            ImageBrush imageBrush;
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
            Grid.SetRow(rect, a.Move.Row);
            Grid.SetColumn(rect, a.Move.Column);
            MainWindow.GameGrid.Children.Add(rect);

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

        public void ResetBoard()
        {
            MainWindow.GameGrid.Children.Clear();
        }

        public void BindUICallback()
        {
            MainWindow.MouseDown += GameGrid_MouseDown;
        }

        public void UnbindUICallback()
        {
            MainWindow.MouseDown -= GameGrid_MouseDown;

        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseXY = e.GetPosition(MainWindow.GameGrid);
            byte row = (byte)(mouseXY.Y / CellSize);
            byte col = (byte)(mouseXY.X / CellSize);
            
            if (mouseXY.X < 0 || mouseXY.X > 3 * CellSize || mouseXY.Y < 0 || mouseXY.Y > 3 * CellSize)
            {
                return;
            }

            try
            {
                Game.MakeMove(Game.GetAction(row, col), true);
            }
            catch (ArgumentException error)
            {
                MessageBox.Show(error.Message);
            }
        }

        private MainWindow MainWindow;
        private IGame Game;
    }
}

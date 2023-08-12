using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace KI_Spiele.Tic_tac_toe
{
    class GameGUI : IGameGUI
    {
        public void InitializeBoard(IGame game, MainWindow window)
        {
            MainWindow = window;
            Game = game;
            Grid gameGrid = window.GameGrid;
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
            window.MouseDown += GameGrid_MouseDown;
        }

        private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mouseXY = e.GetPosition(MainWindow.GameGrid);
            byte row = (byte)(mouseXY.Y / CellSize);
            byte col = (byte)(mouseXY.X / CellSize);
            
            IAction action = new Action() { Move = (row, col)};
            Game.MakeMove(action);
            UpdateBoard(action);
        }

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
        }

        public void ResetBoard()
        {

        }

        private MainWindow MainWindow;
        private IGame Game;
        private double CellSize = 300;
        private double CellInnerPad = 10;
    }
}

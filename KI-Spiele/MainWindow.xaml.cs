using KI_Spiele.Tic_tac_toe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Action = KI_Spiele.Tic_tac_toe.Action;

namespace KI_Spiele
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            InitializeGUI();
            SetGame("Tic-tac-toe");

            InitializeGameGUI();
        }

        private void InitializeGUI()
        {
            GameSelect.Items.Clear();
            GameSelect.Items.Add("Tic-tac-toe");
            GameSelect.Items.Add("Connect Four");

            GameSelect.SelectionChanged += GameSelected;
            GameSelect.SelectedIndex = 0;

            ModeSelect.Items.Clear();
            ModeSelect.Items.Add("Player vs. Player");
            ModeSelect.Items.Add("Player vs. AI");
            ModeSelect.Items.Add("AI vs. AI");

            ModeSelect.SelectedIndex = 0;
            ModeSelect.SelectionChanged += ModeSelected;

            StartingPlayer.Items.Clear();
            StartingPlayer.Items.Add(Player.Zero);
            StartingPlayer.Items.Add(Player.One);
            StartingPlayer.Items.Add(Player.Undefined);

            StartingPlayer.SelectedIndex = 0;
            StartingPlayer.SelectionChanged += StartingPlayerSelected;
        }

        private void InitializeGameGUI()
        {
            switch ((string)GameSelect.SelectedItem)
            {
                case "Tic-tac-toe":
                    SelectedGame.InitializeBoard(this);
                    break;
                //case "connect four":
                //    selectedgame = new ki_spiele.connect_four.game();
                //    break;
            }

        }

        private void GameSelected(object sender, SelectionChangedEventArgs e)
        {
            string game = (string)GameSelect.SelectedItem;
            SetGame(game);
        }

        private void SetGame(string game)
        {
            switch (game)
            {
                case "Tic-tac-toe":
                    SelectedGame = new KI_Spiele.Tic_tac_toe.Game((Player)StartingPlayer.SelectedIndex);
                    break;
                    //case "connect four":
                    //    selectedgame = new ki_spiele.connect_four.game();
                    //    break;
            }
        }

        private void ModeSelected(object sender, SelectionChangedEventArgs e)
        {

        }

        private void StartingPlayerSelected(object sender, SelectionChangedEventArgs e)
        {
            SelectedGame.StartingPlayer = (Player)StartingPlayer.SelectedItem;
        }

        #region --- Private Members ---
        private IGame SelectedGame;
        #endregion
    }
}

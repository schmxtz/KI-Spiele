using AI.Util;
using KI_Spiele.AI;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
            CultureInfo.CurrentCulture = new CultureInfo("en-US", false);
            InitializeGUI();
            InitializeTimer();
            SetGame("Tic-tac-toe");
            InitializeAI();
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
            ModeSelect.Items.Add("AI vs. Random");

            ModeSelect.SelectedIndex = 0;
            ModeSelect.SelectionChanged += ModeSelected;

            StartingPlayer.Items.Clear();
            StartingPlayer.Items.Add(Player.Zero);
            StartingPlayer.Items.Add(Player.One);
            StartingPlayer.Items.Add(Player.Undefined);

            StartingPlayer.SelectedIndex = 2;
            StartingPlayer.SelectionChanged += StartingPlayerSelected;
        }

        private void InitializeGameGUI()
        {
            switch ((string)GameSelect.SelectedItem)
            {
                case "Tic-tac-toe":
                    // Clear remainders of GameGUI
                    GameGrid.Children.Clear();
                    GameGrid.RowDefinitions.Clear();
                    GameGrid.ColumnDefinitions.Clear();

                    SelectedGame.InitializeBoard(this);
                    break;
                case "Connect Four":
                    // Clear remainders of GameGUI
                    GameGrid.Children.Clear();
                    GameGrid.RowDefinitions.Clear();
                    GameGrid.ColumnDefinitions.Clear();

                    SelectedGame.InitializeBoard(this);
                    break;
            }
        }

        private void InitializeAI()
        {
            QLearningAIZero = new QLearning(Player.Zero)
            {
                LearningRate = Double.Parse(AIZeroAlpha.Text),
                DiscountRate = Double.Parse(AIZeroGamma.Text),
                ExplorationRate = Double.Parse(AIZeroRho.Text),
            };
            QLearningAIOne = new QLearning(Player.One)
            {
                LearningRate = Double.Parse(AIOneAlpha.Text),
                DiscountRate = Double.Parse(AIOneGamma.Text),
                ExplorationRate = Double.Parse(AIOneRho.Text),
            };
            QLearningAIZero.OtherAI = QLearningAIOne;
            QLearningAIOne.OtherAI = QLearningAIZero;
            QLearningAIZero.Game = SelectedGame;
            QLearningAIOne.Game = SelectedGame;
        }

        private void InitializeTimer()
        {
            if (null != TrainTimer)
            {
                TrainTimer.Stop();
            }
            else
            {
                TrainTimer = new DispatcherTimer();
                TrainTimer.Tick += LearnStep;
            }
            if (null != SimulateTimer)
            {
                SimulateTimer.Stop();
            }
            else
            {
                SimulateTimer = new DispatcherTimer();                
                SimulateTimer.Tick += SimulateStep;
            }
        }

        private void GameSelected(object sender, SelectionChangedEventArgs e)
        {
            string game = (string)GameSelect.SelectedItem;
            SetGame(game);
        }

        private void SetGame(string game)
        {
            TrainTimer.Stop();
            SimulateTimer.Stop();
            if (SelectedGame != null)
            {
                SelectedGame.UnbindUICallback();
            }
            switch (game)
            {
                case "Tic-tac-toe":                   
                    SelectedGame = new Tic_tac_toe.Game((Player)StartingPlayer.SelectedIndex);
                    break;
                case "Connect Four":
                    SelectedGame = new Connect_Four.Game((Player)StartingPlayer.SelectedIndex);
                    break;
            }
            InitializeGameGUI();
            SetMode((string)ModeSelect.SelectedItem);
        }

        private void ModeSelected(object sender, SelectionChangedEventArgs e)
        {
            string mode = (string)ModeSelect.SelectedItem;
            SetMode(mode);
        }

        private void SetMode(string mode)
        {
            switch (mode)
            {
                case "Player vs. Player":
                    SelectedGame.BindUICallback();
                    TrainGUI.Visibility = Visibility.Hidden;
                    SimulateGUI.Visibility = Visibility.Hidden;
                    KeyDown -= AIMakeMove;
                    break;
                case "Player vs. AI":
                    SelectedGame.UnbindUICallback();
                    SelectedGame.BindUICallback();
                    TrainGUI.Visibility = Visibility.Hidden;
                    SimulateGUI.Visibility = Visibility.Hidden;
                    KeyDown += AIMakeMove;
                    break;
                case "AI vs. AI":
                    SelectedGame.UnbindUICallback();
                    TrainGUI.Visibility = Visibility.Visible;
                    SimulateGUI.Visibility = Visibility.Visible;
                    KeyDown -= AIMakeMove;
                    break;
                case "AI vs. Random":
                    SelectedGame.UnbindUICallback();
                    TrainGUI.Visibility = Visibility.Visible;
                    SimulateGUI.Visibility = Visibility.Visible;
                    KeyDown -= AIMakeMove;

                    // Reseting the LearnedTable is the equivalent of playing randomly
                    QLearningAIOne.ResetLearnedTable();
                    break;
            }
        }

        private void StartTraining(object sender, RoutedEventArgs e)
        {
            SimulateTimer.Stop();
            InitializeAI();
            CurrentIteration = 0;
            double reward = double.Parse(Reward.Text);
            double penalty = double.Parse(Penalty.Text);
            QLearningAIZero.Reward = reward;
            QLearningAIZero.Penalty = penalty;
            QLearningAIOne.Reward = reward;
            QLearningAIOne.Penalty = penalty;
            NumberIterations = long.Parse(NumIterations.Text);

            if ((string)ModeSelect.SelectedItem == "AI vs. Random")
            {
                Learn = false;
                QLearningAIOne.ExplorationRate = 1.0;
            }
            else
            {
                Learn = true;
            }

            StartTimer(TrainTimer, 0.00001);
        }

        private void StartSimulation(object sender, RoutedEventArgs e)
        {
            TrainTimer.Stop();
            CurrentIteration = 0;
            NumberIterations = long.Parse(NumGames.Text);
            PlayerZeroWins.Content = 0;
            PlayerOneWins.Content = 0;
            Draws.Content = 0;

            if (IsSimulated.IsChecked == true)
            {
                StartTimer(SimulateTimer, 1);
            }
            else
            {
                StartTimer(SimulateTimer, 0.00001);
            }            
        }

        private void ResetGameBtn(object sender, RoutedEventArgs e)
        {
            SelectedGame.ResetGame(true);
        }

        private void StartTimer(DispatcherTimer t, double seconds)
        {
            t.Stop();
            t.Interval = TimeSpan.FromSeconds(seconds);

            t.Start();
        }

        private void SimulateStep(object sender, EventArgs e)
        {
            GameResult result;
            if (CurrentIteration < NumberIterations)
            {
                if (SelectedGame.GetNextPlayer() == Player.Zero)
                {
                    if ((result = QLearningAIZero.MakeMove(false, IsSimulated.IsChecked == true)) != GameResult.NotFinished)
                    {
                        CurrentIteration++;
                        switch (result)
                        {
                            case GameResult.PlayerZero:
                                PlayerZeroWins.Content = long.Parse(PlayerZeroWins.Content.ToString()) + 1;
                                break;
                            case GameResult.PlayerOne:
                                PlayerOneWins.Content = long.Parse(PlayerOneWins.Content.ToString()) + 1;
                                break;
                            case GameResult.Draw:
                                Draws.Content = long.Parse(Draws.Content.ToString()) + 1;
                                break;
                        }
                    }
                }
                else
                {
                    if ((result = QLearningAIOne.MakeMove(false, IsSimulated.IsChecked == true)) != GameResult.NotFinished)
                    {
                        CurrentIteration++;
                        switch (result)
                        {
                            case GameResult.PlayerZero:
                                PlayerZeroWins.Content = long.Parse(PlayerZeroWins.Content.ToString()) + 1;
                                break;
                            case GameResult.PlayerOne:
                                PlayerOneWins.Content = long.Parse(PlayerOneWins.Content.ToString()) + 1;
                                break;
                            case GameResult.Draw:
                                Draws.Content = long.Parse(Draws.Content.ToString()) + 1;
                                break;
                        }
                    }
                }
                ProgressSimulate.Value = (int)(CurrentIteration * 100 / (NumberIterations));
            }
            else
            {
                SimulateTimer.Stop();
                SelectedGame.ResetGame(true);
            }
        }


        private void LearnStep(object sender, EventArgs e)
        {
            long LearnPhase = NumberIterations / 4;
            long LearnSteps = LearnPhase / 100;
            if (CurrentIteration < NumberIterations)
            {
                for (int i = 0; i < LearnSteps;)
                {
                    if (SelectedGame.GetNextPlayer() == Player.Zero)
                    {
                        if(QLearningAIZero.MakeMove() != GameResult.NotFinished) i++;
                    }
                    else
                    {
                        if (QLearningAIOne.MakeMove() != GameResult.NotFinished) i++;
                    }
                }
                CurrentIteration += LearnSteps;
            }
            Progress.Value = (int)(CurrentIteration * 100 / (NumberIterations));

            if (CurrentIteration < LearnPhase)
            {
                QLearningAIZero.LearningRate = 0.5;
                QLearningAIZero.ExplorationRate = 1.0;
                QLearningAIOne.LearningRate = 0.5;
                QLearningAIOne.ExplorationRate = 1.0;
            }
            else if (CurrentIteration < 2 * LearnPhase)
            {
                QLearningAIZero.LearningRate = 0.4;
                QLearningAIZero.ExplorationRate = 0.7;
                if (Learn)
                {
                    QLearningAIOne.LearningRate = 0.4;
                    QLearningAIOne.ExplorationRate = 0.7;
                }
            }
            else if (CurrentIteration < 3 * LearnPhase)
            {
                QLearningAIZero.LearningRate = 0.3;
                QLearningAIZero.ExplorationRate = 0.5;
                if (Learn)
                {
                    QLearningAIOne.LearningRate = 0.3;
                    QLearningAIOne.ExplorationRate = 0.5;
                }
            }
            else if (CurrentIteration < 4 * LearnPhase)
            {
                QLearningAIZero.LearningRate = 0.2;
                QLearningAIZero.ExplorationRate = 0.3;
                if (Learn)
                {
                    QLearningAIOne.LearningRate = 0.2;
                    QLearningAIOne.ExplorationRate = 0.3;
                }
            }
            else
            {
                TrainTimer.Stop();
                SelectedGame.ResetGame(true);
            }
        }

        private void AIMakeMove(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.M:
                    Player curPlayer = SelectedGame.GetNextPlayer();
                    if (curPlayer == Player.One)
                    {
                        QLearningAIOne.MakeMove(false, true);
                        break;
                    }
                    break;
            }
        }

        private void StartingPlayerSelected(object sender, SelectionChangedEventArgs e)
        {
            SelectedGame.StartingPlayer = (Player)StartingPlayer.SelectedItem;
        }

        #region --- Private Members ---
        private IGame SelectedGame;
        private QLearning QLearningAIZero;
        private QLearning QLearningAIOne;

        private long CurrentIteration = 0;
        private long NumberIterations;
        private DispatcherTimer TrainTimer;
        private DispatcherTimer SimulateTimer;
        private bool Learn = true;
        #endregion
    }
}

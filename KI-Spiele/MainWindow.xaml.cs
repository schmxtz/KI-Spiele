using KI_Spiele.Tic_tac_toe;
using System;
using System.Collections.Generic;
using System.Linq;
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
            // GameState g = new GameState();
            Player t = Player.One;
            t = (Player)((1 + (byte)t) % 2);
            Console.WriteLine(t);

            t = Player.Zero;
            t = (Player)((1 + (byte)t) % 2);
            Console.WriteLine(t);

            Console.WriteLine(2 % 2);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Project2_PuzzleGame
{
    /// <summary>
    /// Interaction logic for GameMode.xaml
    /// </summary>
    public partial class GameMode : Window
    {
        public int myGameMode { get; set; }
        public GameMode(int gameMode)
        {
            InitializeComponent();
            if (gameMode == 1)
                Unlimited.IsChecked = true;
            else
                Three_Minutes.IsChecked = true;
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
           if(Unlimited.IsChecked==true)
            {
                myGameMode = 1;
            }
            else
            {
                myGameMode = 2;
            }
            DialogResult = true;
            Close();
        }
    }
}

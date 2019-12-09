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
        public int Time_GameMode { get; set; }
        public int Level_GameMode { get; set; }
        public GameMode(int time_gameMode, int level_gameMode)
        {
            InitializeComponent();
            if (time_gameMode == 1)
                Unlimited.IsChecked = true;
            else
                Three_Minutes.IsChecked = true;
            switch (level_gameMode)
            {
                case 3:
                    EasyMode.IsChecked = true;
                    break;
                case 5:
                    MediumMode.IsChecked = true;
                    break;
                case 7:
                    DifficultMode.IsChecked = true;
                    break;
                default:
                    UserMode.IsChecked = true;
                    UserMode_TextBox.IsEnabled = true;
                    UserMode_TextBox.Text = level_gameMode.ToString();
                    break;
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            if (Unlimited.IsChecked == true)
            {
                Time_GameMode = 1;
            }
            else
            {
                Time_GameMode = 2;
            }

            try
            {
                if (EasyMode.IsChecked == true)
                {
                    Level_GameMode = 3;
                }
                else if (MediumMode.IsChecked == true)
                {
                    Level_GameMode = 5;
                }
                else if (DifficultMode.IsChecked == true)
                {
                    Level_GameMode = 7;
                }
                else
                {
                    Level_GameMode = int.Parse(UserMode_TextBox.Text);

                }
                if (Level_GameMode > 1)
                {
                    DialogResult = true;
                    Close();
                }
                else
                {
                    MessageBox.Show("You need input a number > 1");
                }
            }
            catch
            {
                MessageBox.Show("You need input a number");
            }
        }

        private void UserMode_Checked(object sender, RoutedEventArgs e)
        {
            UserMode_TextBox.IsEnabled = true;
        }

        private void UserMode_Unchecked(object sender, RoutedEventArgs e)
        {
            UserMode_TextBox.IsEnabled = false;
        }
    }
}

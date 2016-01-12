using GOMOKU.View;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GOMOKU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {

            
        }

        

        private void ButtonOffline_Click(object sender, RoutedEventArgs e)
        {
            Gameoffline gameoff = new Gameoffline();
            gameoff.ShowDialog();
           
        }

        private void AutoOnline_Click(object sender, RoutedEventArgs e)
        {
            Game1 game1 = new Game1();
            game1.ShowDialog();
            
        }

        private void Online_Click(object sender, RoutedEventArgs e)
        {
            Game game = new Game();
            game.ShowDialog();
            
        }
    }
}

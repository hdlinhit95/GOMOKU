using GOMOKU.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GOMOKU.View
{
    /// <summary>
    /// Interaction logic for Gameoffline.xaml
    /// </summary>
    public partial class Gameoffline : Window
    {
        Gomoku gameoffline;
        private int Started = 0; //1-PVSP;2-PVSC
        public Gameoffline()
        {
            InitializeComponent();
            gameoffline = new Gomoku(Board);
            
            
        }

        private void ButtonNewgame_Click(object sender, RoutedEventArgs e)
        {

            if (Started == 0)
           {
               PVSP X = new PVSP();
               X.ShowDialog();
               NamePlayer1.Content = X.getPlay1();
               NamePlayer2.Content = X.getPlay2();
               gameoffline.NamePlay1 = X.getPlay1();
               gameoffline.NamePlay2 = X.getPlay2();
               gameoffline.DrawBoard();
               gameoffline.InitArrayOco();
               gameoffline.Start_PvsP();
               Started = 1;
           }
           else
           {
                if (Started == 2)
                {
                    PVSC x = new PVSC();
                    x.ShowDialog();
                    NamePlayer1.Content = x.Name();
                    NamePlayer2.Content = "COM";
                    gameoffline.NamePlay1 = x.Name();
                    gameoffline.NamePlay2 = "COM";
                    gameoffline.Start_P_Com();

                }
                else
                {
                    PVSP X = new PVSP();
                    X.ShowDialog();
                    NamePlayer1.Content = X.getPlay1();
                    NamePlayer2.Content = X.getPlay2();
                    gameoffline.NamePlay1 = X.getPlay1();
                    gameoffline.NamePlay2 = X.getPlay2();
                    gameoffline.Start_PvsP();
                    
                }
            }
                
            
            
            
        }

        private void Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point get = e.GetPosition((UniformGrid)sender);
            if(Started == 0)
            {
                MessageBox.Show("Chua khoi dong!");
            }
            if(Started == 1)
            {
                gameoffline.DanhCo1(get.X, get.Y);
                if (gameoffline.TestWin())
                {
                    gameoffline.EndGame();

                }
            }
            if (Started == 2)
            {
                gameoffline.DanhCo(get.X, get.Y);
                if (gameoffline.TestWin())
                {
                    gameoffline.EndGame();

                }
            }
            
        }

        private void ButtonPvsP_Click(object sender, RoutedEventArgs e)
        {
            if (gameoffline._Ready == false)
            {
                Started = 1;
                PVSP X = new PVSP();
                X.ShowDialog();
                NamePlayer1.Content = X.getPlay1();
                NamePlayer2.Content = X.getPlay2();
                gameoffline.NamePlay1 = X.getPlay1();
                gameoffline.NamePlay2 = X.getPlay2();
                gameoffline.DrawBoard();
                gameoffline.InitArrayOco();
                gameoffline._Ready = true;
            }
            else
            {
                Started = 1;
                PVSP X = new PVSP();
                X.ShowDialog();
                NamePlayer1.Content = X.getPlay1();
                NamePlayer2.Content = X.getPlay2();
                gameoffline.NamePlay1 = X.getPlay1();
                gameoffline.NamePlay2 = X.getPlay2();
                gameoffline.Start_PvsP();
            }
            
        }

        private void ButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonPvsCOM_Click(object sender, RoutedEventArgs e)
        {
            if (gameoffline._Ready == false)
            {
                Started = 2;

                PVSC x = new PVSC();
                
                x.ShowDialog();
                NamePlayer1.Content = x.Name();
                NamePlayer2.Content = "COM";
                gameoffline.NamePlay2 = x.Name();
                gameoffline.NamePlay1 = "COM";
                gameoffline.DrawBoard();
                gameoffline.InitArrayOco();
                gameoffline._Ready = true;
            }
            else
            {
                Started = 2;
                PVSC x = new PVSC();
                x.ShowDialog();
                NamePlayer1.Content = x.Name();
                NamePlayer2.Content = "COM";
                gameoffline.NamePlay2 = x.Name();
                gameoffline.NamePlay1 = "COM";
                gameoffline.Start_P_Com();
            }

           
            
            
            
        }

        private void GameOffline_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (gameoffline.sizeList() == 0)
            {
            }
            else
            {
                gameoffline.reDrawchess();
            }
        }

       
    }
}

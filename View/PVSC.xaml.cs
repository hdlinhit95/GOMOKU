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

namespace GOMOKU.View
{
    /// <summary>
    /// Interaction logic for PVSC.xaml
    /// </summary>
    public partial class PVSC : Window
    {
        String name = "";
        public PVSC()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            name = Player.Text;
            Close();
        }
        public String Name()
        {
            return name;

        }
    }
}

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
    /// Interaction logic for PVSP.xaml
    /// </summary>
    public partial class PVSP : Window
    {
        String _Play1 ="";
        String _Play2 = "";
        public PVSP()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _Play1 = Nguoi1.Text;
            _Play2 = Player2.Text;
            Close();
        }
        public String getPlay1()
        {
            return _Play1;
        }
        public String getPlay2()
        {
            return _Play2;
        }
       
    }
}

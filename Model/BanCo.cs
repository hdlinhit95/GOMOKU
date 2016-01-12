using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;

namespace GOMOKU.Model
{
    

    class BanCo
    {
        
        private int _nRow;
        private int _nCol;
        private UniformGrid _Board;// Dùng để vẽ bàn cờ
        
        public BanCo(int row,int col, UniformGrid board)
        {
            _nCol = row;
            _nRow = col;
            _Board = board;
        }
        #region Vẽ bàn cờ trên uniformgrid và xác định vị trí của ô cờ
        public void DrawBoard()
        {
            SolidColorBrush Gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush White = new SolidColorBrush(Colors.DimGray);
            for (int i = 0; i < _nCol * _nRow; i++)
            {
                Grid cell = new Grid();
                if ((i + i / 12) % 2 == 0)
                {
                    
                    cell.Background = Gray;
                    _Board.Children.Add(cell);


                }
                else
                {
                    cell.Background = White;
                    _Board.Children.Add(cell);
                }
            }
            
        }
        
        #endregion
        public double getWidthOCo()
        {
            return _Board.ActualWidth / 12;
        }
        public double getHeightOCo()
        {
            return _Board.ActualHeight / 12;
        }
        public int nRow { get { return _nRow; } }
        public int nCol { get { return _nCol; } }

        #region Phương thức đánh cờ 
        public void drawChess(int col,int row,int own)
        {
            int i = (12 * (row)) + col;

            //Cờ đánh gồm có 2 màu (trắng đen), nền ô cờ có 2 màu(DimGray và LightGray) => 4 trường hợp
            Ellipse e = new Ellipse();
            e.Width = getWidthOCo()/2;
            e.Height = getWidthOCo()/2;
            e.Stroke = new SolidColorBrush(Colors.Black);
            e.Fill = new SolidColorBrush(Colors.White);
            Ellipse e2 = new Ellipse();
            e2.Width = getWidthOCo() / 2;
            e2.Height = getWidthOCo() / 2;
            e2.Stroke = new SolidColorBrush(Colors.Black);
            e2.Fill = new SolidColorBrush(Colors.White);
            Ellipse e1 = new Ellipse();
            e1.Width = getWidthOCo() / 2;
            e1.Height = getWidthOCo() / 2;
            e1.Stroke = new SolidColorBrush(Colors.Black);
            e1.Fill = new SolidColorBrush(Colors.Black);
            
            Ellipse e3 = new Ellipse();
            e3.Width = getWidthOCo() / 2;
            e3.Height = getWidthOCo() / 2;
            e3.Stroke = new SolidColorBrush(Colors.Black);
            e3.Fill = new SolidColorBrush(Colors.Black);
            SolidColorBrush Gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush White = new SolidColorBrush(Colors.DimGray);
            // Cờ màu trắng nền màu DimGray
            Grid cell = new Grid();
            cell.Width = getWidthOCo();
            cell.Height = getHeightOCo();
            cell.Background = White;
            cell.Children.Add(e);
            //Cờ màu đen nền màu DimGray
            Grid cell1 = new Grid();
            cell1.Width = getWidthOCo();
            cell1.Height = getHeightOCo();
            cell1.Background = White;
            cell1.Children.Add(e1);

            //Cờ màu trắng nền màu LightGray
            Grid cell2 = new Grid();
            cell2.Width = getWidthOCo();
            cell2.Height = getHeightOCo();
            cell2.Background = Gray;
            cell2.Children.Add(e2);
            //Cờ màu đen nền mày LightGray
            Grid cell3 = new Grid();
            cell3.Width = getWidthOCo();
            cell3.Height = getHeightOCo();
            cell3.Background = Gray;
            cell3.Children.Add(e3);
            //Người 1 cờ đen, người 2 cờ trắng
            switch (own)
            {
                case 1:
                    if ((i + i / 12) % 2 == 0)
                    {
                        _Board.Children.RemoveAt(i);
                        _Board.Children.Insert(i, cell3);
                    }
                    else
                    {
                        _Board.Children.RemoveAt(i);
                        _Board.Children.Insert(i, cell1);
                    }
                    break;
                case 2:
                    if ((i + i / 12) % 2 == 0)
                    {
                        _Board.Children.RemoveAt(i);
                        _Board.Children.Insert(i, cell2);
                    }
                    else
                    {
                        _Board.Children.RemoveAt(i);
                        _Board.Children.Insert(i, cell);

                    }
                    break;
                default:
                    break;
            }

        }
        public void redrawOco(int row, int col)
        {
            int i = (12 * (row)) + col;
            SolidColorBrush Gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush White = new SolidColorBrush(Colors.DimGray);
            if ((i + i / 12) % 2 == 0)
            {
                Grid cell = new Grid();
                cell.Width = getWidthOCo();
                cell.Height = getHeightOCo();
                cell.Background = Gray;
                _Board.Children.RemoveAt(i);
                _Board.Children.Insert(i, cell);


            }
            else
            {
                _Board.Children.RemoveAt(i);
                Grid cell = new Grid();
                cell.Width = getWidthOCo();
                cell.Height = getHeightOCo();
                cell.Background = White;
                _Board.Children.Insert(i,cell);
            }
        }
        #endregion
        public void RedrawChessBoard()
        {
            SolidColorBrush Gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush White = new SolidColorBrush(Colors.DimGray);
                        for (int i = 0; i < _nCol*_nRow; i++)
            {
                _Board.Children.RemoveAt(i);

                Grid cell = new Grid();
               
                if ((i + i / 12) % 2 == 0)
                {

                    cell.Background = Gray;
                    _Board.Children.Insert(i,cell);


                }
                else
                {
                    cell.Background = White;
                    _Board.Children.Insert(i,cell);
                }
            }
            
        }

    }
}

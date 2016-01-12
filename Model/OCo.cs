using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace GOMOKU.Model
{
    class OCo
    {
        private int _Row;
        private int _Col;
        Point _Location;
        private int _Own;

        
        public int Row
        {
            set { _Row = value; }
            get { return _Row; }
        }
        public int Col
        {
            set { _Col = value; }
            get { return _Col; }
        }
        public Point Location
        {
            set { _Location = value; }
            get { return _Location; }
        }
        public int Own
        {
            set { _Own = value; }
            get { return _Own; }
        }
        public OCo()
        { }
        public OCo (int col, int row, Point location, int own)
        {
            _Col = col;
            _Row = row;
            _Location = location;
            _Own = own;
        }





    }
}

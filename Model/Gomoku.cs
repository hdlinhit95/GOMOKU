using GOMOKU.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
namespace GOMOKU.Model
{
    public enum End {NotWin,Player1,Player2,COM }
    class Gomoku
    {
        private String _NamePlay1 = "";
        private String _NamePlay2 = "";
        private BanCo _Banco;
        private OCo[,] _MangOco;
        private int turn = 1; //Lượt đi: có 2 giá trị 1 và 2;
        private List<OCo> _CacNuocDaDi;// Lưu lại các các ô cờ đã có chủ sở hữu (các ô bị chiếm)
        private End _End; // Biến End xác định chiến thắng của ai
        public bool _Ready = false;
        public bool Ready
        {
            get { return _Ready; }
            set { _Ready = value; }
        }
        public int sizeList()
        {
            return _CacNuocDaDi.Count;
        }
        public OCo Oco(int i, int j)
        {
            return _MangOco[i, j];
        }
        public void DrawOco(int row, int col)
        {
            _Banco.redrawOco(row,col);
        }
        public void drawChess(int col, int row, int own)
        {
            _Banco.drawChess(col,row,own);
        }
        public String NamePlay2
        {
            get { return _NamePlay2; }
            set { _NamePlay2 = value; }
        }
        public String NamePlay1
        {
            get { return _NamePlay1; }
            set { _NamePlay1 = value; }
        }
        public Gomoku(UniformGrid board)
        {
            _Banco = new BanCo(12,12,board);
            _MangOco = new OCo[12, 12];
            _CacNuocDaDi = new List<OCo>();
        }
        //Show bàn cờ
        public void DrawBoard()
        {
            _Banco.DrawBoard();

        }
        //Tạo ra mảng ô cờ trống, không có ai sở hữu
        public void InitArrayOco()
        {
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)

                {
                    _MangOco[i, j] = new OCo(j, i, new Point(j * _Banco.getWidthOCo(), i * _Banco.getHeightOCo()), 0);
                }
            }
        }
        public void addNuocdi(int row, int col,int own)
        {
            _MangOco[row, col].Own = own;
            _CacNuocDaDi.Add(_MangOco[row, col]);
            
        }
        public OCo getLastList()
        {
            return _CacNuocDaDi[sizeList()];
        }

        public void GetLocation(double x, double y,int X,int Y)
        {
            int col, row;
            row = (int)(y / _Banco.getHeightOCo());
            col = (int)(x /_Banco.getWidthOCo());
            //MessageBox.Show("( "+ (row+1) + " , " +(col+1)+" )");
            X = row;
            Y = col;
        }
        #region Start chế độ chơi
        //Hàm Sẵn sàn chơi game _ chế độ người vs người
        public void Start_PvsP()
        {
            Ready = true;
            ResetChessBoard();
            _CacNuocDaDi = new List<OCo>();
            
        }
        public void reDrawchess()
        {
            for (int i = _CacNuocDaDi.Count - 1; i >= 0; i--)
            {
                _Banco.drawChess(_CacNuocDaDi[i].Col, _CacNuocDaDi[i].Row, _CacNuocDaDi[i].Own);
            }
                
        }
        public void Start_P_Com()
        {

            Ready = true;
            ResetChessBoard();
            _CacNuocDaDi = new List<OCo>();
            


        }
        #endregion

        //Chế độ người đanh vs người 
        public bool DanhCo1(double x, double y) 
        {
            int col, row;
            row = (int)(y / _Banco.getHeightOCo());
            col = (int)(x / _Banco.getWidthOCo());

            if (_MangOco[row,col].Own != 0)
            {
                return false;
            }
            if(Ready = true)
            {
                switch (turn)
                {
                    case 1:
                        _Banco.drawChess(col, row, turn);
                        _MangOco[row, col].Own = 1;
                        turn = 2;
                        break;
                    case 2:
                        _Banco.drawChess(col, row, turn);
                        _MangOco[row, col].Own = 2;
                        turn = 1;
                        break;
                    default:
                        break;
                }
                _CacNuocDaDi.Add(_MangOco[row, col]);
            }
            return true;
            
            
        }

        //Chế độ người đánh trước và máy đánh sau
        public bool DanhCo(double x,double y)
        {
            
                int col, row;
                row = (int)(y / _Banco.getHeightOCo());
                col = (int)(x / _Banco.getWidthOCo());

                if (_MangOco[row, col].Own != 0)
                {
                    return false;
                }
                _Banco.drawChess(col, row, turn);
                _MangOco[row, col].Own = 1;
                turn = 2;
                _CacNuocDaDi.Add(_MangOco[row, col]);

                StartCom(turn);
                turn = 1;
                return true;
                         
        }
       


        #region Kiểm tra chiến thắng
        public void EndGame()
        {
            switch (_End)
            {
                case End.NotWin:
                    MessageBox.Show("Ván cờ hòa!");
                    break;
                case End.Player1:
                    MessageBox.Show(NamePlay1 + "  thắng!");
                    break;
                case End.Player2:
                    MessageBox.Show(NamePlay1 + "  thắng!");
                    break;
                case End.COM:
                    break;
                default:
                    break;
            }
            _Ready = false;
            
        }
        public bool TestWin()
        {
            if(_CacNuocDaDi.Count == 64)
            {
                _End = End.NotWin;
                
                return true;
            }
            foreach (OCo oco in _CacNuocDaDi)
            {
                if (DuyetDoc(oco.Row, oco.Col, oco.Own) || DuyetNgang(oco.Row, oco.Col, oco.Own) || DuyetCheoXuoi(oco.Row, oco.Col, oco.Own) || DuyetCheoNguoc(oco.Row, oco.Col, oco.Own))
                {
                    _End = oco.Own == 1 ? End.Player1 : End.Player2;
                   
                    return true;
                }
                
            }
            
            return false;
        }
        private bool DuyetDoc(int curRow,int curCol,int curOwn)
        {
            if (curRow > _Banco.nRow - 5)
                return false;
            int Count;
            for (Count = 1; Count < 5; Count++)
            {
                if (_MangOco[curRow + Count, curCol].Own != curOwn)
                    return false;
            }
            if (curRow == 0 || curRow + Count == _Banco.nRow)
                return true;
            if (_MangOco[curRow - 1, curCol].Own == 0||_MangOco[curRow + Count,curCol].Own == 0)
                return true;

                return false;
        }
        private bool DuyetNgang(int curRow,int curCol,int curOwn)
        {
            if (curCol > _Banco.nCol - 5)
                return false;
            int Count;
            for (Count = 1; Count < 5; Count++)
            {
                if (_MangOco[curRow, curCol + Count].Own != curOwn)
                    return false;
            }
            if (curCol == 0 || curCol + Count == _Banco.nCol)
                return true;
            if (_MangOco[curRow, curCol - 1].Own == 0||_MangOco[curRow,curCol + Count].Own == 0)
                return true;

                return false;
        }
        private bool DuyetCheoXuoi(int curRow, int curCol, int curOwn)
        {
            if (curCol > _Banco.nCol - 5 || curRow > _Banco.nRow - 5)
                return false;
            int Count;
            for (Count = 1; Count < 5; Count++)
            {
                if (_MangOco[curRow + Count, curCol + Count].Own != curOwn)
                    return false;
            }
            if ( curRow == 0 || curRow + Count == _Banco.nRow ||curCol == 0 || curCol + Count == _Banco.nCol)
                return true;
            if (_MangOco[curRow - 1, curCol - 1].Own == 0 || _MangOco[curRow + Count, curCol + Count].Own == 0)
                return true;
            return false;
        }
        private bool DuyetCheoNguoc(int crRow,int crColumn,int crOwn)
        { 
            /*
            if (curCol < 4 || curCol > _Banco.nCol - 5)
                return false;
            int Count;
            for (Count = 1; Count < 5; Count++)
            {
                if (_MangOco[curRow - Count, curCol + Count].Own != curOwn)
                    return false;
            }
            if (curRow == 4 || curRow  == _Banco.nRow -1 || curCol == 0 || curCol + Count==_Banco.nCol)
                return true;
            if (_MangOco[curRow + 1, curCol - 1].Own == 0 || _MangOco[curRow, curCol + Count].Own == 0)
                return true;
            */
            if (crRow < 4 || crColumn > _Banco.nCol - 5)
                return false;
            int Dem;
            for (Dem = 1; Dem < 5; Dem++)
            {
                if (_MangOco[crRow - Dem, crColumn + Dem].Own != crOwn)
                    return false;
            }
            if (crRow == 4 || crRow == _Banco.nRow - 1 || crColumn == 0 || crColumn + Dem == _Banco.nCol)
                return true;
            if (_MangOco[crRow + 1, crColumn - 1].Own == 0 || _MangOco[crRow, crColumn + Dem].Own == 0)
                return true;
            
            return false;
        }

        #endregion
        //Reset lại bàn cờ
        public void ResetChessBoard()
        {

            
           _CacNuocDaDi.Clear();
            
            for (int i = 0; i < _Banco.nRow; i++)
            {
                for (int j = 0; j < _Banco.nCol; j++)
                {
                    _MangOco[i, j].Own = 0;
                }
            }
                
            InitArrayOco();
            _Banco.RedrawChessBoard();
        }
        #region AI
        private long[] _MD_TC = new long[7] {0,3,24,192,1536,1288,98304};
        private long[] _MD_PT = new long[7] { 0,1,9,81,729,6561,59049}; 


        public void StartCom(int turn)
        {
            if (_CacNuocDaDi.Count == 0)
            {
              _Banco.drawChess(_Banco.nCol/2,_Banco.nRow/2,2);
              _MangOco[_Banco.nCol / 2, _Banco.nRow / 2].Own = turn;
              _CacNuocDaDi.Add(_MangOco[_Banco.nCol / 2, _Banco.nRow / 2]);
            }
            else
            {
                OCo oco = Timkiemnuocdi();
                if(_MangOco[oco.Row, oco.Col].Own ==0)
                {
                    _Banco.drawChess(oco.Col,oco.Row, turn);
                    _MangOco[oco.Row, oco.Col].Own = turn;
                    _CacNuocDaDi.Add(_MangOco[oco.Row, oco.Col]);
                }
                       
            }
 
        }
        public OCo Timkiemnuocdi()
        { 
            OCo _ocoResult = new OCo();
            long _Diem_Max = 0;
            for (int i = 0; i < _Banco.nRow; i++ )
            {
                for (int j = 0; j < _Banco.nCol; j++)
                {
                    if (_MangOco[i,j].Own == 0)
                    {
                        long _Diem_TC = DTC_DuyetDoc(i, j) + DTC_DuyetNgang(i, j) + DTC_DuyetCheoXuoi(i, j) + DTC_DuyetCheoNguoc(i, j);
                        long _Diem_PT = DPT_DuyetNgang(i, j) + DPT_DuyetDoc(i, j) + DPT_DuyetCheoXuoi(i, j) + DPT_DuyetCheoNguoc(i, j);
                        long _Diem_Tam_Thoi = _Diem_TC > _Diem_PT? _Diem_TC:_Diem_PT;
                        long _Diem_Tong = (_Diem_PT + _Diem_TC) > _Diem_Tam_Thoi ? (_Diem_PT + _Diem_TC) : _Diem_Tam_Thoi;
                        if (_Diem_Max < _Diem_Tong)
                        {
                            _Diem_Max = _Diem_Tong;
                            _ocoResult = new OCo(_MangOco[i,j].Col,_MangOco[i,j].Row,_MangOco[i,j].Location,_MangOco[i,j].Own);
                        }
                    }
                }
            }
            return _ocoResult;

        }
        #region DuyetDiemTanCong
        private long DTC_DuyetDoc(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            if (crRow + 1 < _Banco.nRow && _MangOco[crRow + 1, crColumn].Own == 0)
            {

            }
            if (crRow > 0 && _MangOco[crRow - 1, crColumn].Own == 0)
            {

            }
            //
            for (int dem = 1; dem < 5 && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow + dem, crColumn].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow + dem2 < _Banco.nRow; dem2++)
                        if (_MangOco[crRow + dem2, crColumn].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow + dem2, crColumn].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crRow - dem >= 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow - dem, crColumn].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow - dem2 >= 0; dem2++)
                        if (_MangOco[crRow - dem2, crColumn].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow - dem2, crColumn].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQD == 2)
                return 0;
            if (_SoQD == 0)
                _Diem_Tong += _MD_TC[_SoQT] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT];
            if (_SoQD2 == 0)
                _Diem_Tong += _MD_TC[_SoQT2] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT2];
            if (_SoQT >= _SoQT2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQT == 4)
                _Diem_Tong *= 2;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            return _Diem_Tong;
        }
        private long DTC_DuyetNgang(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            if (crColumn + 1 < _Banco.nCol && _MangOco[crRow, crColumn + 1].Own == 0)
            {

            }
            if (crColumn > 0 && _MangOco[crRow, crColumn - 1].Own == 0)
            {

            }
            //
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol; dem++)
            {
                if (_MangOco[crRow, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn + dem2 < _Banco.nCol; dem2++)
                        if (_MangOco[crRow, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0; dem++)
            {
                if (_MangOco[crRow, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn - dem2 >= 0; dem2++)
                        if (_MangOco[crRow, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQD == 2)
                return 0;
            if (_SoQD == 0)
                _Diem_Tong += _MD_TC[_SoQT] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT];
            if (_SoQD2 == 0)
                _Diem_Tong += _MD_TC[_SoQT2] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT2];
            if (_SoQT >= _SoQT2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQT == 4)
                _Diem_Tong *= 2;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            return _Diem_Tong;
        }
        private long DTC_DuyetCheoXuoi(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            if (crRow + 1 < _Banco.nRow && crColumn + 1 < _Banco.nCol && _MangOco[crRow + 1, crColumn + 1].Own == 0)
            {

            }
            if (crRow > 0 && crColumn > 0 && _MangOco[crRow - 1, crColumn - 1].Own == 0)
            {

            }
            //
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow + dem, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn + dem2 < _Banco.nCol && crRow + dem2 < _Banco.nRow; dem2++)
                        if (_MangOco[crRow + dem2, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow + dem2, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0 && crRow - dem >= 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow - dem, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn - dem2 >= 0 && crRow - dem2 >= 0; dem2++)
                        if (_MangOco[crRow - dem2, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow - dem2, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQD == 2)
                return 0;
            if (_SoQD == 0)
                _Diem_Tong += _MD_TC[_SoQT] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT];
            if (_SoQD2 == 0)
                _Diem_Tong += _MD_TC[_SoQT2] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT2];
            if (_SoQT >= _SoQT2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;

            if (_SoQT == 4)
                _Diem_Tong *= 2;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            return _Diem_Tong;
        }
        private long DTC_DuyetCheoNguoc(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            if (crRow > 0 && crColumn + 1 < _Banco.nCol && _MangOco[crRow - 1, crColumn + 1].Own == 0)
            {

            }
            if (crRow + 1 < _Banco.nRow && crColumn > 0 && _MangOco[crRow + 1, crColumn - 1].Own == 0)
            {

            }
            //
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol && crRow - dem > 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow - dem, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn + dem2 < _Banco.nCol && crRow - dem2 > 0; dem2++)
                        if (_MangOco[crRow - dem2, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow - dem2, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0 && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                }
                else if (_MangOco[crRow + dem, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                    break;
                }
                else // Own = 0
                {
                    for (int dem2 = 1; dem2 < 6 && crColumn - dem2 >= 0 && crRow + dem2 < _Banco.nRow; dem2++)
                        if (_MangOco[crRow + dem2, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                        }
                        else if (_MangOco[crRow + dem2, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                            break;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQD == 2)
                return 0;
            if (_SoQD == 0)
                _Diem_Tong += _MD_TC[_SoQT] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT];
            if (_SoQD2 == 0)
                _Diem_Tong += _MD_TC[_SoQT2] * 2;
            else
                _Diem_Tong += _MD_TC[_SoQT2];
            if (_SoQT >= _SoQT2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQT == 4)
                _Diem_Tong *= 2;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            return _Diem_Tong;
        }
        #endregion
        #region DuyetDiemPhongThu
        private long DPT_DuyetDoc(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            for (int dem = 1; dem < 5 && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow + dem, crColumn].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow + dem2 < _Banco.nRow; dem2++)
                        if (_MangOco[crRow + dem, crColumn].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow + dem, crColumn].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crRow - dem >= 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow - dem, crColumn].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow - dem2 >= 0; dem2++)
                        if (_MangOco[crRow - dem2, crColumn].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow - dem2, crColumn].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQT == 2)
                return 0;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            /* 
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            */
            if (_SoQD >= _SoQD2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQD == 4)
                _Diem_Tong *= 2;
            return _Diem_Tong;
        }
        private long DPT_DuyetNgang(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol; dem++)
            {
                if (_MangOco[crRow, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn + dem2 < _Banco.nCol; dem2++)
                        if (_MangOco[crRow, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0; dem++)
            {
                if (_MangOco[crRow, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn - dem2 >= 0; dem2++)
                        if (_MangOco[crRow, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else break;
                    break;
                }
            }
            if (_SoQT == 2)
                return 0;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            /* 
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            */
            if (_SoQD >= _SoQD2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQD == 4)
                _Diem_Tong *= 2;
            return _Diem_Tong;
        }
        private long DPT_DuyetCheoXuoi(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow + dem, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow + dem2 < _Banco.nRow && crColumn + dem2 < _Banco.nCol; dem2++)
                        if (_MangOco[crRow + dem2, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow + dem2, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0 && crRow - dem >= 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow - dem, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crColumn - dem2 >= 0 && crRow - dem2 >= 0; dem2++)
                        if (_MangOco[crRow - dem2, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow - dem2, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQT == 2)
                return 0;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            /* 
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            */
            if (_SoQD >= _SoQD2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQD == 4)
                _Diem_Tong *= 2;
            return _Diem_Tong;
        }
        private long DPT_DuyetCheoNguoc(int crRow, int crColumn)
        {
            long _Diem_Tong = 0;
            int _SoQT = 0;
            int _SoQD = 0;
            int _SoQT2 = 0;
            int _SoQD2 = 0;
            for (int dem = 1; dem < 5 && crColumn + dem < _Banco.nCol && crRow - dem > 0; dem++)
            {
                if (_MangOco[crRow - dem, crColumn + dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow - dem, crColumn + dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow - dem2 >= 0 && crColumn + dem2 < _Banco.nCol; dem2++)
                        if (_MangOco[crRow - dem2, crColumn + dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow - dem2, crColumn + dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            for (int dem = 1; dem < 5 && crColumn - dem >= 0 && crRow + dem < _Banco.nRow; dem++)
            {
                if (_MangOco[crRow + dem, crColumn - dem].Own == 1)
                {
                    _SoQT++;
                    break;
                }
                else if (_MangOco[crRow + dem, crColumn - dem].Own == 2)
                {
                    _SoQD++;
                }
                else // Own = 0
                {
                    for (int dem2 = 2; dem2 < 6 && crRow + dem2 < _Banco.nRow && crColumn - dem2 >= 0; dem2++)
                        if (_MangOco[crRow + dem2, crColumn - dem2].Own == 1)
                        {
                            _SoQT2++;
                            break;
                        }
                        else if (_MangOco[crRow + dem2, crColumn - dem2].Own == 2)
                        {
                            _SoQD2++;
                        }
                        else
                            break;
                    break;
                }
            }
            if (_SoQT == 2)
                return 0;
            if (_SoQT == 0)
                _Diem_Tong += _MD_PT[_SoQD] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD];
            /* 
            if (_SoQT2 == 0)
                _Diem_Tong += _MD_PT[_SoQD2] * 2;
            else
                _Diem_Tong += _MD_PT[_SoQD2];
            */
            if (_SoQD >= _SoQD2)
                _Diem_Tong -= 1;
            else
                _Diem_Tong -= 2;
            if (_SoQD == 4)
                _Diem_Tong *= 2;
            return _Diem_Tong;
        }
        #endregion

    }
        #endregion

    
}
    

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
using Quobject.SocketIoClientDotNet.Client;
using System.Windows.Threading;
using System.Threading;
using Newtonsoft.Json.Linq;

namespace GOMOKU.View
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Window
    {
        List<OCo> HighLight = new List<OCo>();
        string time = DateTime.Now.ToString("h:mm:ss tt");
        bool nameClick = false;
        string NamePlayer = "";
        Gomoku gomoku;
        Socket socket = IO.Socket("ws://gomoku-lajosveres.rhcloud.com:8000");
        public Game()
        {
           
            InitializeComponent();
            gomoku = new Gomoku(_Board);
            gomoku.DrawBoard();
            gomoku.InitArrayOco();
            NewSocket();
            
        }
        private void NewSocket()
        {
            socket.On(Socket.EVENT_CONNECT, () =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    chatView.Text += "Server: " + "Connected\n" + "(" + time + ")" + "\n";
                });

            });
            socket.On(Socket.EVENT_MESSAGE, (data) =>
            {
                MessageBox.Show(data.ToString());

            });
            socket.On(Socket.EVENT_CONNECT_ERROR, (data) =>
            {
                chatView.Text += data.ToString() + "\n";
            });
            socket.On("ChatMessage", (data) =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    string text = (string)((JObject)data)["message"];
                    text = text.Replace("<br />", "\n");
                    string from = (string)((JObject)data)["from"];
                    if (from == null)
                        from = "Server";
                    chatView.Text += from + ": " + text + "\n" + "(" + time + ")" + "\n";
                });
            });
            
            socket.On(Socket.EVENT_ERROR, (data) =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    chatView.Text += data.ToString() + "\n";
                });
            });
            socket.On("EndGame", (data) =>
            {
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    string text = (string)((JObject)data)["message"];
                    chatView.Text += "Server: " + text + "\n" + "(" + time + ")" + "\n";
                    try
                    {
                        for (int i = 0; i < 5; i++)
                        {
                          HighLight.Add(gomoku.Oco((int)((JObject)data)["highlight"][i]["row"],(int)((JObject)data)["highlight"][i]["col"]));
                          
                        }
                        DispatcherTimer dispatcherTimer = new DispatcherTimer();
                        dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100000);
                        dispatcherTimer.Start();
                    }
                    catch (ArgumentOutOfRangeException e)
                    { }
                });

            });
            socket.On("NextStepIs", (data) =>
            {
                //this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                //{
                //    chatView.Text += "NextStepIs: " + data.ToString();
                //});
                int own;
                int isTurn = (int)((JObject)data)["player"];
                if (isTurn == 0) own = 1;
                else own = 2;
                int x = (int)((JObject)data)["row"];
                int y = (int)((JObject)data)["col"];
                this.Dispatcher.Invoke(DispatcherPriority.Normal, (ThreadStart)delegate
                {
                    gomoku.drawChess(y, x, own);
                    gomoku.addNuocdi(x, y, own);

                });
            });
        }

        //Hám nhâp nháy các quân cờ chiến thắng
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            for (int j = 0; j < 100;j++ )
                for (int i = 0; i < 5; i++)
                {
                    gomoku.DrawOco(HighLight[i].Row, HighLight[i].Col);
                    gomoku.drawChess(HighLight[i].Col, HighLight[i].Row, HighLight[i].Own);
                }
        }

        

        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {
            
            
            if (TextBoxYourname.Text == "")
            {
                MessageBox.Show("Nhập tên vào");
            }
            else
            {
                socket.Emit("MyNameIs",TextBoxYourname.Text);
                socket.Emit("ConnectToOtherPlayer");

            }
        }

        private void ButtonSend_Click(object sender, RoutedEventArgs e)
        {
            if (TextBoxChat.Text == "")
            {
                MessageBox.Show("Hãy nhập nội dung chat");
            }
            else
            {
                socket.Emit("ChatMessage", TextBoxChat.Text);
                TextBoxChat.Clear();
            }
        }

        private void _Board_MouseDown(object sender, MouseButtonEventArgs e)
        {
            int x, y;
            x = (int)(e.GetPosition((IInputElement)sender).Y / (_Board.ActualHeight/12));
            y = (int)(e.GetPosition((IInputElement)sender).X / (_Board.ActualWidth / 12));
            socket.Emit("MyStepIs", JObject.FromObject(new { row = x, col = y }));
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (gomoku.sizeList() == 0)
            {
            }
            else
            {
                gomoku.reDrawchess();
            }
        }

        
            
       
       

    }
}

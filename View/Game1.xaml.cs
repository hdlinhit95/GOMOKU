using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using GOMOKU.Model;
using Newtonsoft.Json.Linq;
using Quobject.SocketIoClientDotNet.Client;

namespace GOMOKU.View
{
    /// <summary>
    /// Interaction logic for Game1.xaml
    /// </summary>
    public partial class Game1 : Window
    {
        List<OCo> HighLight = new List<OCo>();
        string time = DateTime.Now.ToString("h:mm:ss tt");
        bool nameClick = false;
        string NamePlayer = "";
        Gomoku gomoku;
        Socket socket = IO.Socket("ws://gomoku-lajosveres.rhcloud.com:8000");
        public Game1()
        {
            InitializeComponent();
            gomoku = new Gomoku(_Board);
            gomoku.DrawBoard();
            gomoku.InitArrayOco();
            NewSocket();
        }
        public void NewSocket()
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
                    if (text.IndexOf("You are the first player!")!=-1)
                    {
                       
                        socket.Emit("MyStepIs", JObject.FromObject(new { row = 5, col =5 }));
                    }
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
                            HighLight.Add(gomoku.Oco((int)((JObject)data)["highlight"][i]["row"], (int)((JObject)data)["highlight"][i]["col"]));

                        }
                        //DispatcherTimer dispatcherTimer = new DispatcherTimer();
                        //dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                        //dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 100000);
                        //dispatcherTimer.Start();
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
                    
                    if (isTurn == 0)
                    {
                        gomoku.addNuocdi(x, y, own);
                        gomoku.drawChess(y, x, own);
                    }
                    else
                    {              
                        gomoku.drawChess(y, x, own);
                        gomoku.addNuocdi(x, y, own);
                        if (gomoku.sizeList() == 1)
                        {
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = x + 1, col = y + 1 }));
                        }
                        else
                        {
                            OCo oco = gomoku.Timkiemnuocdi();
                            socket.Emit("MyStepIs", JObject.FromObject(new { row = oco.Row, col = oco.Col }));
                        }
                        
                    }
                });
            });




        }

        





        private void ButtonChange_Click(object sender, RoutedEventArgs e)
        {

            if (TextBoxYourname.Text == "")
            {
                MessageBox.Show("Nhập tên vào");
            }
            else
            {
                socket.Emit("MyNameIs", TextBoxYourname.Text);
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

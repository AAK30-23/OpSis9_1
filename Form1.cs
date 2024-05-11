using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using SimpleTCP;



// Серверная часть

namespace OpSis9_1
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpServer server;

        private void Form1_Load(object sender, EventArgs e)
        {
            server = new SimpleTcpServer();
            server.Delimiter = 0x13; //разделитель для сообщений
            server.StringEncoder = Encoding.UTF8; //кодировки для преобразования строк в байты и наоборот
            server.DataReceived += Server_DataReceived;
        }

        private void Server_DataReceived(object sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
                e.ReplyLine(string.Format("Ваше сообщение: {0}", e.MessageString));
            });
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            txtStatus.Text += "Сервер включен...";
                // Разбиваем строку на октеты и преобразуем их в массив байтов
            byte[] ipBytes = Array.ConvertAll(txtHost.Text.Split('.'), byte.Parse);
            //  объект IPAddress из массива байтов
            System.Net.IPAddress ip = new System.Net.IPAddress(ipBytes);
            //System.Net.IPAddress ip = new System.Net.IPAddress(long.Parse(txtHost.Text));
            server.Start(ip, Convert.ToInt32(txtPort.Text));
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if(server.IsStarted)
            {
                server.Stop();
            }
        }
    }
            
}
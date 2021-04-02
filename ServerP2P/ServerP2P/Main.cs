using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ServerP2P
{
    public partial class Main : Form
    {
        private Server server = Server.getInstance();
        private List<Client> clients = new List<Client>();
        public Main()
        {

            InitializeComponent();
            try
            {
                label4.Text = "" + server.IP;
                label5.Text = "" + server.Port;
            }
            catch (Exception ex)
            {
                this.Close();
                MessageBox.Show(ex.Message);
            }
        }


        private void Main_Load(object sender, EventArgs e)
        {
            server.listener.Bind(new IPEndPoint(IPAddress.Parse(server.IP), server.Port));
            Thread receiving = new Thread(() => ListeningNewClients());
            receiving.IsBackground = true;
            receiving.Start();
        }

        private void ListeningNewClients()
        {
            while (true)
            {
                server.listener.Listen(0);// One client at one time
                Socket temp = server.listener.Accept();
                Client newClient = new Client();
                newClient.Connector = temp;
                temp = null;
                clients.Add(newClient);/*Appending new client to Cient's list.*/

                Thread receiving = new Thread(() => CheckingIncomingMessage(newClient));
                receiving.IsBackground = true;
                receiving.Start();
                SetText("New peer Connected :");
            }
        }
        delegate void SetTextCallback(string text);

        private void SetText(string text)
        {
            // InvokeRequired required compares the thread ID of the
            // calling thread to the thread ID of the creating thread.
            // If these threads are different, it returns true.
            if (this.listBox1.InvokeRequired)
            {
                SetTextCallback d = new SetTextCallback(SetText);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.listBox1.Items.Add (text);
            }
        }
        private void CheckingIncomingMessage(Client thisClient)
        {
            while (true)
            {
                try
                {

                    byte[] buffer = new byte[1500];
                    int msgLength = thisClient.Connector.Receive(buffer, 0, buffer.Length, 0);
                    char[] chars = new char[msgLength];
                    Encoding.Default.GetDecoder().GetChars(buffer, 0, msgLength, chars, 0);
                    String message = new String(chars);
                    
                    if (message[0] == '±')
                    {
                        String extractedMsg = message.Substring(message.IndexOf('±') + 1);//Ignoring '±' from message
                        thisClient.IP = extractedMsg.Substring(0, extractedMsg.IndexOf(":"));//Saving IP
                        thisClient.Port = Convert.ToInt32(extractedMsg.Substring(extractedMsg.IndexOf(':') + 1));//Saving Port
                        SetText("New Client added, IP:" + thisClient.IP + ", and Port:" + thisClient.Port + ", and name:" + thisClient.name);
                    }
                   
                    else
                    {
                        for (int i = 0; i < clients.Count(); i++)
                        {
                            if (clients[i] == thisClient)
                                continue;// To prevent sending message to thisClient
                            String msgToSendInGroup = message;
                            SendMessageToClient(thisClient.Port + " : " + msgToSendInGroup, clients[i].Connector);
                        }
                    }
                }
                catch (Exception ex)
                {
                    SetText(thisClient.Port + " client left. " + ex.Message);
                    clients.Remove(thisClient);
                    return;//No more this client alive, so we close accpting things from it
                }
            }
        }

        private void SendMessageToClient(String msg, Socket destinClient)
        {
            destinClient.Send(Encoding.Default.GetBytes(msg));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

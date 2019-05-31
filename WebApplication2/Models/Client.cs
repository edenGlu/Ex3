using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ex3.Models
{
    // connect to the server and responsible for sending and receiving messages from him
    public class Client
    {
        
        private TcpClient client = null;
        private NetworkStream stream;
        private BinaryWriter writer;
        private BinaryReader reader;
        /*
            * Connect to the port and ip that get.
            */
        public void Connect(int port, string ip)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse(ip), port);
            client = new TcpClient(); // cread new TCP in evry connect
            client.Connect(ep);
            stream = client.GetStream();
            writer = new BinaryWriter(stream);
            reader = new BinaryReader(stream);
        }
        // send the msg to the simulator and get the comment
        public double Send(string msg)
        {
            byte[] b = Encoding.ASCII.GetBytes(msg);
            stream.Write(b, 0, b.Length);
            byte[] values = new byte[client.ReceiveBufferSize];
            int x = stream.Read(values, 0, values.Length); // get the data
            string data = Encoding.ASCII.GetString(values, 0, x).Split(' ')[2];
            double num = Convert.ToDouble(data.Substring(1, data.Length - 2));
            return num ;
        }
        // close if the socket open
        public void CLoseClient()
        {
            if (client != null)
            {
                client.Close();
            }
        }
    }
}

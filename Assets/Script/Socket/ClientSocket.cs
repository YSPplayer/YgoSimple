using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;


namespace TCGame.Client.CSocket
{
    public class ClientSocket
    {
        private static Socket clientSocket;
        private static string serverIp = "192.168.43.78";
        private static int port = 7120;
        private const int MAX_BUFFER = 1024;
        /// <summary>
        ///初始化客户端服务器 
        /// </summary>
        /// <returns></returns>
        public static bool InitClientSocket()
        {
            try
            {
                //v4 stream tcp
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverIP = IPAddress.Parse(serverIp);
                // 连接服务器
                clientSocket.Connect(new IPEndPoint(serverIP, port));
                return true;
            }
            catch (Exception e) {
                Debug.Log(e.Message);
                return false;
            }
        }

        public static int SendMessageSync(string message)
        {
            //发送信息给服务器
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            return clientSocket.Send(buffer);
        }
        public static void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
        }
        public static string ReceiveMessageSync(out bool result)
        {
            byte[] buffer = new byte[MAX_BUFFER];
            int len = clientSocket.Receive(buffer);
            if (len == 0)
            {
                result = false;
                return "";
            }
            else
            {
                result = true;
                return Encoding.UTF8.GetString(buffer, 0, len);
            }

        }
        public static void ReceiveMessage()
        {
            byte[] buffer = new byte[MAX_BUFFER];
            //开始异步线程接收
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        private static void SendCallback(IAsyncResult result)
        {
            try
            {
                //执行回调函数，这个是返回发送的字节数
                int bytesSent = clientSocket.EndSend(result);

            }
            catch (Exception ex)
            {
                Debug.Log(ex.Message);
            }
        }
        private static void ReceiveCallback(IAsyncResult ar)
        {
            byte[] buffer = new byte[MAX_BUFFER];
            // 完成异步数据接收
            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                // 处理接收到的数据
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // 继续异步接收数据
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                // 连接已断开
                clientSocket.Close();
            }

        }
    }
}
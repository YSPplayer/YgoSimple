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
        ///��ʼ���ͻ��˷����� 
        /// </summary>
        /// <returns></returns>
        public static bool InitClientSocket()
        {
            try
            {
                //v4 stream tcp
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress serverIP = IPAddress.Parse(serverIp);
                // ���ӷ�����
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
            //������Ϣ��������
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
            //��ʼ�첽�߳̽���
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
        }
        private static void SendCallback(IAsyncResult result)
        {
            try
            {
                //ִ�лص�����������Ƿ��ط��͵��ֽ���
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
            // ����첽���ݽ���
            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                // ������յ�������
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                // �����첽��������
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            else
            {
                // �����ѶϿ�
                clientSocket.Close();
            }

        }
    }
}
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using TCGame.Client.Core;


namespace TCGame.Client.CSocket
{
    public static class SocketManage
    {
        private static Socket clientSocket;
        private static string serverIp = "192.168.43.78";
        private static int port = 7120;
        private const int MAX_BUFFER = 1024;
        private static byte[] receiveBuffer = new byte[MAX_BUFFER];
        /*
             ��һ������˼·��
             ������˫��ģʽ�£�
             ��ȡ��ҿ�������-> ���͸�������->�����ݷ��ظ�˫��
        ->���ݷ��ص����ݹ�����Ƭ����
            ����server����->��ȡ��ǰʱ��״̬->����ʱ��״̬->����UI
            
            
            ������ʼʱ�㣺
             ��ȡai��������->���͸�������->�����ݷ��ظ�˫�� 
        ->���ݷ��ص����ݹ�����Ƭ����

         */
        public static bool StartServer()
        {
            if (CoreApI.CreateSocketServer() != CoreApI.SUCCESS_INIT) return false;
            if (CoreApI.BindSocketServer() != CoreApI.SUCCESS_BIND) return false;
            CoreApI.StartServer();//�����첽�̵߳ķ�����
            return true;
        }

        public static void CloseServer()
        {
            CoreApI.CloseServer();
        }

        /// <summary>
        ///��ʼ���ͻ���socket
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
                Close();
                return false;
            }
        }

        public static int SendMessageSync(string message)
        {
            //������Ϣ��������
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            try
            {
                return clientSocket.Send(buffer);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Close();
                return -1;
            }
        }
        public static void SendMessage(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            try
            {
                clientSocket.BeginSend(buffer, 0, buffer.Length, SocketFlags.None, SendCallback, null);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Close();
                return;
            }
        }
        public static string ReceiveMessageSync()
        {
            byte[] buffer = new byte[MAX_BUFFER];
            int len = -1;
            try
            {
                clientSocket.Receive(buffer);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Close();
                return null;
            }
            if (len <= 0)
            {
                return null;
            }
            else
            {
                return Encoding.UTF8.GetString(buffer, 0, len);
            }

        }

        public static void ReceiveMessage()
        {
            try
            {
                //��ʼ�첽�߳̽���
                clientSocket.BeginReceive(receiveBuffer, 0, receiveBuffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                Close();
                return;
            }
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
            // ����첽���ݽ��գ���ȡ���� ��Ҫtry
            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                // ������յ�������
                string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                Debug.Log(receivedData);
                // �����첽��������
                ReceiveMessage();
            }
        }
        //�رշ���������
        public static void Close()
        {
            if (clientSocket != null)
            {
                try
                {
                    // ���÷��ͺͽ���
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    // �������������ѹرն������쳣
                    Debug.Log("Shutdown exception: " + ex.Message);
                }
                try
                {
                    // �ر�socket
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    // ����ر�ʱ���κ��쳣
                    Console.WriteLine("Close exception: " + ex.Message);
                }
                CoreApI.CloseServer();
            }
        }
    }
}
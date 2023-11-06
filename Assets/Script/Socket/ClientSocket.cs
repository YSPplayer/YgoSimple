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
             下一步开发思路：
             比如是双人模式下：
             获取玩家卡组数据-> 发送给服务器->把数据返回给双方
        ->根据返回的数据构建卡片对象
            发送server请求->获取当前时点状态->返回时点状态->构建UI
            
            
            决斗开始时点：
             获取ai卡组数据->发送给服务器->把数据返回给双方 
        ->根据返回的数据构建卡片对象

         */
        public static bool StartServer()
        {
            if (CoreApI.CreateSocketServer() != CoreApI.SUCCESS_INIT) return false;
            if (CoreApI.BindSocketServer() != CoreApI.SUCCESS_BIND) return false;
            CoreApI.StartServer();//开启异步线程的服务器
            return true;
        }

        public static void CloseServer()
        {
            CoreApI.CloseServer();
        }

        /// <summary>
        ///初始化客户端socket
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
                Close();
                return false;
            }
        }

        public static int SendMessageSync(string message)
        {
            //发送信息给服务器
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
                //开始异步线程接收
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
            // 完成异步数据接收，获取数组 需要try
            int bytesRead = clientSocket.EndReceive(ar);
            if (bytesRead > 0)
            {
                // 处理接收到的数据
                string receivedData = Encoding.UTF8.GetString(receiveBuffer, 0, bytesRead);
                Debug.Log(receivedData);
                // 继续异步接收数据
                ReceiveMessage();
            }
        }
        //关闭服务器连接
        public static void Close()
        {
            if (clientSocket != null)
            {
                try
                {
                    // 禁用发送和接收
                    clientSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception ex)
                {
                    // 可能由于连接已关闭而引发异常
                    Debug.Log("Shutdown exception: " + ex.Message);
                }
                try
                {
                    // 关闭socket
                    clientSocket.Close();
                }
                catch (Exception ex)
                {
                    // 处理关闭时的任何异常
                    Console.WriteLine("Close exception: " + ex.Message);
                }
                CoreApI.CloseServer();
            }
        }
    }
}
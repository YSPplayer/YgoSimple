#ifndef SOCKETSERVER_H
#define SOCKETSERVER_H
#include <iostream>
#include <winsock2.h>
#include <thread>
#include <functional>
#pragma comment(lib, "ws2_32.lib")

#define SYSTEM_WINDOWS 0
#define SYSTEM_LIUNX 1
#define SERVER_IP "192.168.43.78"  
#define PORT 7120//默认运行端口
namespace Core{
	namespace Socket{
class SocketManger{
	private: 
		static SOCKET serverListen;//这个是我们的服务器网络监听socket
		static sockaddr_in sockAddr;//这个是我们自己的地址
	public:
		static bool serverLoop;//服务器线程是否循环
		static UINT8 Init();//初始化函数
		static UINT8 Bind();//监听字相关
		static void Start();//开启异步线程,启动server
		static void Receive();//循环接收数据的异步线程函数
		static UINT8 Close();//关闭服务器的socket

	};
	#define ERROR_NET_INIT 1 //网络初始化失败
	#define ERROR_SOCKET_INIT 2 //套接字初始化失败
	#define SUCCESS_INIT 3 //套接字创建成功
	#define ERROR_BIND_SOCKET 4 //绑定套接字失败
	#define ERROR_BIND_LISTEN 5 //监听失败
	#define SUCCESS_BIND 6 //绑定监听器成功
    #define ERROR_CLOSE 7 //服务器关闭失败
	#define SUCCESS_CLOSE 8 //服务器关闭成功
	#define BUFFER_MAX 1024 //收发数据的最大字节数


	}
	
}



#endif // !SOCKETSERVER_H

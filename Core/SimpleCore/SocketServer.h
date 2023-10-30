#ifndef SOCKETSERVER_H
#define SOCKETSERVER_H
#include <iostream>
#include <winsock2.h>
#pragma comment(lib, "ws2_32.lib")

#define SERVER_IP "192.168.43.78"  
#define PORT 7120//默认运行端口
namespace Core{
	namespace Socket{
class SocketManger{
	private: 
		SOCKET serverListen;//这个是我们的服务器网络监听socket
		sockaddr_in sockAddr;//这个是我们自己的地址
	public:
		SocketManger(){};
		~SocketManger(){};
		UINT8 Init();//初始化函数
	};
	#define ERROR_NET_INIT 0 //网络初始化失败
	#define ERROR_SOCKET_INIT 1 //套接字初始化失败
	#define SUCCESS_INIT 12 //套接字创建成功

	}
}



#endif // !SOCKETSERVER_H

#include "SocketServer.h"

using namespace Core::Socket;
//static成员变量要初始化，不初始化编译器报错，这里我们直接把它写出来就初始化了
bool SocketManger::serverLoop = false;
SOCKET SocketManger::serverListen;
sockaddr_in SocketManger::sockAddr;
//初始化服务器函数
UINT8 SocketManger::Init() {
	//1.初始化网络
	WSADATA data;
	int ret =  WSAStartup(MAKEWORD(2,2),&data);
	//网络初始化失败
	if(ret != 0) return ERROR_NET_INIT;
	//2.初始化socket对象
	SocketManger::serverListen = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	//socket初始化失败
	if(SocketManger::serverListen == SOCKET_ERROR) return ERROR_SOCKET_INIT;
	sockAddr.sin_family = AF_INET;
	sockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_IP);
	sockAddr.sin_port = htons(PORT);
	return SUCCESS_INIT;
}

UINT8 SocketManger::Bind() {
	//监听器初始化
	int ret = bind(SocketManger::serverListen,(SOCKADDR *)&sockAddr,sizeof(sockAddr));
	if(ret == SOCKET_ERROR) return ERROR_BIND_SOCKET;
	if(listen(serverListen,10) == SOCKET_ERROR) return ERROR_BIND_LISTEN;
	return SUCCESS_BIND;
}
void SocketManger::Start() {
	//对象函数不能直接被线程调用到，要用到绑定器
	std::thread t(std::bind(&SocketManger::Receive));
	t.detach();//主线程和子线程分开执行

}
void SocketManger::Receive() {
	FD_SET fd_read;//统一管理我们的socket，都是socket对象
	FD_ZERO(&fd_read);//初始化为空集合,这里存放监听套接字+客户端套接字
	FD_SET(SocketManger::serverListen,&fd_read);//往集合里面放监听套接字，serverListen
	SocketManger::serverLoop = true;
	while(SocketManger::serverLoop) {
		FD_SET temp = fd_read;//创建新集合
		const timeval tv = {1,0};//超时阻塞函数跳过
		int ret = select(NULL,&temp,NULL,NULL,&tv);//阻塞函数，自动识别有无网络事件，只保留有网络事件的元素
		if(ret == 0) {//如果接收超时，我们继续
			Sleep(1);
			continue;
		}
		//证明有网络事件发生
		for (int i = 0; i < temp.fd_count; i++) {
			SOCKET& socket = temp.fd_array[i];
			if(socket == SocketManger::serverListen) {//如果服务器有网络事件，证明有客户端在连接服务器
				sockaddr_in clientAddr;//客户端的socket
				int nlen = sizeof(sockaddr_in);
				//阻塞函数，接收客户的连接，上述阻塞已经在select完成，所以这里将会直接进入
				SOCKET client = accept(serverListen,(SOCKADDR *)&clientAddr,&nlen);
				if(client == SOCKET_ERROR){
					printf("一个客户端接收失败！\n");
				}
				FD_SET(std::move(client),&fd_read);//往集合里面放client
				printf("一个客户端建立连接\n");
			} else {//如果客户端套接字有网络事件，证明客户端在发送数据，服务器接收数据
				char buffer[BUFFER_MAX] = { 0 };//初始化一个buffer数组，数组值为0，大小为1024
				int ret = recv(socket,buffer,1024,0);//接收客户端的数据
				if(ret > 0) {
					//处理客户端数据
				} else {
					//断开连接，移除元数据中的当前元素
					FD_CLR(socket,&fd_read);
				}

			}
		}

	}
} 

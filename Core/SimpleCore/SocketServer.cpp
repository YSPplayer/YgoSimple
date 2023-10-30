#include "SocketServer.h"

using namespace Core::Socket;
//初始化服务器函数
UINT8 SocketManger::Init() {
	//1.初始化网络
	WSADATA data;
	int ret =  WSAStartup(MAKEWORD(2,2),&data);
	//网络初始化失败
	if(ret != 0) return ERROR_NET_INIT;
	//2.初始化socket对象
	this->serverListen = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	//socket初始化失败
	if(serverListen == SOCKET_ERROR) return ERROR_SOCKET_INIT;
	this->sockAddr.sin_family = AF_INET;
	this->sockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_IP);
	this->sockAddr.sin_port = htons(PORT);
	return SUCCESS_INIT;
}


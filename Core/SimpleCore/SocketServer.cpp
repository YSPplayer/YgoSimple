#include "SocketServer.h"

using namespace Core::Socket;
//��ʼ������������
UINT8 SocketManger::Init() {
	//1.��ʼ������
	WSADATA data;
	int ret =  WSAStartup(MAKEWORD(2,2),&data);
	//�����ʼ��ʧ��
	if(ret != 0) return ERROR_NET_INIT;
	//2.��ʼ��socket����
	this->serverListen = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	//socket��ʼ��ʧ��
	if(serverListen == SOCKET_ERROR) return ERROR_SOCKET_INIT;
	this->sockAddr.sin_family = AF_INET;
	this->sockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_IP);
	this->sockAddr.sin_port = htons(PORT);
	return SUCCESS_INIT;
}


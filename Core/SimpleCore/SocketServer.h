#ifndef SOCKETSERVER_H
#define SOCKETSERVER_H
#include <iostream>
#include <winsock2.h>
#pragma comment(lib, "ws2_32.lib")

#define SERVER_IP "192.168.43.78"  
#define PORT 7120//Ĭ�����ж˿�
namespace Core{
	namespace Socket{
class SocketManger{
	private: 
		SOCKET serverListen;//��������ǵķ������������socket
		sockaddr_in sockAddr;//����������Լ��ĵ�ַ
	public:
		SocketManger(){};
		~SocketManger(){};
		UINT8 Init();//��ʼ������
	};
	#define ERROR_NET_INIT 0 //�����ʼ��ʧ��
	#define ERROR_SOCKET_INIT 1 //�׽��ֳ�ʼ��ʧ��
	#define SUCCESS_INIT 12 //�׽��ִ����ɹ�

	}
}



#endif // !SOCKETSERVER_H

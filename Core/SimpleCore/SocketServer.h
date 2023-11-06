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
#define PORT 7120//Ĭ�����ж˿�
namespace Core{
	namespace Socket{
class SocketManger{
	private: 
		static SOCKET serverListen;//��������ǵķ������������socket
		static sockaddr_in sockAddr;//����������Լ��ĵ�ַ
	public:
		static bool serverLoop;//�������߳��Ƿ�ѭ��
		static UINT8 Init();//��ʼ������
		static UINT8 Bind();//���������
		static void Start();//�����첽�߳�,����server
		static void Receive();//ѭ���������ݵ��첽�̺߳���
		static UINT8 Close();//�رշ�������socket

	};
	#define ERROR_NET_INIT 1 //�����ʼ��ʧ��
	#define ERROR_SOCKET_INIT 2 //�׽��ֳ�ʼ��ʧ��
	#define SUCCESS_INIT 3 //�׽��ִ����ɹ�
	#define ERROR_BIND_SOCKET 4 //���׽���ʧ��
	#define ERROR_BIND_LISTEN 5 //����ʧ��
	#define SUCCESS_BIND 6 //�󶨼������ɹ�
    #define ERROR_CLOSE 7 //�������ر�ʧ��
	#define SUCCESS_CLOSE 8 //�������رճɹ�
	#define BUFFER_MAX 1024 //�շ����ݵ�����ֽ���


	}
	
}



#endif // !SOCKETSERVER_H

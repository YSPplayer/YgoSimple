#include "SocketServer.h"

using namespace Core::Socket;
//static��Ա����Ҫ��ʼ��������ʼ��������������������ֱ�Ӱ���д�����ͳ�ʼ����
bool SocketManger::serverLoop = false;
SOCKET SocketManger::serverListen;
sockaddr_in SocketManger::sockAddr;
//��ʼ������������
UINT8 SocketManger::Init() {
	//1.��ʼ������
	WSADATA data;
	int ret =  WSAStartup(MAKEWORD(2,2),&data);
	//�����ʼ��ʧ��
	if(ret != 0) return ERROR_NET_INIT;
	//2.��ʼ��socket����
	SocketManger::serverListen = socket(AF_INET,SOCK_STREAM,IPPROTO_TCP);
	//socket��ʼ��ʧ��
	if(SocketManger::serverListen == SOCKET_ERROR) return ERROR_SOCKET_INIT;
	sockAddr.sin_family = AF_INET;
	sockAddr.sin_addr.S_un.S_addr = inet_addr(SERVER_IP);
	sockAddr.sin_port = htons(PORT);
	return SUCCESS_INIT;
}

UINT8 SocketManger::Bind() {
	//��������ʼ��
	int ret = bind(SocketManger::serverListen,(SOCKADDR *)&sockAddr,sizeof(sockAddr));
	if(ret == SOCKET_ERROR) return ERROR_BIND_SOCKET;
	if(listen(serverListen,10) == SOCKET_ERROR) return ERROR_BIND_LISTEN;
	return SUCCESS_BIND;
}
void SocketManger::Start() {
	//����������ֱ�ӱ��̵߳��õ���Ҫ�õ�����
	std::thread t(std::bind(&SocketManger::Receive));
	t.detach();//���̺߳����̷ֿ߳�ִ��

}
void SocketManger::Receive() {
	FD_SET fd_read;//ͳһ�������ǵ�socket������socket����
	FD_ZERO(&fd_read);//��ʼ��Ϊ�ռ���,�����ż����׽���+�ͻ����׽���
	FD_SET(SocketManger::serverListen,&fd_read);//����������ż����׽��֣�serverListen
	SocketManger::serverLoop = true;
	while(SocketManger::serverLoop) {
		FD_SET temp = fd_read;//�����¼���
		const timeval tv = {1,0};//��ʱ������������
		int ret = select(NULL,&temp,NULL,NULL,&tv);//�����������Զ�ʶ�����������¼���ֻ�����������¼���Ԫ��
		if(ret == 0) {//������ճ�ʱ�����Ǽ���
			Sleep(1);
			continue;
		}
		//֤���������¼�����
		for (int i = 0; i < temp.fd_count; i++) {
			SOCKET& socket = temp.fd_array[i];
			if(socket == SocketManger::serverListen) {//����������������¼���֤���пͻ��������ӷ�����
				sockaddr_in clientAddr;//�ͻ��˵�socket
				int nlen = sizeof(sockaddr_in);
				//�������������տͻ������ӣ����������Ѿ���select��ɣ��������ｫ��ֱ�ӽ���
				SOCKET client = accept(serverListen,(SOCKADDR *)&clientAddr,&nlen);
				if(client == SOCKET_ERROR){
					printf("һ���ͻ��˽���ʧ�ܣ�\n");
				}
				FD_SET(std::move(client),&fd_read);//�����������client
				printf("һ���ͻ��˽�������\n");
			} else {//����ͻ����׽����������¼���֤���ͻ����ڷ������ݣ���������������
				char buffer[BUFFER_MAX] = { 0 };//��ʼ��һ��buffer���飬����ֵΪ0����СΪ1024
				int ret = recv(socket,buffer,1024,0);//���տͻ��˵�����
				if(ret > 0) {
					//����ͻ�������
				} else {
					//�Ͽ����ӣ��Ƴ�Ԫ�����еĵ�ǰԪ��
					FD_CLR(socket,&fd_read);
				}

			}
		}

	}
} 

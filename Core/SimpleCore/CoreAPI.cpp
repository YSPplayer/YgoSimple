#include "CoreAPI.h"
using namespace Core::Socket;
 UINT8 Core::CreateSocketServer() {
	//�������ǵĳ�ʼ���������ӿں���
	SocketManger* socketManger = new SocketManger();
	return socketManger->Init();
}
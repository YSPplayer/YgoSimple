#include "CoreAPI.h"
using namespace Core::Socket;
 UINT8 Core::CreateSocketServer() {
	//�������ǵĳ�ʼ���������ӿں���
	return SocketManger::Init();
 }
 UINT8 Core::BindSocketServer() {
	 //�󶨼�����
	 return SocketManger::Bind();
 }
 void Core::StartServer() {
	 //��ʼ�߳�
	 SocketManger::Start();
 }
 void Core::CloseServer() {
	 SocketManger::serverLoop = false;
 }

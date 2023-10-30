#include "CoreAPI.h"
using namespace Core::Socket;
 UINT8 Core::CreateSocketServer() {
	//返回我们的初始化服务器接口函数
	SocketManger* socketManger = new SocketManger();
	return socketManger->Init();
}
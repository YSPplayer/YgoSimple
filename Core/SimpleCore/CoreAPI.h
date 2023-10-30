#ifndef COREAPI_H
#define COREAPI_H
#include "SocketServer.h"
namespace Core {
extern "C" {
		//创建我们的服务器端口
		__declspec(dllexport) UINT8 CreateSocketServer();
	}
}
#endif // DEBUG
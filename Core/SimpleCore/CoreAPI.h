#ifndef COREAPI_H
#define COREAPI_H
#include "SocketServer.h"
#define COREAPI __declspec(dllexport)
namespace Core {
	extern "C"{
		//创建我们的服务器端口
		COREAPI UINT8 CreateSocketServer();
		//绑定服务器端口
		COREAPI UINT8 BindSocketServer();
		//开启异步线程服务器
		COREAPI void StartServer();
		//停止异步线程服务器
		COREAPI UINT8 CloseServer();
	}
}

#endif // DEBUG
#ifndef COREAPI_H
#define COREAPI_H
#include "SocketServer.h"
namespace Core {
extern "C" {
		//�������ǵķ������˿�
		__declspec(dllexport) UINT8 CreateSocketServer();
	}
}
#endif // DEBUG
#ifndef COREAPI_H
#define COREAPI_H
#include "SocketServer.h"
#define COREAPI __declspec(dllexport)
namespace Core {
	extern "C"{
		//�������ǵķ������˿�
		COREAPI UINT8 CreateSocketServer();
		//�󶨷������˿�
		COREAPI UINT8 BindSocketServer();
		//�����첽�̷߳�����
		COREAPI void StartServer();
		//ֹͣ�첽�̷߳�����
		COREAPI void CloseServer();
	}
}

#endif // DEBUG
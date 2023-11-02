#include "CoreAPI.h"
using namespace Core::Socket;
 UINT8 Core::CreateSocketServer() {
	//返回我们的初始化服务器接口函数
	return SocketManger::Init();
 }
 UINT8 Core::BindSocketServer() {
	 //绑定监听器
	 return SocketManger::Bind();
 }
 void Core::StartServer() {
	 //开始线程
	 SocketManger::Start();
 }
 void Core::CloseServer() {
	 SocketManger::serverLoop = false;
 }

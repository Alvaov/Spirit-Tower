#pragma once
#include <WS2tcpip.h>
#include <string>
#include <iostream>
#pragma comment(lib, "ws2_32.lib")

class Tcplistener;
//Callback to data received
typedef void(*MessageRecievedHandler)(Tcplistener* listener, int sockid, std::string msg);
class Tcplistener{
public:
	Tcplistener(int _port, std::string _ip, MessageRecievedHandler handler);
	~Tcplistener();
	void Send(int clientSocket, std::string msg);
	bool Init();
	void Run();
	void cleanup();
private:
	std::string my_Ip_Adrr;
	int my_Port;
	MessageRecievedHandler msg_Rec;
	SOCKET create_socket();
	SOCKET wait_For_Socket(SOCKET listening);
};


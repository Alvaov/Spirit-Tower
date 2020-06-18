#include "Tcplistener.h"
Tcplistener::Tcplistener(int _port, std::string _ip, MessageRecievedHandler handler)
: my_Ip_Adrr(_ip), my_Port(_port), msg_Rec(handler){

};
Tcplistener::~Tcplistener() {
	cleanup();
};
void Tcplistener::Send(int clientSocket, std::string msg) {
	send(clientSocket, msg.c_str(), msg.size()+1, 0);
};
bool Tcplistener::Init(){
	WSAData data;
	WORD ver = MAKEWORD(2, 2);
	int wsInit = WSAStartup(ver, &data);
	if (wsInit == 0) {
		return true;
	}else {
		std::cerr << "could not start socket\n";
		return false;
	}
};
void Tcplistener::Run(){
	char buff[49192];
	while (true) {
		SOCKET listening = create_socket();
		if (listening == INVALID_SOCKET) {
			break;
		}SOCKET client = wait_For_Socket(listening);
		if (client != INVALID_SOCKET) {
			closesocket(listening);
			int bytesRecieved = recv(client,buff,49192,0);
			do {
				ZeroMemory(buff, 49192);
				int bytes_Rec = recv(client, buff, 49192, 0);
				if (bytes_Rec > 0) {
					if (msg_Rec != NULL) {
						msg_Rec(this, client, std::string(buff, 0, bytes_Rec));
					}
				}
			} while (bytesRecieved > 0);
		}closesocket(client);
	}

};
void Tcplistener::cleanup(){
	WSACleanup();
};
SOCKET Tcplistener::create_socket() {
	SOCKET listening = socket(AF_INET, SOCK_STREAM, 0);
	if (listening != INVALID_SOCKET){
		sockaddr_in hint;
		hint.sin_family = AF_INET;
		hint.sin_port = htons(my_Port);
		inet_pton(AF_INET, my_Ip_Adrr.c_str(), &hint.sin_addr);

		int bindOk = bind(listening, (sockaddr*)&hint, sizeof(hint));
		if (bindOk != SOCKET_ERROR) {
			int listenOk = listen(listening, SOMAXCONN);
			if (listenOk == SOCKET_ERROR) {
				std::cerr << "could not listen\n";
				return -1;
			}
		}else{
			std::cerr << "could not bind\n";
			return -1;
		}
	}
	return listening;
};
SOCKET Tcplistener::wait_For_Socket(SOCKET listening) {
	SOCKET client = accept(listening, NULL, NULL);
	return client;
};
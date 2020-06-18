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
	SOCKET listening = create_socket();
	if (listening == INVALID_SOCKET) {
		return;
	}	
	fd_set master;
	FD_ZERO(&master);
	FD_SET(listening, &master);
		
	while (true) {
		try {
			fd_set copy = master;
			int socketCount = select(0, &copy, NULL, NULL, NULL);
			for (int i = 0; i < socketCount; i++) {
				SOCKET sock = copy.fd_array[i];
				if (sock == listening && socketCount <= 2) {//el <=2 puede generar bugs
					SOCKET client = wait_For_Socket(listening);
					FD_SET(client, &master);
				}
				else {
					ZeroMemory(buff, 49192);
					int bytes_Rec = recv(sock, buff, 49192, 0);
					if (bytes_Rec < 0) {
						closesocket(sock);
						FD_CLR(sock, &master);
					}
					else {
						msg_Rec(this, sock, std::string(buff, 0, bytes_Rec));
					}
				}
			}
		}catch (...) {
			std::cerr << "Hubo un error en el servidor\n";
			break;
		}
	}
	closesocket(listening);
	if (master.fd_array[1] != INVALID_SOCKET) {
		closesocket(master.fd_array[1]);
	}FD_CLR(master.fd_array[0],&master);	
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
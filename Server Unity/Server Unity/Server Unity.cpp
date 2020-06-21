// Server Unity.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Tcplistener.h"
int playerPos[2];
char map[60][60];//cambiar segun el mapa
void fill_map(int level) {
    switch (level) {
    default:
        for (int l = 0; l < 60 ; l++) {
            for (int k = 0; k < 60 ; k++) {
                map[l][k] = 'w';
            }
        }
        break;
    }

}
void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg);
int main(){
    fill_map(0);
    Tcplistener server(54100, "127.0.0.1", Listener_MesssageRec);
    if (server.Init()) {
        server.Run();
    }
}

void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg) {
    std::cout <<"Raw Data: "<< msg << std::endl;
    std::string msg_arr[3];
    int pos_in_msg = 0;
    for (int arr_pos = 0; arr_pos < 3; arr_pos++) {
        for (; msg[pos_in_msg] != ':'; pos_in_msg++) {
            msg_arr[arr_pos] += msg[pos_in_msg];
        }pos_in_msg++;
    }
    if (msg_arr[0] == "Player") {
        if (msg_arr[1] == "Position") {
            std::string player_pos_x;
            std::string player_pos_y;
            int i = 0;
            for (; msg_arr[2][i] != ','; i++) {
                player_pos_x += msg_arr[2][i];
            }player_pos_y = msg_arr[2].substr(i+1, msg_arr[2].size());
            map[playerPos[0]][playerPos[1]] = 'w';
            playerPos[0] = std::stoi(player_pos_x);
            playerPos[1] = std::stoi(player_pos_y);
            map[playerPos[0]][playerPos[1]] = 'c';
        }
    }
    listener->Send(client, msg);
};
// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file

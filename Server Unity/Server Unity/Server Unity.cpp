// Server Unity.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Tcplistener.h"
#include "Pathfinding_A.h"
#include "Linked_list.h"
#include "Espectro.h"
int playerPos[2];
lista<Espectro> espectros;

void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg);
int main(){
    //Path_Astar prueba = Path_Astar();
    //prueba.CreateMap();
    int playerT[2] = { 0,2 };
    int playerT2[2] = { 40,32 };
    //std::cout << prueba.print_route(playerT,playerT2) << "\n";
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
    if (msg_arr[0] == "0Player") {
        if (msg_arr[1] == "Position") {
            try {
                std::string player_pos_x;
                std::string player_pos_y;
                int i = 0;
                for (; msg_arr[2][i] != ','; i++) {
                    player_pos_x += msg_arr[2][i];
                }player_pos_y = msg_arr[2].substr(i + 1, msg_arr[2].size());
                playerPos[0] = std::stoi(player_pos_x);
                playerPos[1] = std::stoi(player_pos_y);
            }
            catch (...) {
                std::cerr << "jaja se cayo\n";
            }
        }
    }
    else if (msg_arr[0].substr(1, msg_arr[0].size()) == "Espectro") {
        
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

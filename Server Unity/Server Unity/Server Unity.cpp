// Server Unity.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Tcplistener.h"
#include "Pathfinding_A.h"
#include "Linked_list.h"
#include "Espectro.h"
int playerPos[2];
lista<Espectro*>* espectros;
node_map* mapaActual;
Path_Astar escenario;
node_map* mapa_backtracking;
void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg);
int main(){
    escenario = Path_Astar();
    mapaActual = escenario.CreateMap();
    espectros = new lista<Espectro*>();
    mapa_backtracking = backtraking().CreateMap();
    Tcplistener server(54100, "127.0.0.1", Listener_MesssageRec);
    if (server.Init()) {
        server.Run();
    }
}
void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg) {
    std::cout << msg << std::endl;
    std::string msg_arr[4];
    int pos_in_msg = 0;
    try {
        for (int arr_pos = 0; arr_pos < 4; arr_pos++) {
            for (; msg[pos_in_msg] != ':'; pos_in_msg++) {
                msg_arr[arr_pos] += msg[pos_in_msg];
            }pos_in_msg++;
        }
    }
    catch(...){
        std::cerr << "Sintaxis incorrecta\n";
        return;
    }
    if (msg_arr[1] == "Player") {
        if (msg_arr[2] == "Position") {
            try {
                std::string player_pos_x;
                std::string player_pos_y;
                int i = 0;
                for (; msg_arr[3][i] != ','; i++) {
                    player_pos_x += msg_arr[3][i];
                }player_pos_y = msg_arr[3].substr(i + 1, msg_arr[3].size());
                playerPos[0] = std::stoi(player_pos_x);
                playerPos[1] = std::stoi(player_pos_y);
            }
            catch (...) {
                std::cerr << "jaja se cayo\n";
            }
        }
    }
    else if (msg_arr[1] == "Spectrum") {
        if (msg_arr[2] == "Detected") {
            try {
                int enemyPos[2];
                std::string enemy_pos_x;
                std::string enemy_pos_y;
                int i = 0;
                for (; msg_arr[3][i] != ','; i++) {
                    enemy_pos_x += msg_arr[3][i];
                }enemy_pos_y = msg_arr[3].substr(i + 1, msg_arr[3].size());
                enemyPos[0] = std::stoi(enemy_pos_x);
                enemyPos[1] = std::stoi(enemy_pos_y);
                std::string path = escenario.send_route(msg_arr[0], playerPos, enemyPos);
                listener->Send(client, path);
            }
            catch(...){
                std::cerr << "A* no se logro calcular\n";
            }
        }else if (msg_arr[2] == "New") {
            try {
                int enemyPos[2];
                std::string enemy_pos_x;
                std::string enemy_pos_y;
                int enemy_id;
                int i = 0;
                for (; msg_arr[3][i] != ','; i++) {
                    enemy_pos_x += msg_arr[3][i];
                }
                enemy_pos_y += msg_arr[3].substr(i + 1, msg_arr[3].size());;
                enemy_id = std::stoi(msg_arr[0]);
                enemyPos[0] = std::stoi(enemy_pos_x);
                enemyPos[1] = std::stoi(enemy_pos_y);
                Espectro* espectro = new Espectro(enemyPos[0], enemyPos[1],enemy_id);
                espectros->insert(espectro);
                listener->Send(client,msg_arr[0]+":Spectrum:Created:");
            }
            catch (...) {
                std::cerr << "no se logro crear el enemigo\n";
            }
        }else if (msg_arr[2] == "BackTracking"){
            int enemyPos[2];
            std::string enemy_pos_x;
            std::string enemy_pos_y;
            int i = 0;
            for (; msg_arr[3][i] != ','; i++) {
                enemy_pos_x += msg_arr[3][i];
            }enemy_pos_y = msg_arr[3].substr(i + 1, msg_arr[3].size());
            enemyPos[0] = std::stoi(enemy_pos_x);
            enemyPos[1] = std::stoi(enemy_pos_y);
            backtraking trackback = backtraking(mapa_backtracking);
            std::string path = trackback.send_route(msg_arr[0], enemyPos, playerPos);
            listener->Send(client, path);
        }
    }
    else if (msg_arr[1] == "Grid") {
        if (msg_arr[2] == "Obstacle") {
            std::string pos_x;
            std::string pos_y;
            int i = 0;
            for (; msg_arr[3][i] != ','; i++) {
                pos_x += msg_arr[3][i];
            }
            pos_y = msg_arr[3].substr(i + 1, msg_arr[3].size());
            int x = std::stoi(pos_x);
            int y = std::stoi(pos_y);
            mapaActual[(y * escenario.nMapHeight) + (x + (y / escenario.nMapHeight))].bObstacle = true;
        }
    }

    else if (msg_arr[1] == "Room") {

    }

    else if (msg_arr[1] == "Health") {
        try {
            if (msg_arr[1][2] == 0) {

                //Por hacer: funcion de gameover 
                std::cout << "Salud ha alcanzado 0, terminando juego :(";
            }
        }
        catch (...) {
            std::cerr << "Se trato de hacer un numero de un string no valido o array values ot of bounds\n";
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

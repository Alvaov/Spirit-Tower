// Server Unity.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Tcplistener.h"
#include "Pathfinding_A.h"
#include "Linked_list.h"
#include "Espectro.h"
#include <random>
#include "Enemy_Genetics.h"

int playerPos[2];
lista<Espectro*>* espectros;
node_map* mapaActual;
Path_Astar escenario;
node_map* mapa_backtracking;
int lvl = 1;

//function pointer is declared
void Listener_MesssageRec(Tcplistener* listener, int client, std::string msg);

//funcion para limpiar el recibir de mensajes
int* get_position(std::string pos_in_string) {
    std::string pos_x;
    std::string pos_y;
    int i = 0;
    for (; pos_in_string[i] != ','; i++) {
        pos_x += pos_in_string[i];
    }pos_y = pos_in_string.substr(i + 1, pos_in_string.size());
    int* pos = new int[2];
    pos[0] = std::stoi(pos_x);
    pos[1] = std::stoi(pos_y);
    return pos;
}

int main(){
    escenario = Path_Astar();
    mapaActual = escenario.CreateMap();
    espectros = new lista<Espectro*>();
    mapa_backtracking = backtraking().CreateMap();
    Enemy_Genetics* enemy_genetics = new Enemy_Genetics();
    enemy_genetics->work();
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
                int* pos = get_position(msg_arr[3]);
                playerPos[0] = pos[0];
                playerPos[1] = pos[1];
                delete pos;
            }
            catch (...) {
                std::cerr << "jaja se cayo\n";
            }
        }
    }
    else if (msg_arr[1] == "Spectrum") {
        if (msg_arr[2] == "Detected") {
            try {
                int* enemyPos = get_position(msg_arr[3]);
                std::string path = escenario.send_route(msg_arr[0], playerPos, enemyPos);
                listener->Send(client, path);
                delete enemyPos;
            }
            catch(...){
                std::cerr << "A* no se logro calcular\n";
            }
        }else if (msg_arr[2] == "New") {
            try {
                int* enemyPos = get_position(msg_arr[3]);
                int enemy_id = std::stoi(msg_arr[0]);
                Espectro* espectro = new Espectro(enemyPos[0], enemyPos[1], enemy_id, lvl);
                espectros->insert(espectro);
                listener->Send(client,msg_arr[0]+":Spectrum:Created:");
                delete enemyPos;
            }
            catch (...) {
                std::cerr << "no se logro crear el enemigo\n";
            }
        }else if (msg_arr[2] == "Backtracking"){
            int* enemyPos = get_position(msg_arr[3]);
            int id = std::stoi(msg_arr[0]);
            backtraking trackback = backtraking(mapa_backtracking);
            Espectro* espectro = nullptr;
            for (int j = 0; j < espectros->get_object_counter(); j++) {
                if (espectros->get_data_by_pos(j)->getId() == id) {
                    espectro = espectros->get_data_by_pos(j);
                    break;
                }
            }
            std::string target = espectro->getPath(0);
            int* intTarget = get_position(target);
            std::string path = trackback.send_route(id, enemyPos, get_position(espectro->getPath(0)));
            listener->Send(client, msg_arr[0]+":Spectrum:Backtracking:"+path);
            delete enemyPos;
        }
    }else if (msg_arr[1] == "Grid") {
        if (msg_arr[2] == "Obstacle") {
            int* pos = get_position(msg_arr[3]);
            mapaActual[(pos[1] * escenario.nMapHeight) + (pos[0] + (pos[1] / escenario.nMapHeight))].bObstacle = true;
            delete pos;
        }
    }else if (msg_arr[1] == "Chuchu") {
        if (msg_arr[2] == "New") {

        }
        else if (msg_arr[2] == "path") {
            int* enemyPos = get_position(msg_arr[3]);
            listener->Send(client, bresenham(enemyPos[0], enemyPos[1],playerPos[0], playerPos[1]));
            delete enemyPos;
        }
    }
    else if (msg_arr[1] == "Safe") {
        std::cout << "Player is in safe zone" << std::endl;
    }
    else if (msg_arr[1] == "Rat") {
        if (msg_arr[2] == "Move") {
            int* rat = get_position(msg_arr[3]);
            std::default_random_engine generator;
            std::uniform_int_distribution<int> distribution(-1, 1);
            int movement[2] = {distribution(generator), distribution(generator)};
            
            node_map rat_to_move_pos = 
                mapaActual[
                    ((rat[1] + movement[1]) * escenario.nMapHeight) + 
                    ((rat[0] + movement[0]) + 
                    ((rat[1] + movement[1]) / escenario.nMapHeight))
                ];
            if (!rat_to_move_pos.bObstacle) {
                std::string msg_to_send = msg_arr[0] + ":Rat:Move:";
                msg_to_send += (rat[0] + movement[0]);
                msg_to_send += ",";
                msg_to_send += (rat[1] + movement[1]);
                msg_to_send += ":";
            }
            delete rat;
        }
    }
    else if (msg_arr[1] == "Health") {

    }

    else if (msg_arr[1] == "Coins") {
        try {
            if (msg_arr[1][2] == 0) {
            }
        }
        catch (...) {
            std::cerr << "Se trato de hacer un numero de un string no valido o array values ot of bounds\n";
        }
    }

    else if (msg_arr[1] == "Treasures") {
        try {
            if (msg_arr[1][2] == 0) {
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

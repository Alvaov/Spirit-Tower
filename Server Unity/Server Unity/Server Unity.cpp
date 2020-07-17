// Server Unity.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "Tcplistener.h"
#include "Pathfinding_A.h"
#include "Linked_list.h"
#include "Espectro.h"
#include "SpectralEye.h"
#include "FinalBoss.h"
#include <random>
#include "Enemy_Genetics.h"
#include <chrono>
#include <stdlib.h>
int playerPos[2];
FinalBoss* boss;
lista<Espectro*>* espectros;
lista<SpectralEye*>* spectralEyes;
node_map* mapaActual;
Path_Astar escenario;
node_map* mapa_backtracking;
Enemy_Genetics* enemy_genetics;
int lvl = 0;

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
    spectralEyes = new lista<SpectralEye*>();
    mapa_backtracking = backtraking().CreateMap();
    enemy_genetics = new Enemy_Genetics();
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
        if (msg_arr[2] == "Health") {
            if (msg_arr[3] == "0") {
                std::cout << "Player has died, game over :c";
                //listener->Send(client, "0:Player:Dead:");
            }
        }
    }
    else if (msg_arr[1] == "Spectrum") {
        if (msg_arr[2] == "Detected") {
            try {
                int* enemyPos = get_position(msg_arr[3]);
                int id = std::stoi(msg_arr[0]);
                Espectro* espectro = nullptr;
                for (int j = 0; j < espectros->get_object_counter(); j++) {
                    if (espectros->get_data_by_pos(j)->getId() == id) {
                        espectro = espectros->get_data_by_pos(j);
                        break;
                    }
                }
                espectro->set_position(enemyPos[0], enemyPos[1]);
                std::string path = escenario.send_route(msg_arr[0], playerPos, enemyPos);
                listener->Send(client, path);
                delete enemyPos;
            }
            catch(...){
                std::cerr << "A* no se logro calcular\n";
            }
        }else if (msg_arr[2] == "New") {
            try {

                std::string pos;
                std::string type;
                int i = 0;
                for (; msg_arr[3][i] != ';'; i++) {
                    pos += msg_arr[3][i];
                }type = msg_arr[3].substr(i + 1, msg_arr[3].size());
                std::string* result = new std::string[2];

                int* enemyPos = get_position(pos);
                int enemy_id = std::stoi(msg_arr[0]);

                lista<Person*> list = enemy_genetics->getList();
                Person* datos_espector = list.get_data_by_pos(enemy_id);
                Espectro* espectro = new Espectro(
                    datos_espector->get_health(),
                    enemyPos[0], 
                    enemyPos[1], 
                    datos_espector->get_speed(), 
                    5, 
                    datos_espector->get_vision_range(), 
                    enemy_id, 
                    lvl, 
                    type
                );
                espectros->insert(espectro);
                std::string patrolPath = "";
                for (int j = 0; j < espectro->get_path()->get_object_counter(); j++) {
                    patrolPath += espectro->get_path()->get_data_by_pos(j);
                    patrolPath += ";";
                }
                std::string DNA = std::to_string(datos_espector->get_follow_speed()) + 
                    ","+
                    std::to_string(datos_espector->get_speed()) + 
                    "," +
                    std::to_string(datos_espector->get_vision_range());
                listener->Send(client,msg_arr[0]+":Spectrum:Created:"+patrolPath+"*"+DNA);
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
            listener->Send(client, msg_arr[0]+":Spectrum:Backtracking:"+path+target);
            delete enemyPos;
        }else if (msg_arr[2] == "Attack") { //Producir dano
            std::cout << "Spectrum hit player. Player is gonna die";
            listener->Send(client, "0:Player:Damage:5");
        }
        else if (msg_arr[2] == "Damage") { //Recibir dano
            listener->Send(client, msg_arr[0] + ":Spectrum:Dead:");
        }
        else if (msg_arr[2] == "Teleport") {
            int closestPos[2] = {0,0};
            int actualDistance = INT_MAX;
            for (int i = 0; i < espectros->get_object_counter(); i++) {
                Espectro* espectro = espectros->get_data_by_pos(i);
                int startX = espectro->get_x();
                int startY = espectro->get_y();
                int distance = sqrtf((startX - playerPos[0])* (startX - playerPos[0]) + (startY - playerPos[1]) * (startY - playerPos[1]));
                if (distance < actualDistance) {
                    actualDistance = distance;
                    closestPos[0] = startX;
                    closestPos[1] = startY;
                }
            }
            for (int j = 0; j < spectralEyes->get_object_counter(); j++) {
                SpectralEye* ojoEspectral = spectralEyes->get_data_by_pos(j);
                int startX = ojoEspectral->pos_x;
                int startY = ojoEspectral->pos_y;
                int distance = sqrtf((startX - playerPos[0]) * (startX - playerPos[0]) + (startY - playerPos[1]) * (startY - playerPos[1]));
                if (distance < actualDistance) {
                    actualDistance = distance;
                    closestPos[0] = startX;
                    closestPos[1] = startY;
                }
            }

            listener->Send(client, msg_arr[0] + ":Spectrum:Teleport:" + std::to_string(closestPos[0]) + "," + std::to_string(closestPos[1]) + ":");
        }
    }
    else if (msg_arr[1] == "Grid") {
        if (msg_arr[2] == "Obstacle") {
            int* pos = get_position(msg_arr[3]);
            mapaActual[(pos[1] * escenario.nMapHeight) + (pos[0] + (pos[1] / escenario.nMapHeight))].bObstacle = true;
            delete pos;
        }else if (msg_arr[2] == "New"){
            if (!espectros->isEmpty()) {
                for (int p = 0; p < espectros->get_object_counter(); p++) {
                    delete espectros->get_data_by_pos(p);
                }
                espectros->delete_list();
            }
            lvl++;
            mapaActual = escenario.CreateMap();
            for (int p = 0; p < 50; p++) {
                enemy_genetics->work();
            }
            std::cout << "se crea el mapa nuevamente" << "\n";
        }
    }
    else if (msg_arr[1] == "Chuchu") {
        if (msg_arr[2] == "New") {
            listener->Send(client, msg_arr[0] + ":Chuchu:Created:");
        }
        else if (msg_arr[2] == "Move") {
            int* enemyPos = get_position(msg_arr[3]);
            listener->Send(client, 
                msg_arr[0]+
                ":Chuchu:Move:"+
                bresenham(enemyPos[0], enemyPos[1],playerPos[0], playerPos[1], mapaActual)+
                ":");
            delete enemyPos;
        }
        else if (msg_arr[2] == "Attack") {
            listener->Send(client, "0:Player:Damage:1");
        }
        else if (msg_arr[2] == "Damage") {
            listener->Send(client, msg_arr[0] + ":Chuchu:Dead:");
        }
    }
    else if (msg_arr[1] == "Boss") {
        if (msg_arr[2] == "New") {

            boss = new FinalBoss();
            boss->setRoute();
            std::string startPath = "";
            for (int j = 0; j < boss->get_path()->get_object_counter(); j++) {
                startPath += boss->get_path()->get_data_by_pos(j);
                startPath += ";";
            }
            listener->Send(client, "0:Boss:Created:"+startPath+"*"+"9");
        }
    }
    else if (msg_arr[1] == "Eye") {
        if (msg_arr[2] == "New") {

            int* enemyPos = get_position(msg_arr[3]);
            int id = std::stoi(msg_arr[0]);
            SpectralEye* ojoEspectral = new SpectralEye(id, enemyPos[0],enemyPos[1]);
            spectralEyes->insert(ojoEspectral);
            listener->Send(client, msg_arr[0] + ":Eye:Created:");
            delete enemyPos;

        }
    }
    else if (msg_arr[1] == "Safe") {
        std::cout << "Player is in safe zone" << std::endl;
    }
    else if (msg_arr[1] == "Rat") {
        if (msg_arr[2] == "Move") {
            int* rat = get_position(msg_arr[3]);
            int rand_num();
            int rat_x = rand_num();
            int rat_y = rand_num();
            int movement[2] = {rat_x, rat_y};
            node_map rat_to_move_pos = 
                mapaActual[
                    ((rat[1] + movement[1]) * escenario.nMapHeight) + 
                    ((rat[0] + movement[0]) + 
                    ((rat[1] + movement[1]) / escenario.nMapHeight))
                ];
            if (!rat_to_move_pos.bObstacle) {
                std::string msg_to_send = msg_arr[0] + ":Rat:Move:";
                msg_to_send += std::to_string(rat[0] + movement[0]);
                msg_to_send += ",";
                msg_to_send += std::to_string(rat[1] + movement[1]);
                msg_to_send += ":";
                listener->Send(client, msg_to_send);
            }
            delete rat;
        }if (msg_arr[2] == "New") {
            listener->Send(client,msg_arr[0]+":Rat:Created:");
        }
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
};

int rand_num() {
    srand(time(nullptr));
    int num = rand() % (3);
    if (num == 0) {
        return 0;
    }else if (num == 1) {
        return 1;
    }else if (num == 2) {
        return -1;
    }
}
// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file

#pragma once

#include <iostream>
#include "Enemy.h"

class Espectro : public Enemy {

public:
	Espectro(){};
	Espectro(int x, int y, int _id, int lvl) : Enemy(x, y) {
		id = _id;
		pos_x = x;
		pos_y = y;
		path = lista<std::string>();
		//se asignan las rutas de patrullaje segun el nivel y el espectro
		switch (id){
			case 0:
				switch (lvl){
					case 1:
						path.insert("39,32"); 
						path.insert("74,32");
						path.insert("74,54");
						path.insert("39,54");
						break;
					default:
						break;
				}
				break;
			case 1:
				switch (lvl) {
				case 1:
					path.insert("81,36");
					path.insert("81,58");
					path.insert("107,58");
					path.insert("107,36");
					break;
				default:
					break;
				}
				break;
			case 2:
				switch (lvl) {
				case 1:
					path.insert("66,64");
					path.insert("89,64");
					path.insert("89,80");
					path.insert("36,80");
					path.insert("36,96");
					path.insert("66,96");
					break;
				default:
					break;
				}
				break;
			default:
				break;
		}
	}
	int getId() {
		return id;
	}
	void set_Id(int _id) {
		id = _id;
	}
	void set_path(std::string _path) {
		path = _path;
	}
	void set_follow_speed(int _speed) {
		follow_speed = _speed;
	}
	lista<std::string> get_path() {
		return path;
	}
	int get_follow_speed() {
		return follow_speed;
	}
	std::string getPath(int contador) {
		if (contador < path.get_object_counter() && contador > -1) {
			std::string ruta = path.get_data_by_pos(contador);
			return ruta;
		}
	}
private:
	lista<std::string> path;
	int follow_speed;
	int id;
	void runaway();
	void backtracking();
	void pathfinding_A();
};
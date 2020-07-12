#pragma once

#include <iostream>
#include "Enemy.h"

class Espectro : public Enemy {
private:
	lista<std::string> path;
	int follow_speed;
	int id;
	void runaway();
	void give_path(int lvl, int x, int y,int id);
public:
	Espectro(){};
	Espectro(int x, int y, int _id, int lvl) : Enemy(x, y) {
		give_path(lvl, x, y, id);
	}
	Espectro(int _health, int x, int y, int _speed, int _dmg, int vision, int _id, int lvl) : Enemy(_health,x, y, _speed, _dmg, vision) {
		give_path(lvl, x, y, id);
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
};
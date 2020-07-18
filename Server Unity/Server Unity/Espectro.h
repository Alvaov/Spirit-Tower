#pragma once

#include <iostream>
#include "Enemy.h"
/**
 * @brief clase que representa al espectro
 */
class Espectro : public Enemy {
private:
	lista<std::string>* path;
	int follow_speed;
	int id;
	/**
	 * @brief funcion que sirve para correr
	 * 
	 */
	void runaway();
	/**
	 * @brief se da la ruta al espectro segun el nivel actual, y su id
	 * 
	 * @param lvl nivel actual
	 * @param x coordeandas del eje x
	 * @param y coordenadas del eje y
	 * @param _id id del espector
	 */
	void give_path(int lvl, int x, int y, int _id) {
		id = _id;
		pos_x = x;
		pos_y = y;
		path = new lista<std::string>();
		//se asignan las rutas de patrullaje segun el nivel y el espectro
		switch (id) {
		case 0:
			switch (lvl) {
			case 1:
				path->insert("39,32");
				path->insert("74,32");
				path->insert("74,54");
				path->insert("39,54");
				break;
			case 2:
				path->insert("72,55");
				path->insert("86,64");
				path->insert("82,79");
				path->insert("73,86");
				path->insert("46,86");
				path->insert("38,80");
				path->insert("34,73");
				path->insert("34,62");
				path->insert("45,46");
				path->insert("47,63");
				path->insert("51,71");
				path->insert("58,75");
				path->insert("71,70");
				path->insert("73,63");
				break;
			case 3:
				path->insert("47,8");
				path->insert("47,28");
				path->insert("36,39");
				path->insert("16,30");
				break;
			case 4:
				path->insert("58,25");
				path->insert("71,28");
				path->insert("85,41");
				path->insert("71,28");
				break;
			default:
				break;
			}
			break;
		case 1:
			switch (lvl) {
			case 1:
				path->insert("81,36");
				path->insert("81,58");
				path->insert("107,58");
				path->insert("107,36");
				break;
			case 2:
				path->insert("54,51");
				path->insert("65,51");
				path->insert("65,57");
				path->insert("54,57");
				break;
			case 3:
				path->insert("54,44");
				path->insert("65,44");
				path->insert("65,75");
				path->insert("54,75");
				break;
			case 4:
				path->insert("80,89");
				path->insert("63,95");
				path->insert("45,92");
				path->insert("63,95");
				break;
			default:
				break;
			}
			break;
		case 2:
			switch (lvl) {
			case 1:
				path->insert("66,64");
				path->insert("89,64");
				path->insert("89,80");
				path->insert("36,80");
				path->insert("36,96");
				path->insert("66,96");
				break;
			case 2:
				path->insert("99,58");
				path->insert("99,24");
				path->insert("21,24");
				path->insert("21,58");
				path->insert("29,58");
				path->insert("29,29");
				path->insert("91,29");
				path->insert("91,58");
				break;
			case 3:
				path->insert("101,95");
				path->insert("83,85");
				path->insert("74,91");
				path->insert("74,112");
				break;
			case 4:
				path->insert("17,75");
				path->insert("25,90");
				path->insert("44,101");
				path->insert("25,90");
				break;
			default:
				break;
			}
			break;
		default:
			break;
		}
	}
public:
	std::string tipo;
	/**
	 * @brief Construct a new Espectro object
	 * 
	 */
	Espectro(){
	};
	/**
	 * @brief Construct a new Espectro object
	 * 
	 * @param x coordenadas del eje x
	 * @param y coordenadas del eje y 
	 * @param _id identificacion del espectro
	 * @param lvl nivel actual
	 */
	Espectro(int x, int y, int _id, int lvl) : Enemy(x, y) {
		give_path(lvl, x, y, id);
	}
	/**
	 * @brief Construct a new Espectro object
	 * 
	 * @param _health vida del espectro
	 * @param x coordenadas en x
	 * @param y coordenadas en y
	 * @param _speed velocidad del espectro
	 * @param _dmg cantidad de corazones que quita del jugador
	 * @param vision rango de vision del enemigo
	 * @param _id identificador del enemigo
	 * @param lvl nivel actual
	 * @param _tipo tipo de enemigo
	 */
	Espectro(int _health, int x, int y, int _speed, int _dmg, int vision, int _id, int lvl, std::string _tipo) : Enemy(_health,x, y, _speed, _dmg, vision) {
		id = _id;
		tipo = _tipo;
		give_path(lvl, x, y, id);
	}
	/**
	 * @brief Get the Id object
	 * 
	 * @return id
	 */
	int getId() {
		return id;
	}
	/**
	 * @brief Set the Id object
	 * 
	 * @param _id identificador
	 */
	void set_Id(int _id) {
		id = _id;
	}
	/**
	 * @brief Set the follow speed object
	 * 
	 * @param _speed velocidad de persecucion
	 */
	void set_follow_speed(int _speed) {
		follow_speed = _speed;
	}
	/**
	 * @brief Get the path object
	 * 
	 * @return lista<std::string>* conisgue la lista de puntos claves para el enemigo 
	 */
	lista<std::string>* get_path() {
		return path;
	}
	/**
	 * @brief Get the follow speed object
	 * 
	 * @return velocidad de persecucion
	 */
	int get_follow_speed() {
		return follow_speed;
	}
	/**
	 * @brief Get the Path object
	 * 
	 * @param contador posicion del path a conseguir
	 * @return std::string conseguir la posicion individual en la posicion establecida
	 */
	std::string getPath(int contador) {
		if (contador < path->get_object_counter() && contador > -1) {
			std::string ruta = path->get_data_by_pos(contador);
			return ruta;
		}
	}
	/**
	 * @brief Set the position
	 * 
	 * @param x cordenadas eje x
	 * @param y cordenadas eje y
	 */
	void set_position(int x, int y) {
		pos_x = x;
		pos_y = y;
	}
	/**
	 * @brief Get the x object
	 * 
	 * @return cordenadas en x
	 */
	int get_x() {
		return pos_x;
	}
	/**
	 * @brief Get the y object
	 * 
	 * @return cordenadas en y
	 */
	int get_y() {
		return pos_y;
	}
};
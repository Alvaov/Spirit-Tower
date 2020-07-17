#pragma once

#include <iostream>
#include "Enemy.h"
#include "Linked_list.h"
/**
 * @brief Clase que representa al jefe final
 */
class FinalBoss
{
private:
	lista<std::string>* path;
	int life = 9;
	int actualPhase = 0;
public:
/**
 * @brief Construct a new Final Boss object
 * 
 */
	FinalBoss();
	/**
	 * @brief Set the Route object
	 * 
	 */
	void setRoute();
	lista<std::string>* get_path();
	/**
	 * @brief Get the Path object
	 * 
	 * @param contador posicion del path a escoger
	 * @return cordenadas en x,y donde moverse 
	 */
	std::string getPath(int contador);
};


#pragma once

#include <iostream>
#include "Enemy.h"
#include "Linked_list.h"

class FinalBoss
{
private:
	lista<std::string>* path;
	int life = 9;
	int actualPhase = 0;
public:

	FinalBoss();
	void setRoute();
	lista<std::string>* get_path();
	std::string getPath(int contador);
};


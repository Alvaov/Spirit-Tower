#pragma once
#include <time.h>
#include <stdlib.h>
#include "Linked_list.h"
#include "Genetic_Algorithm.h"


//Aplica el algoritmo genetico a una poblacion de enemigos
class Enemy_Genetics {

private:
    // Lista de enemigos (Poblacion)
    lista<Person*> Enemies;

public:
    Enemy_Genetics();
    void work();
};


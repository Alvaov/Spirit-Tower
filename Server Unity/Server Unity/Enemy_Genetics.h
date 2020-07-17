#pragma once
#include <time.h>
#include <stdlib.h>
#include "Linked_list.h"
#include "Genetic_Algorithm.h"


//Aplica el algoritmo genetico a una poblacion de enemigos
/**
 * @brief clase que sirve como adaptador entre geneticos y la necesidad del juego
 * @author Yendry Pamela Badilla Gutierrez	
 */
class Enemy_Genetics {

private:
    // Lista de enemigos (Poblacion)
    lista<Person*> Enemies;

public:
    /**
     * @brief Construct a new Enemy_Genetics object
     * 
     */
    Enemy_Genetics();
    /**
     * @brief funcion que se llama para pasar una generacion
     */
    void work();
    /**
     * @brief Get the List object consigue los individuos de la generacion
     * @return lista<Person*> 
     */
    lista<Person*> getList();
};
/**
 * @brief funcion que se encarga de ordenar, sirve como interfaz unica para ordenar la lista.
 * @param list puntero de la lista a ordenar
 * @param low el punto minimo de esa lista
 * @param high el punto maximo de esa lista
 */
void quickSortGenetics(lista<Person*>* list, int low, int high);
/**
 * @brief funcion que se encarga de ordenar los elementos en esta iteracion
 * @param list lista a ordenar
 * @param low punto mas bajo de la sub-lista
 * @param high punto mas alto de la sub-lista
 * @return pivote de la siguiente iteracion
 */
int partitionGenetics(lista<Person*>* list, int low, int high);
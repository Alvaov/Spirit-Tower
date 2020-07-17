#pragma once
#include "Linked_List.h"
#include <string>
struct node_map;
/**
 * @brief algoritmo de brasenham para crear una linea recta
 * 
 * @param x1 cordeanda en x inicial
 * @param y1 cordenada en y inicial
 * @param x2 cordenada en x target
 * @param y2 cordeanda en y target
 * @param mapaActual mapa donde se esta realizando el calculo
 * @return std::string retorna el camino a realizar
 */
std::string bresenham(int x1, int y1, int x2, int y2, node_map* mapaActual);
/**
 * @brief node del mapa
 * 
 */
struct node_map
{
		bool bObstacle = false;			// Is the node an obstruction?
		bool bVisited = false;			// Have we searched this node before?
		float fGlobalGoal;				// Distance to goal so far
		float fLocalGoal;				// Distance to goal if we took the alternative route
		int x;							// Nodes position in 2D space
		int y;
		lista<node_map*> ListNeighbours;	// Connections to neighbours
		node_map* parent;					// Node connecting to this node that offers shortest parent
};
/**
 * @brief calcula el algoritmo A* por una matriz de una dimension
 * @author Javidx9
 */
class Path_Astar
{
public:
	/**
	 * @brief Construct a new Path_Astar object
	 * 
	 */
	Path_Astar();
private:
	node_map* nodes = nullptr;
	
	node_map* nodeStart = nullptr;
	node_map* nodeEnd = nullptr;
public:
	/**
	 * @brief Create a Map object
	 * 
	 * @param size tamaño del mapa a crear 
	 * @return node_map* array del mapa
	 */
	node_map* CreateMap(int size = 120);
	/**
	 * @brief resuelve el camino del enemigo al jugador
	 * @param posPlayer tupla con la posicion del jugador
	 * @param posEnemy tupla con la posicion del enemigo
	 * @return true si existe una ruta
	 * @return false no exite ruta
	 */
	bool Solve_AStar(int posPlayer[2], int posEnemy[2]);
	/**
	 * @brief manda la ruta calculada al servidor. 
	 * 
	 * @param spectrumId id
	 * @param posPlayer posicion del jugador
	 * @param posEnemy posicion del enemigo
	 * @return std::string ruta encontrada
	 */
	std::string send_route(std::string spectrumId, int posPlayer[2], int posEnemy[2]);
	int nMapWidth = 120;
	int nMapHeight = 120;
};
/**
 * @brief clase para realizar el backtracking
 */
class backtraking{
public:
	/**
	 * @brief Create a Map object
	 * 
	 * @param size map size
	 * @return node_map* array del mapa
	 */
	node_map* CreateMap(int size = 120);
	int nMapWidth = 120;
	int nMapHeight = 120;
	/**
	 * @brief Construct a new backtraking object
	 * 
	 * @param mapita mapa a utilizar
	 */
	backtraking(node_map* mapita);
	/**
	 * @brief Construct a new backtraking object
	 * 
	 */
	backtraking();
	/**
	 * @brief encuentra la ruta entre ambos puntos
	 * 
	 * @param posXY posicion inicial
	 * @param end_pos posiciopn final
	 * @param dist distancia
	 * @return true si existe ruta
	 * @return false si no existe la ruta
	 */
	bool find_shortest_path(int posXY, int end_pos, int dist);
	/**
	 * @brief funcion recursiva del backtracking
	 * 
	 * @param posEnemy posicion del enemigo
	 * @param destination destino
	 * @return node_map* mapa por el que se trasversa
	 */
	node_map* backtrack(int posEnemy[2], int destination[2]);
	/**
	 * @brief da la ruta de la posicion
	 * @param spectrumId id del espectro
	 * @param posPlayer posicion del jugador
	 * @param posEnemy posicion del enemigo
	 * @return std::string ruta a enviar
	 */
	std::string send_route(int spectrumId, int posPlayer[2], int posEnemy[2]);
	/**
	 * @brief funcion recursiva del quicksort
	 * 
	 * @param list lista a ordenar
	 * @param low indice mas bajo
	 * @param high indice mas alto
	 * @param target posicion a buscar
	 * @return ruta al punto
	 */
	int partition(lista<node_map*>* list, int low, int high, node_map* target);
	/**
	 * @brief quicksort, ordena segun la distancia entre nodos
	 * 
	 * @param list lista a ordenar
	 * @param low posicion mas baja
	 * @param high posicion mas alta
	 * @param target nodo a llegar
	 */
	void quickSort(lista<node_map*>* list, int low, int high, node_map* target);
private:
	int min_dist;
	/**
	 * @brief verifica que se puede entrar al nodo
	 * 
	 * @param posXY posicion del nodo
	 * @return true se puede entrar
	 * @return false no se puede entrar
	 */
	bool is_safe(int posXY);
	/**
	 * @brief es una posicion valida
	 * 
	 * @param posXY posicion del nodo
	 * @return true es valido
	 * @return false no es valido
	 */
	bool is_valid(int posXY);
	node_map* nodes;
};
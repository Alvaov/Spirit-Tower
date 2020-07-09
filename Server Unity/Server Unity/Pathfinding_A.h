#pragma once
#include "Linked_List.h"
#include <string>
std::string bresenham(int x1, int y1, int x2, int y2);
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
class Path_Astar
{
public:
	Path_Astar();
private:
	node_map* nodes = nullptr;
	
	node_map* nodeStart = nullptr;
	node_map* nodeEnd = nullptr;
public:
	node_map* CreateMap(int size = 120);
	bool Solve_AStar(int posPlayer[2], int posEnemy[2]);
	std::string send_route(std::string spectrumId, int posPlayer[2], int posEnemy[2]);
	int nMapWidth = 120;
	int nMapHeight = 120;
};

class backtraking{
public:
	node_map* CreateMap(int size = 120);
	int nMapWidth = 120;
	int nMapHeight = 120;
	backtraking(node_map* mapita);
	backtraking();
	bool find_shortest_path(int posXY, int end_pos, int dist);
	node_map* backtrack(int posEnemy[2], int destination[2]);
	std::string send_route(int spectrumId, int posPlayer[2], int posEnemy[2]);
	int partition(lista<node_map*>* list, int low, int high, node_map* target);
	void quickSort(lista<node_map*>* list, int low, int high, node_map* target);
private:
	int min_dist;
	bool is_safe(int posXY);
	bool is_valid(int posXY);
	node_map* nodes;
};
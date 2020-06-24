#pragma once
#include "Linked_List.h"
#include <string>
class Path_Astar
{
public:
	Path_Astar();

private:
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

	node_map* nodes = nullptr;
	int nMapWidth = 60;
	int nMapHeight = 60;
	node_map* nodeStart = nullptr;
	node_map* nodeEnd = nullptr;
public:
	bool CreateMap();
	bool Solve_AStar(int posPlayer[2], int posEnemy[2]);
	std::string print_route(int posPlayer[2], int posEnemy[2]);
};
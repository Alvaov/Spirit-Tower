#pragma once
#include "Linked_List.h"
#include <string>
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
	node_map* CreateMap(int size = 20);
	bool Solve_AStar(int posPlayer[2], int posEnemy[2]);
	std::string send_route(std::string spectrumId, int posPlayer[2], int posEnemy[2]);
	int nMapWidth = 20;
	int nMapHeight = 20;
};

class backtraking{
public:
	int nMapWidth = 20;
	int nMapHeight = 20;
	backtraking(node_map* mapita);
	backtraking();
	node_map* nodes;
	bool is_safe(int posXY);
	bool is_valid(int posXY);
	void find_shortest_path(int posXY, int end_pos, int dist);
	node_map* backtrack(int posEnemy[2], int destination[2]);
	std::string send_route(std::string spectrumId, int posPlayer[2], int posEnemy[2]);
private:
	int min_dist;
};
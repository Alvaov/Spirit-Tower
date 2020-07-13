

/*
OneLoneCoder.com - PathFinding A*
"No more getting lost..." - @Javidx9
License
~~~~~~~
Copyright (C) 2018  Javidx9
This program comes with ABSOLUTELY NO WARRANTY.
This is free software, and you are welcome to redistribute it
under certain conditions; See license for details.
Original works located at:
https://www.github.com/onelonecoder
https://www.onelonecoder.com
https://www.youtube.com/javidx9
GNU GPLv3
https://github.com/OneLoneCoder/videos/blob/master/LICENSE
From Javidx9 :)
~~~~~~~~~~~~~~~
Hello! Ultimately I don't care what you use this for. It's intended to be
educational, and perhaps to the oddly minded - a little bit of fun.
Please hack this, change it and use it in any way you see fit. You acknowledge
that I am not responsible for anything bad that happens as a result of
your actions. However this code is protected by GNU GPLv3, see the license in the
github repo. This means you must attribute me if you use it. You can view this
license here: https://github.com/OneLoneCoder/videos/blob/master/LICENSE
Cheers!
Background
~~~~~~~~~~
The A* path finding algorithm is a widely used and powerful shortest path
finding node traversal algorithm. A heuristic is used to bias the algorithm
towards success. This code is probably more interesting than the video. :-/
Author
~~~~~~
Twitter: @javidx9
Blog: www.onelonecoder.com
Video:
~~~~~~
https://youtu.be/icZj67PTFhc
Last Updated: 08/10/2017
*/
#include <iostream>
#include <string>
#include "Pathfinding_A.h"
Path_Astar::Path_Astar() {};
node_map* Path_Astar::CreateMap(int size)
{
	// Create a 2D array of nodes - this is for convenience of rendering and construction
	// and is not required for the algorithm to work - the nodes could be placed anywhere
	// in any space, in multiple dimensions...
	nMapHeight = size;
	nMapHeight = size;
	nodes = new node_map[nMapWidth * nMapHeight];
	for (int x = 0; x < nMapWidth; x++)
		for (int y = 0; y < nMapHeight; y++)
		{
			nodes[y * nMapWidth + x].x = x; // ...because we give each node its own coordinates
			nodes[y * nMapWidth + x].y = y;
			nodes[y * nMapWidth + x].bObstacle = false;
			nodes[y * nMapWidth + x].parent = nullptr;
			nodes[y * nMapWidth + x].bVisited = false;
		}

	// Create connections - in this case nodes are on a regular grid
	for (int x = 0; x < nMapWidth; x++)
		for (int y = 0; y < nMapHeight; y++)
		{
			if (y > 0)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y - 1) * nMapWidth + (x + 0)]);
			if (y < nMapHeight - 1)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 1) * nMapWidth + (x + 0)]);
			if (x > 0)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 0) * nMapWidth + (x - 1)]);
			if (x < nMapWidth - 1)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 0) * nMapWidth + (x + 1)]);

			// We can also connect diagonally
			if (y>0 && x>0)
				nodes[y*nMapWidth + x].ListNeighbours.insert(&nodes[(y - 1) * nMapWidth + (x - 1)]);
			if (y<nMapHeight-1 && x>0)
				nodes[y*nMapWidth + x].ListNeighbours.insert(&nodes[(y + 1) * nMapWidth + (x - 1)]);
			if (y>0 && x<nMapWidth-1)
				nodes[y*nMapWidth + x].ListNeighbours.insert(&nodes[(y - 1) * nMapWidth + (x + 1)]);
			if (y<nMapHeight - 1 && x<nMapWidth-1)
				nodes[y*nMapWidth + x].ListNeighbours.insert(&nodes[(y + 1) * nMapWidth + (x + 1)]);
				
		}

	// Manually positio the start and end markers so they are not nullptr
	nodeStart = &nodes[(nMapHeight / 2) * nMapWidth + 1];
	nodeEnd = &nodes[(nMapHeight / 2) * nMapWidth + nMapWidth - 2];
	return nodes;
}

bool Path_Astar::Solve_AStar(int posPlayer[2], int posEnemy[2])
{	
	//passing values from a 2D array to a 1D array;
	nodeEnd = &nodes[(posEnemy[1] * nMapHeight) + (posEnemy[0] + (posEnemy[1]/nMapHeight))];
	nodeStart = &nodes[(posPlayer[1] * nMapHeight) + (posPlayer[0] + (posPlayer[1] / nMapHeight))];
	// Reset Navigation Graph - default all node states
	for (int x = 0; x < nMapWidth; x++)
		for (int y = 0; y < nMapHeight; y++)
		{
			nodes[y * nMapWidth + x].bVisited = false;
			nodes[y * nMapWidth + x].fGlobalGoal = INFINITY;
			nodes[y * nMapWidth + x].fLocalGoal = INFINITY;
			nodes[y * nMapWidth + x].parent = nullptr;	// No parents
		}

	auto distance = [](node_map* a, node_map* b) // For convenience
	{
		return sqrtf((a->x - b->x) * (a->x - b->x) + (a->y - b->y) * (a->y - b->y));
	};

	auto heuristic = [distance](node_map* a, node_map* b) // So we can experiment with heuristic
	{
		return distance(a, b);
	};

	// Setup starting conditions
	node_map* nodeCurrent = nodeStart;
	nodeStart->fLocalGoal = 0.0f;
	nodeStart->fGlobalGoal = heuristic(nodeStart, nodeEnd);

	// Add start node to not tested list - this will ensure it gets tested.
	// As the algorithm progresses, newly discovered nodes get added to this
	// list, and will themselves be tested later
	lista<node_map*> OpenList;
	OpenList.insert(nodeStart);

	// if the not tested list contains nodes, there may be better paths
	// which have not yet been explored. However, we will also stop 
	// searching when we reach the target - there may well be better
	// paths but this one will do - it wont be the longest.
	while (!OpenList.isEmpty() && nodeCurrent != nodeEnd)// Find absolutely shortest path // && nodeCurrent != nodeEnd)
	{
		// Sort Untested nodes by global goal, so lowest is first
		node_map* lowest = OpenList.get_data_by_pos(0);
		int lowest_pos = 0;
		for (int y = 0; y < OpenList.get_object_counter(); y++) {
			if (lowest->fGlobalGoal > OpenList.get_data_by_pos(y)->fGlobalGoal) {
				lowest = OpenList.get_data_by_pos(y);
				lowest_pos = y;
			}
		}
		OpenList.swap(0, lowest_pos);

		// Front of OpenList is potentially the lowest distance node. Our
		// list may also contain nodes that have been visited, so ditch these...
		while (!OpenList.isEmpty() && OpenList.get_data_by_pos(0)->bVisited)
			OpenList.delete_by_pos(0);

		// ...or abort because there are no valid nodes left to test
		if (OpenList.isEmpty())
			break;

		nodeCurrent = OpenList.get_data_by_pos(0);
		nodeCurrent->bVisited = true; // We only explore a node once


		// Check each of this node's neighbours...
		for (int  o = 0; o < nodeCurrent->ListNeighbours.get_object_counter(); o++)
		{
			auto nodeNeighbour = nodeCurrent->ListNeighbours.get_data_by_pos(o);
			// ... and only if the neighbour is not visited and is 
			// not an obstacle, add it to NotTested List
			if (!nodeNeighbour->bVisited && !nodeNeighbour->bObstacle) {
				OpenList.insert(nodeNeighbour);
			}

			// Calculate the neighbours potential lowest parent distance
			float fPossiblyLowerGoal = nodeCurrent->fLocalGoal + distance(nodeCurrent, nodeNeighbour);

			// If choosing to path through this node is a lower distance than what 
			// the neighbour currently has set, update the neighbour to use this node
			// as the path source, and set its distance scores as necessary
			if (fPossiblyLowerGoal < nodeNeighbour->fLocalGoal)
			{
				nodeNeighbour->parent = nodeCurrent;
				nodeNeighbour->fLocalGoal = fPossiblyLowerGoal;

				// The best path length to the neighbour being tested has changed, so
				// update the neighbour's score. The heuristic is used to globally bias
				// the path algorithm, so it knows if its getting better or worse. At some
				// point the algo will realise this path is worse and abandon it, and then go
				// and search along the next best path.
				nodeNeighbour->fGlobalGoal = nodeNeighbour->fLocalGoal + heuristic(nodeNeighbour, nodeEnd);
			}
		}
	}
	return true;
}
std::string Path_Astar::send_route(std::string spectrumId,int posPlayer[2], int posEnemy[2]) {
	std::string msg = spectrumId +":Spectrum:Pathfinding:";
	if (Solve_AStar(posPlayer, posEnemy)) {
		node_map* temp_node = nodeEnd;
		while (temp_node->parent != nullptr) {
			msg += std::to_string(temp_node->x);
			msg += "," + std::to_string(temp_node->y);
			msg += ";";
			temp_node = temp_node->parent; //:x,y:
		}
	}return msg;
}

bool backtraking::is_safe(int posXY) {
	if (nodes[posXY].bObstacle || nodes[posXY].bVisited) {
		return false;
	}
	else {
		return true;
	}
}
bool backtraking::is_valid(int posXY) {
	if (posXY > 0 && posXY < nMapHeight* nMapHeight) {
		return true;
	}return false;
}

bool backtraking::find_shortest_path(int posXY, int end_pos, int dist) {
	if (posXY == end_pos) {
		if (min_dist > dist) {
			min_dist = dist;
		}
		return true;
	}
	nodes[posXY].bVisited = true;
	//ordena los nodos
	quickSort(&nodes[posXY].ListNeighbours,0,nodes[posXY].ListNeighbours.get_object_counter()-1,&nodes[end_pos]);
	//recorre los nodos vecinos en y mira si son la ruta mas corta.
	for (int i = 0; i < nodes[posXY].ListNeighbours.get_object_counter(); i++) {
		node_map* temp_node = nodes[posXY].ListNeighbours.get_data_by_pos(i);
		//node_pos es la conversion de la matriz 2D en su posicion en la de 1D
		int node_pos = (temp_node->y * nMapHeight) + (temp_node->x + (temp_node->y / nMapHeight));
		if (is_safe(node_pos) && is_valid(node_pos)) {
			temp_node->parent = &nodes[posXY];
			if (find_shortest_path(node_pos, end_pos, dist + 1)) {
				return true;
			}
		}
	}
	nodes[posXY].bVisited = false;
	return false;
}

backtraking::backtraking(node_map* mapita) {
	nodes = mapita;
	min_dist = 8323;
}

backtraking::backtraking() {
	min_dist = 8323;
}

node_map* backtraking::backtrack(int posEnemy[2], int destination[2]){
	int posXY = (posEnemy[1] * nMapHeight) + (posEnemy[0] + (posEnemy[1] / nMapWidth));
	int end_pos = (destination[1] * nMapHeight) + (destination[0] + (destination[1] / nMapHeight));
	for (int x = 0; x < nMapHeight; x++) {
		for (int y = 0; y < nMapHeight; y++)
		{
			nodes[y * nMapHeight + x].bVisited = false;
			nodes[y * nMapHeight + x].fGlobalGoal = INFINITY;
			nodes[y * nMapHeight + x].fLocalGoal = INFINITY;
			nodes[y * nMapHeight + x].parent = nullptr;	// No parents

		}
	}
	min_dist = 874127;
	find_shortest_path(posXY, end_pos, 0);
	return &nodes[end_pos];
}

std::string backtraking::send_route(int spectrumId, int posPlayer[2], int posEnemy[2]) {
	std::string msg = "";
	node_map* temp_node = backtrack(posEnemy, posPlayer);
	while (temp_node->parent != nullptr) {
		msg += std::to_string(temp_node->x);
		msg += "," + std::to_string(temp_node->y);
		msg += ";";
		temp_node = temp_node->parent; //:x,y:
	}
	return msg;
};

/* This function takes last element as pivot, places
   the pivot element at its correct position in sorted
	array, and places all smaller (smaller than pivot)
   to left of pivot and all greater elements to right
   of pivot */
int backtraking::partition(lista<node_map*>* list, int low, int high, node_map* target)
{
	int pivot = 
		std::abs((list->get_data_by_pos(high)->x) - (target->x)) + 
		std::abs((list->get_data_by_pos(high)->y) - (target->y));    // pivot 
	
	int i = (low - 1);  // Index of smaller element 

	for (int j = low; j <= high - 1; j++)
	{
		// If current element is smaller than or 
		// equal to pivot 
		int dist= 
			std::abs((list->get_data_by_pos(j)->x) - (target->x)) +
			std::abs((list->get_data_by_pos(j)->y) - (target->y));

		if (dist <= pivot)
		{
			i++;    // increment index of smaller element 
			list->swap(i,j);
		}
	}
	list->swap(i + 1, high);
	return (i + 1);
}

/* The main function that implements QuickSort
 arr[] --> Array to be sorted,
  low  --> Starting index,
  high  --> Ending index */
void backtraking::quickSort(lista<node_map*>* list, int low, int high, node_map* target)
{
	if (low < high)
	{
		/* pi is partitioning index, arr[p] is now
		   at right place */
		int pi = partition(list, low, high, target);

		// Separately sort elements before 
		// partition and after partition 
		quickSort(list, low, pi - 1, target);
		quickSort(list, pi + 1, high, target);
	}
}
node_map* backtraking::CreateMap(int size)
{
	// Create a 2D array of nodes - this is for convenience of rendering and construction
	// and is not required for the algorithm to work - the nodes could be placed anywhere
	// in any space, in multiple dimensions...
	nMapHeight = size;
	nMapHeight = size;
	nodes = new node_map[nMapWidth * nMapHeight];
	for (int x = 0; x < nMapWidth; x++)
		for (int y = 0; y < nMapHeight; y++)
		{
			nodes[y * nMapWidth + x].x = x; // ...because we give each node its own coordinates
			nodes[y * nMapWidth + x].y = y;
			nodes[y * nMapWidth + x].bObstacle = false;
			nodes[y * nMapWidth + x].parent = nullptr;
			nodes[y * nMapWidth + x].bVisited = false;
		}

	// Create connections - in this case nodes are on a regular grid
	for (int x = 0; x < nMapWidth; x++)
		for (int y = 0; y < nMapHeight; y++)
		{
			if (y > 0)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y - 1) * nMapWidth + (x + 0)]);
			if (y < nMapHeight - 1)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 1) * nMapWidth + (x + 0)]);
			if (x > 0)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 0) * nMapWidth + (x - 1)]);
			if (x < nMapWidth - 1)
				nodes[y * nMapWidth + x].ListNeighbours.insert(&nodes[(y + 0) * nMapWidth + (x + 1)]);
		}
	return nodes;
}

std::string bresenham(int x1, int y1, int x2, int y2)
{
	int dY = (y2 - y1);
	int dX = (x2 - x1);
	int IncYi;
	int IncXi;
	if (dY >= 0) {
		IncYi = 1;
	}
	else {
		dY = -dY;
		IncYi = -1;
	}
	if (dX >= 0) {
		IncXi = 1;
	}
	else {
		dX = -dX;
		IncXi = -1;
	}
	int IncYr;
	int IncXr;
	if (dX >= dY) {
		IncYr = 0;
		IncXr = IncXi;
	}
	else {
		IncXr = 0;
		IncYr = IncYi;
		//posibles cambios
		int k = dX;
		dX = dY;
		dY = k;
	}
	int X = x1;
	int Y = y1;
	int avR = (2 * dY);
	int av = (avR - dX);
	int avI = (av - dX);
	std::string msg;
	while ((X != x2) || (Y != y2)) {
		msg += std::to_string(X) + "," + std::to_string(Y);
		msg += ";";
		if (av >= 0) {
			X = (X + IncXi);     // X aumenta en inclinado.
			Y = (Y + IncYi);    // Y aumenta en inclinado.
			av = (av + avI);
		}
		else {
			X = (X + IncXr);     // X aumenta en recto.
			Y = (Y + IncYr);    // Y aumenta en recto.
			av = (av + avR);
		}
	}return msg;
}
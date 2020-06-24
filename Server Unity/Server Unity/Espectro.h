#pragma once

#include <iostream>

class Espectro{

	Espectro() {

	}

	std::string backtracking_path;
	std::string patrullar;
	std::string path_a;
	int follow_speed;

	void runaway();
	void backtracking();
	void pathfinding_A();

	void set_backtracking(std::string _backtracking) {
		backtracking_path = _backtracking;
	}
	void set_patrullar(std::string _patrullar) {
		patrullar = _patrullar;
	}
	void set_follow_speed(int _speed) {
		follow_speed = _speed;
	}
	std::string get_backtracking_path() {
		return backtracking_path;
	}
	std::string get_patrullar() {
		return patrullar;
	}
	std::string get_path_a() {
		return path_a;
	}
	int get_follow_speed() {
		return follow_speed;
	}

};
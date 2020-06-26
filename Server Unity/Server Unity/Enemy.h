#pragma once
class Enemy
{
protected:
	int pos_x;
	int pos_y;
	int health;
	int speed;
	int vision_range;
	int dmg;

	Enemy(int _health, int x, int y, int _speed, int _dmg, int vision) {
		pos_x = x;
		pos_y = y;
		health = _health;
		speed = _speed;
		vision_range = vision;
		dmg = _dmg;
	}
	Enemy(int x, int y) {
		pos_x = x;
		pos_y = y;
	}
	//virtual void hit_player();
	//virtual void recive_dmg();
	void set_x(int x) {
		pos_x = x;
	}
	void set_y(int y) {
		pos_y = y;
	}
	void set_health(int _health) {
		health = _health;
	}
	void set_speed(int _speed) {
		speed = _speed;
	}
	void set_vision(int _vision) {
		vision_range = _vision;
	}
	void set_dmg(int _dmg) {
		dmg = _dmg;
	}
	int get_x() {
		return pos_x;
	}
	int get_y() {
		return pos_y;
	}
	int get_health() {
		return health;
	}
	int get_speed() {
		return speed;
	}
	int get_vision() {
		return vision_range;
	}
	int get_dmg() {
		return dmg;
	}
};


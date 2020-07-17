#pragma once
/**
 * @brief enemy representetion from the server side
 * 
 */
class Enemy
{
protected:
	int pos_x;
	int pos_y;
	int health;
	int speed;
	int vision_range;
	int dmg;
	/**
	 * @brief Construct a new Enemy object
	 * @param _health health assosiated to this enemy
	 * @param x coordinates in the x axis of this enemy
	 * @param y coordinates in the y axis of this enemy
	 * @param _speed speed assosiated to this enemy
	 * @param _dmg damage assosiated to this enemy
	 * @param vision vision range assosiated to this enemy
	 */
	Enemy(int _health, int x, int y, int _speed, int _dmg, int vision) {
		pos_x = x;
		pos_y = y;
		health = _health;
		speed = _speed;
		vision_range = vision;
		dmg = _dmg;
	}
	/**
	 * @brief Construct a new Enemy object
	 * 
	 * @param x coordinates in the x axis of this enemy
	 * @param y coordinates in the y axis of this enemy
	 */
	Enemy(int x, int y) {
		pos_x = x;
		pos_y = y;
	}
	/**
	 * @brief Construct a new Enemy object
	 * 
	 */
	Enemy() {};
	//virtual void hit_player();
	//virtual void recive_dmg();
	/**
	 * @brief Set the x object
	 * 
	 * @param x coordinates in the x axis
	 */
	void set_x(int x) {
		pos_x = x;
	}
	/**
	 * @brief Set the y object
	 * 
	 * @param y coordinates in the y axis
	 */
	void set_y(int y) {
		pos_y = y;
	}
	/**
	 * @brief Set the health object
	 * 
	 * @param _health health to assosiate
	 */
	void set_health(int _health) {
		health = _health;
	}
	/**
	 * @brief Set the speed object
	 * 
	 * @param _speed speed to assosiate 
	 */
	void set_speed(int _speed) {
		speed = _speed;
	}
	/**
	 * @brief Set the vision object
	 * 
	 * @param _vision vision range to assosiate
	 */
	void set_vision(int _vision) {
		vision_range = _vision;
	}
	/**
	 * @brief Set the dmg object
	 * 
	 * @param _dmg damage to assosiate
	 */
	void set_dmg(int _dmg) {
		dmg = _dmg;
	}
	/**
	 * @brief Get the x object
	 * 
	 * @return x axis coordinates
	 */
	int get_x() {
		return pos_x;
	}
	/**
	 * @brief Get the y object
	 * 
	 * @return int axis coordinates
	 */
	int get_y() {
		return pos_y;
	}
	/**
	 * @brief Get the health object
	 * 
	 * @return health of the enemy
	 */
	int get_health() {
		return health;
	}
	/**
	 * @brief Get the speed object
	 * 
	 * @return speed of the enemy
	 */
	int get_speed() {
		return speed;
	}
	/**
	 * @brief Get the vision object
	 * 
	 * @return the vision range of the enemy
	 */
	int get_vision() {
		return vision_range;
	}
	/**
	 * @brief Get the dmg object
	 * @return damage that this enemy deals with a punch
	 */
	int get_dmg() {
		return dmg;
	}
};


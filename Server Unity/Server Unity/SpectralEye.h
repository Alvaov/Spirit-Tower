#pragma once

class SpectralEye {


public:
	int id;
	int pos_x;
	int pos_y;
	int health;

	SpectralEye(int newId,int posX, int posY) {
		id = newId;
		pos_x = posX;
		pos_y = posY;
		health = 1;
	}
};

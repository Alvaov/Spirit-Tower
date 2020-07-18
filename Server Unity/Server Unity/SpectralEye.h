#pragma once
/**
 * @brief representa los ojos espectrales
 * 
 */
class SpectralEye {

public:
	int id;
	int pos_x;
	int pos_y;
	int health;
	/**
	 * @brief Construct a new Spectral Eye object
	 * 
	 * @param newId id del espectro
	 * @param posX cordenada en el eje x
	 * @param posY cordenada en el eje y
	 */
	SpectralEye(int newId,int posX, int posY) {
		id = newId;
		pos_x = posX;
		pos_y = posY;
		health = 1;
	}
};

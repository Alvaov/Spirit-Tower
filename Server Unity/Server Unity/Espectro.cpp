#Include "Espectro.h"

void Espectro::give_path(int lvl, int x, int y, int _id) {
	id = _id;
	pos_x = x;
	pos_y = y;
	path = lista<std::string>();
	//se asignan las rutas de patrullaje segun el nivel y el espectro
	switch (id) {
	case 0:
		switch (lvl) {
		case 1:
			path.insert("39,32");
			path.insert("74,32");
			path.insert("74,54");
			path.insert("39,54");
			break;
		default:
			break;
		}
		break;
	case 1:
		switch (lvl) {
		case 1:
			path.insert("81,36");
			path.insert("81,58");
			path.insert("107,58");
			path.insert("107,36");
			break;
		default:
			break;
		}
		break;
	case 2:
		switch (lvl) {
		case 1:
			path.insert("66,64");
			path.insert("89,64");
			path.insert("89,80");
			path.insert("36,80");
			path.insert("36,96");
			path.insert("66,96");
			break;
		default:
			break;
		}
		break;
	default:
		break;
	}
};
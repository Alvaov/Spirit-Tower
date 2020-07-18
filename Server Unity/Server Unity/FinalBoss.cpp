#include "FinalBoss.h"


FinalBoss::FinalBoss() {
	life = 6;
	actualPhase = 0;
	path = new lista<std::string>();
}

void FinalBoss::setRoute() {
	switch (actualPhase) {
	case 0:
		path->insert("61,70");
		path->insert("70,60");
		path->insert("61,50");
		path->insert("70,60");
		path->insert("61,70");
		path->insert("70,60");
		break;

	case 1:
		path->insert("61,70");
		path->insert("70,60");
		path->insert("61,50");
		path->insert("51,54");
		path->insert("51,54");
		break;
	default:
		break;
	}
}

lista<std::string>* FinalBoss::get_path() {
	return path;
}

std::string FinalBoss::getPath(int contador) {
	if (contador < path->get_object_counter() && contador > -1) {
		std::string ruta = path->get_data_by_pos(contador);
		return ruta;
	}
}
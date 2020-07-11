#include "Enemy_Genetics.h"


Enemy_Genetics::Enemy_Genetics() {}

void Enemy_Genetics::work() {
    // Semilla random
    srand(time(NULL));

    // Instanciacion de enemigos
    for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
        Enemies.insert(new Person());
        // Generacion aleatoria de los parametros
        // Calcular el fitness del enemigo
    }

    for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
        // Decidir si un enemigo es apto o no
        Enemies.get_data_by_pos(i)->selection();
    }

    for (int i = 0; i < AMOUNT_OF_PEOPLE; i = i + 2) {
        // Reproduccion: cruce, mutacion e inversion
        Person* child = Enemies.get_data_by_pos(i)->crossover(Enemies.get_data_by_pos(i + 1));
        // Insertar el hijo en la poblacion
        Enemies.insert(child); 
    }
    quickSortGenetics(&Enemies, 0, Enemies.get_object_counter()-1);
    // Mostrar la poblacion
	for (int i = Enemies.get_object_counter() - 1; i > AMOUNT_OF_PEOPLE-1; i--) {
		delete Enemies.get_data_by_pos(i);
		Enemies.delete_by_pos(i);
	}
	std::cout << "prueba";
}
int partitionGenetics(lista<Person*>* list, int low, int high)
{
    double pivot = list->get_data_by_pos(high)->getFitness();  // pivot 
	int i = (low - 1);  // Index of smaller element 
	for (int j = low; j <= high - 1; j++)
	{
		// If current element is smaller than or 
		// equal to pivot 
		double fitness  = list->get_data_by_pos(j)->getFitness();
		if (fitness >= pivot)
		{
			i++;    // increment index of smaller element 
			list->swap(i, j);
		}
	}
	list->swap(i + 1, high);
	return (i + 1);
}

/* The main function that implements QuickSort
 list --> Array to be sorted,
  low  --> Starting index,
  high  --> Ending index */
void quickSortGenetics(lista<Person*>* list, int low, int high)
{
	if (low < high)
	{
		/* pi is partitioning index, arr[p] is now
		   at right place */
		int pi = partitionGenetics(list, low, high);

		// Separately sort elements before 
		// partition and after partition 
		quickSortGenetics(list, low, pi - 1);
		quickSortGenetics(list, pi + 1, high);
	}
}

lista<Person*> Enemy_Genetics::getList() {
    return Enemies;
}
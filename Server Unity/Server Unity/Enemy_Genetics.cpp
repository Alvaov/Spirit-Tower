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

    
    // Mostrar la poblacion
     for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
         std::cout << std::endl;
         std::cout << std::endl;
         std::cout << std::endl;
         for (int j = 0; j <= 31; ++j) {
            std::cout << Enemies.get_data_by_pos(i)->getBit(j);
         }
     }
}
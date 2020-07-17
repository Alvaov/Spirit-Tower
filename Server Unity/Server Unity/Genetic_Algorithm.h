#include <iostream>
#include <random>
#include <map>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>
#include "Linked_List.h"

#define AMOUNT_OF_PEOPLE 100
#define DEBUG 0

/*
Solo falta separarlo
*/


/**
 * @brief clase que representa a los individuos del algoritmo genetico
 * 
 */
class Person {
private:
    uint8_t bitArray[4];
    int health = 1;
    int speed = 30;
    int vision_range = 60;
    int follow_speed = 45;
    int fitness;
    bool alive = true;
  
public:
/**
 * @brief Construct a new Person object
 * 
 */
    Person() {

        // inicializar memoria
        for (int i = 0; i < 31; ++i) {

            int random_bit = randomBool();
            setBit(i, random_bit);
            //setBit(this->bitarray, i);

        }

        // Mapeo de valores a atributos
        map();

        // Calcular fitness
        calculateFitness();

        // Prints
        if (DEBUG) {
            std::cout << "Health: " << health << std::endl;
            std::cout << "Speed: " << speed << std::endl;
            std::cout << "Vision Range: " << vision_range << std::endl;
            std::cout << "Follow speed: " << follow_speed << std::endl;
            std::cout << "Fitness: " << fitness << std::endl;
        }

    }
    /**
     * @brief Destroy the Person object
     * 
     */
    ~Person() {

    }
    /**
     * @brief Get the health object
     * 
     * @return vida
     */
    int get_health() {
        return health;
    }
    /**
     * @brief Get the speed object
     * 
     * @return velocidad
     */
    int get_speed() {
        return speed;
    }
    /**
     * @brief Get the vision range object
     * 
     * @return rango de vision
     */
    int get_vision_range() {
        return vision_range;
    }
    /**
     * @brief Get the follow speed object
     * 
     * @return velocidad de persecusion
     */
    int get_follow_speed() {
        return follow_speed;
    }
    /**
     * @brief generar un booleano al azar
     * 
     * @return booleano al azar
     */
    int randomBool() {
        return 0 + (rand() % (1 - 0 + 1)) == 1;
    }

    // Genera un numero random entre 0 y 100
    /**
     * @brief generar un numero al azar entre max y min
     * 
     * @param min minimo 
     * @param max maximo
     * @return numero generado
     */
    int randomNum(int min, int max) {
        int random_num = rand() % (max - min + 1) + min;
        return random_num;
    }

    // Obtener valor de un bit en una posicion
    /**
     * @brief Get the Bit object
     * 
     * @param index indice del bit
     * @return bit del array
     */
    int getBit(int index) {
        return (this->bitArray[index >> 3] >> (index & 7)) & 1;
    }
    /*
    int getBit(int bitarray[4], int k) {
        return (this->bitarray[(k / 32)] & (1 << (k % 32)));
    }*/

    // establecer valor de un bit en una posicion
    /**
     * @brief Set the Bit object
     * 
     * @param index indice del bit a setear
     * @param value valor nuevo del bit
     */
    void setBit(int index, int value) {
        uint8_t celda = this->bitArray[index >> 3];
        celda &= ~(1 << (index & 7));

        celda |= value << (index & 7);

        this->bitArray[index >> 3] = celda;

    }
    /*
    void setBit(int bitarray[4], int k) {
        (this->bitarray[(k / 32)] |= (1 << (k % 32)));
    }*/

    // Asigna los valores de cada caracteristica del cromosoma
    /**
     * @brief asigna las caracteristicas a los cromosomas
     * 
     */
    void map() {

        for (int i = 0; i <= 7; ++i) {
            health += getBit(i) * pow(2, i);
            //health += getBit(this->bitarray[i]) * pow(2, i);
        }

        for (int i = 8; i <= 15; ++i) {
            speed += getBit(i) * pow(2, i - 8);
        }

        for (int i = 16; i <= 23; ++i) {
            vision_range += getBit(i) * pow(2, i - 16);
        }

        for (int i = 24; i <= 31; ++i) {
            follow_speed += getBit(i) * pow(2, i - 24);
        }

    }

    // Funcion Fitness
    /**
     * @brief calcula el fitness
     * 
     */
    void calculateFitness() {
        double max_value = 255;

        double health_weight = 0.25;
        double speed_weight = 0.25;
        double vision_range_weight = 0.25;
        double damage_weight = 0.25;

        double health_normalized = (health / max_value) * 100;
        double speed_normalized = (speed / max_value) * 100;
        double vision_range_normalized = (vision_range / max_value) * 100;
        double damage_normalized = (follow_speed / max_value) * 100;

        fitness = health_normalized * health_weight +
            speed_normalized * speed_weight +
            vision_range_normalized * vision_range_weight +
            damage_normalized * damage_weight;

    }
    /**
     * @brief Get the Fitness
     * 
     * @return fitness del individuo
     */
    double getFitness() {
        return fitness;
    }
    /*
    void clearMemory() {
        std::cout << "Clear: ";
        for (int i = 0; i <= 31; ++i) {
            std::cout << this->getBit(i);
        }

        std::cout << std::endl;

        for (int i = 0; i <= 31; ++i) {

            //this->setBit(i, false);
            this->setBit(i, 0);

        }

        std::cout << "Clear: ";

        for (int i = 0; i <= 31; ++i) {
            std::cout << this->getBit(i);
        }

        std::cout << std::endl;
    }*/

    // Seleccion natural
    /**
     * @brief decide si deberian vivir o morir
     * 
     */
    void selection() {
        // si el fitness es mayor a 70 lo selecciona
        if (fitness >= 70) {
            alive = true;
        }
        else {
            alive = false;
        }
    }

    // Reproduccion: Cruce, mutacion, inversion
    /**
     * @brief cruce entre individuos
     * 
     * @param person persona a cruzar con
     * @return Person* nueva persona
     */
    Person* crossover(Person* person) {
        // Crea el hijo
        Person* child = new Person();
        //child->clearMemory();

        for (int i = 0; i <= 31; ++i) {
            // combina los cromosomas de los dos progenitores con &&
            int value_inherited = this->getBit(i) && person->getBit(i);
            // le asigna el valor obtenido de la combinacion al hijo 
            child->setBit(i, value_inherited);

            //std::cout << this->getBit(i) << person->getBit(i) << value_inherited << std::endl;

        }
        int random_mutation = randomNum(0, 100);

        // ocurre mutacion
        if (0 <= random_mutation && random_mutation <= 10) {
            child->mutation();
        }

        // Hijo mutado
       int random_inversion = randomNum(0, 100);

        // ocurre inversion
        if (0 <= random_inversion  && random_inversion <= 5) {
            child->inversion();
        }

        // Hijo con inversion
        return child;
    }

    // Mutacion
    /**
     * @brief muta bits del cromosoma
     * 
     */
    void mutation() {
        //bit random donde se aplicara la mutacion
        int random_bit = randomNum(0, 31);
        setBit(random_bit, getBit(random_bit) + 1);
    }

    //Inversion
    /**
     * @brief invierte cierta cantidad de bits
     * 
     */
    void inversion() {

        int random_bit_inicial = randomNum(0, 31);
        int random_bit_final = random_bit_inicial + 10;
        if (random_bit_final > 31) {
            random_bit_final = 31;
        }
        
        for (int i = random_bit_inicial; i <= random_bit_final; ++i) {
            setBit(i, getBit(i) + 1);
        }
    }

};
    
    /*Como array
    int main(int argc, char const* argv[]) {
        // Semilla random
        srand(time(NULL));

        // Lista de enemigos (Poblacion)
        Person* people[AMOUNT_OF_PEOPLE];

        // Instanciacion de enemigos
        for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
            people[i] = new Person();
            // Generacion aleatoria de los parametros
            // Calcular el fitness del enemigo
        }

        for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
            people[i]->selection();
            // Decidir si un enemigo es apto o no
        }

        for (int i = 0; i < AMOUNT_OF_PEOPLE; i = i + 2) {
            people[i]->crossover(people[i + 1]);
        }


        // Mostrar la poblacion
        for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
            std::cout << std::endl;
            std::cout << std::endl;
            std::cout << std::endl;
            for (int j = 0; j <= 31; ++j) {
                std::cout << people[i]->getBit(j);
            }
        }

        return 0;

    }

// Utilizando la lista
    int main(int argc, char const* argv[]) {
        
        
        Enemy_Genetics* enemy_genetics = new Enemy_Genetics();
        enemy_genetics->work();

        return 0;

    }*/
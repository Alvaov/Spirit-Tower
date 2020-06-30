#include <iostream>
#include <random>
#include <map>
#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <time.h>


#define AMOUNT_OF_PEOPLE 2
#define DEBUG 0

/*
Notes:
Revisar y arreglar el setBit para la mutacion,
Separar todo el archivo
Adecuarlo a las clases de Enemy y Espectro

De acuerdo al diagrama del funcionamiento de algoritmos geneticos quede en cruce, mutacion (falta inversion)

*/


int randomBool() {
    return 0 + (rand() % (1 - 0 + 1)) == 1;
}

class Person
{
    int bitarray[4];
    int health;
    int speed;
    int vision_range;
    int damage;
    int fitness;
    bool alive;

public:
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
            std::cout << "Damage: " << damage << std::endl;
            std::cout << "Fitness: " << fitness << std::endl;
        }

    }
    ~Person();


    // Obtener valor de un bit en una posici�n

    int getBit(int index) {
        return (int)((this->bitarray[index / 8] >> 7 - (index & 0x7)) & 0x1);
    }
    /*
    int getBit(int bitarray[4], int k) {
        return (this->bitarray[(k / 32)] & (1 << (k % 32)));
    }*/

    // establecer valor de un bit en una posici�n

    void setBit(int index, int value) {
        this->bitarray[index / 8] = this->bitarray[index / 8] | (value & 0x1) << 7 - (index & 0x7);
    }
    /*
    void setBit(int bitarray[4], int k) {
        (this->bitarray[(k / 32)] |= (1 << (k % 32)));
    }*/

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
            damage += getBit(i) * pow(2, i - 24);
        }

    }

    void calculateFitness() {
        double max_value = 255;

        double health_weight = 0.25;
        double speed_weight = 0.25;
        double vision_range_weight = 0.25;
        double damage_weight = 0.25;

        double health_normalized = (health / max_value) * 100;
        double speed_normalized = (speed / max_value) * 100;
        double vision_range_normalized = (vision_range / max_value) * 100;
        double damage_normalized = (damage / max_value) * 100;

        fitness = health_normalized * health_weight +
            speed_normalized * speed_weight +
            vision_range_normalized * vision_range_weight +
            damage_normalized * damage_weight;

    }

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
    }

    void selection() {
        if (fitness >= 70) {
            alive = true;
        }
        else {
            alive = false;
        }
    }


    Person* crossover(Person* person) {
        Person* child = new Person();
        //child->clearMemory();

        for (int i = 0; i <= 31; ++i) {

            int value_inherited = this->getBit(i) && person->getBit(i);

            //child->setBit(i,value_inherited);

            std::cout << this->getBit(i) << person->getBit(i) << value_inherited << std::endl;

        }

        // Progenitor 1
        for (int i = 0; i <= 31; ++i) {
            std::cout << this->getBit(i);
        }
        std::cout << std::endl;

        // Progenitor 2
        for (int i = 0; i <= 31; ++i) {
            std::cout << person->getBit(i);
        }
        std::cout << std::endl;

        // Hijo
        for (int i = 0; i <= 31; ++i) {
            std::cout << child->getBit(i);
        }

        std::cout << std::endl;

        return child;
    }

};




/*int main(int argc, char const* argv[])
{
    // Semilla random
    srand(time(NULL));

    // Lista de enemigos
    Person* people[AMOUNT_OF_PEOPLE];

    // Instanciaci�n de enemigos
    for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
        people[i] = new Person();
        // Generaci�n aleatoria de los par�metros
        // Calcular el fitness del enemigo
    }

    for (int i = 0; i < AMOUNT_OF_PEOPLE; ++i) {
        people[i]->selection();
        // Decidir si un enemigo es apto o no
    }

    for (int i = 0; i < AMOUNT_OF_PEOPLE; i = i + 2) {
        people[i]->crossover(people[i + 1]);
        // Decidir si un enemigo es apto o no
    }

    return 0;

}*/
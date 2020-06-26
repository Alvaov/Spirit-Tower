using System.Collections.Generic;
using System;

public class Nodo<Object> {
    private Nodo<Object> anterior;
    private Nodo<Object> siguiente;
    private Object valor;
    /**
     * El constructor crea un nodo con el valor asignado
     * @param valor
     */
    public Nodo(Object valor){
      this.valor = valor;
      this.siguiente = null;
      this.anterior = null;
    }
    /**
     * La función setValor le cambia el valor al nodo,
     * valorNuevo sera el valor que va a sustituir el valor del nodo.
     * @param valorNuevo
     */
    public void setValor(Object valorNuevo){
        this.valor = valorNuevo;
    }
    /**
     * retorna el valor del nodo
     * @return
     */
    public Object getValor(){
        return valor;
    }
    /**
     * Asigna el siguienteNodo a ser el valor de siguiente,
     * en otras palabras enlaza el nodo con el siguiente
     * @param siguienteNodo
     */
    public void setSiguiente(Nodo<Object> siguienteNodo){
        this.siguiente = siguienteNodo;
    }
    /**
     * retorna el siguiente nodo
     * @return
     */
    public Nodo<Object> getSiguiente(){
        return siguiente;
    }
    /**
     * asigna el nodoAnterior a ser el nuevo nodoAnterior
     * @param nodoAnterior
     */
    public void setAnterior(Nodo<Object> nodoAnterior){
        this.anterior = nodoAnterior;
    }
    /**
     * retorna el nodo anterior a este
     * @return
     */
    public Nodo<Object> getAnterior(){
        return anterior;
    }
}

/**
 *
 * @author Brian Wagemans Alvarado
 * Como referencia se utiliza el video de Youtube "Listas enlazadas pt 2- Clase lista enlazada
 * métodos básicos(añadir,obtener,tamaño,vacia)" por latincoder.
 */
public class Lista<Object>{
    private Nodo<Object> primerElemento;
    private Nodo<Object> ultimoElemento;
    private int cantidadElementos = 0;
    /**
     * se inicializa la Lista sin Nodos.
     */
    public Lista(){
        this.primerElemento = null;
        this.ultimoElemento = null;
    }
    /**
     * retorna un true si esta vacia, false caso contrario
     * @return true
     * True si esta vacia, False si tiene elementos
     */
    public bool estaVacia(){
        if (cantidadElementos == 0){
            return true;
        }else{
            return false;
        }
    }
    /**
     * retorna la cantidad de elementos, es 1+ de la cantidad de indices.
     * @return
     *  El tamaño de la lista
     */
    public int getTamaño(){
        return cantidadElementos;
    }
    /**
     * Crea un Nodo con Cualquier valor y lo anida a la cola de la lista
     * @param valor
     */
    public void añadirElementos(Object valor){
        if (primerElemento == null){
            primerElemento = new Nodo<Object>(valor);
            ultimoElemento = primerElemento;
            primerElemento.setSiguiente(ultimoElemento);
        }else{

          Nodo<Object> ultimo = getNodo(cantidadElementos-1);
          ultimo.setSiguiente(new Nodo<Object>(valor));
          ultimoElemento = ultimo.getSiguiente();

        }
        cantidadElementos+=1;
    }
    /**
     * retorna el Nodo en el indice especificado
     * @param indice
     * @return
     */
    private Nodo<Object> getNodo(int indice){
        Nodo<Object> buscador=primerElemento;
        int contador= 0;
        while(contador<indice){
            buscador = buscador.getSiguiente();
            contador++;
        }
        return buscador;
    }
    /**
     * metodo que retorna el valor del nodo en el indice especificado como el
     * parametro indice(NO RETORNA EL NODO).
     * @param indice
     * @return
     */
    public Object getValorEnIndice(int indice){
        int contador = 0;
        Nodo<Object> buscador = primerElemento;
        while (contador<indice){
            buscador = buscador.getSiguiente();
            contador+=1;
        }
        return buscador.getValor();
    }
    /**
     * Función que deja sobreEscribir el valor en el nodo Especificado
     * nuevo valor del nodo es el parametro valor
     * indice es el indice del nodo en la lista
     * @param indice
     * @param valor
     */
    public void sobreEscribirNodo(int indice, Object valor){
        if (cantidadElementos !=0 ){
            if (indice<cantidadElementos){
                getNodo(indice).setValor(valor);
            }else{
                Console.WriteLine("El indice esta fuera de la Lista");
            }
        }else{
            Console.WriteLine("No puede sobrescribir una lista vacia");
        }
    }
    /**
     * retorna el valor del ultimo Nodo
     * @return
     */
    public Object getUltimoElemento(){
        return ultimoElemento.getValor();
    }
    /**
     * retorna el valor del primer Nodo.
     * @return
     */
    public Object getPrimerElemento(){
        return primerElemento.getValor();
    }
    /**
     * Elimina el elemento en el indice que se espera.
     * El indice marca el elemento a eliminar.
     * @param indice
     */
    public void Eliminar(int indice){
        if (indice == 0){
            primerElemento = primerElemento.getSiguiente();
            primerElemento.setAnterior(null);
        }
        if (indice == cantidadElementos-1){
            ultimoElemento = ultimoElemento.getAnterior();
            ultimoElemento.setSiguiente(null);

        }else{
            Nodo<Object> buscador = primerElemento;
            int contador = 0;
            while (contador<indice+1){
                buscador = buscador.getSiguiente();
                contador++;
            }
            getNodo(indice-1).setSiguiente(buscador);
            getNodo(indice-1).setAnterior(getNodo(indice-2));
        }
        cantidadElementos-=1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase encargada de manejar los nodos que utiliza el grid 
 * para construir un mapa en forma de matriz, posee la información
 * de la posición x,y, walkable, worldPosition.
 */
public class Node
{
    public bool walkable;
    public Vector3 worldPosition;
    public int x;
    public int y;

    /***
     * Constructor del nodo utilizado en el grid.
     * @params bool walkable Vector3 worldPos int x int y
     */
    public Node(bool _walkable, Vector3 _worldPos, int _x, int _y)
    {
        walkable = _walkable;
        worldPosition = _worldPos;
        x = _x;
        y = _y;
    }
}

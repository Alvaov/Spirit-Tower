using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/***
 * Clase encargada de toda la lógica y comportamiento 
 * que debe presentar la rata en el juego.
 */
public class RatScript : MonoBehaviour
{

    public CharacterController rat;
    public int id;
    public int speed = 1;
    private Vector3 target;
    public float frameInterval;
    public bool addedToList = false;
    public string[] path;


    // Start is called before the first frame update
    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    void Start()
    {
        rat = GetComponent<CharacterController>();
        id = Client.ratId;
        frameInterval = 500 + (id * 5) + id;
        Client.ratId += 1;
    }

    // Update is called once per frame
    /***
     * Método que se ejecuta una vez por frame
     * evaluando constantemente la información 
     * y el estado actual de la rata
     * para su correcto funcionamiento.
     */
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.rats.getTamaño(); i++)
            {
                if (Client.instance.rats.getValorEnIndice(i).id == this.id)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.rats.añadirElementos(this);
            }
            Client.instance.tcp.SendData(id + ":Rat:New:" + Grid.instance.GetAxesFromWorldPoint(rat.transform.position) + ":");
        }

        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData(id + ":Rat:Move:" + Grid.instance.GetAxesFromWorldPoint(rat.transform.position) + ":");
        }

        Walk();
    }

    /***
     * Método encargado de procesar los elementos que tenga en el array
     * de strings que contiene el camino indicado por el servidor
     * se encarga de recorrer esta lista y en caso de llegar al final recorre nuevamente la lista
     * debido a que esta se actualiza constantemente por parte del servidor.
     */
    void Walk()
    {
        try
        {
            string[] pos_grid = path[0].Split(',');
            int x;
            int z;
            Int32.TryParse(pos_grid[0], out x);
            Int32.TryParse(pos_grid[1], out z);

            target = Grid.instance.GetWorldPointFromAxes(x, z);

            if (transform.position != target)
            {
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                FaceTarget();

            }
        }
        catch{
            //Debug.Log("Error convirtiendo a entero rat");
        }
    }

    /***
     * Método que calcula la rotación necesaria 
     * para el objeto con el propósito de que mire 
     * al objetivo actual de su ruta.
     */
    void FaceTarget()
    {
        Vector3 direction = (transform.position - target).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }
}

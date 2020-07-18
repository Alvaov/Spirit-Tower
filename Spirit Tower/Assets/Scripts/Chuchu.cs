using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/***
 * Clase encargada de administrar las acciones,
 * en general, que require el chuchu
 * para su correcto funcionamiento.
 */
public class Chuchu : MonoBehaviour
{

    public CharacterController chuchu;
    public int id;
    public int speed = 1;
    public int stepPath = 0;
    public bool stop = false;
    private Vector3 target;
    public float frameInterval;
    public bool addedToList = false;
    public string[] path;


    GameObject player;
    Player playerScript;


    // Start is called before the first frame update
    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    void Start()
    {
        chuchu = GetComponent<CharacterController>();
        id = Client.chuchuId;
        frameInterval = 160 + (id * 10) + id;
        Client.chuchuId += 1;
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
    }
    /***
     * Método que detecta mientras haya otro collider dentro de sí mismo, 
     * si es el jugador lo ataca y
     * notifica al servidor.
     */
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Player.defender)
            {
                Client.instance.tcp.SendData("0:Chuchu:Attack::");
            }
        }
    }

    /***
     * Método que detecta colisiones con collider, 
     * si es la espada el chuchu se muere y
     * notifica al servidor.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (Player.atacar) 
            {
                playerScript.monedas += 200;
                Client.instance.tcp.SendData(id + ":Chuchu:Damage:1:");
                Destroy(gameObject);
            }
        }
    }


    /***
     * Método que se ejecuta cada frame en el cual se realizan las acciones
     * necesarias para una correcta operación del chuchu.
     */
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.chuchus.getTamaño(); i++)
            {
                if (Client.instance.chuchus.getValorEnIndice(i).id == this.id)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.chuchus.añadirElementos(this);
            }
            Client.instance.tcp.SendData(id + ":Chuchu:New:" + Grid.instance.GetAxesFromWorldPoint(chuchu.transform.position) + ":");

        }
        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData(id + ":Chuchu:Move:" + Grid.instance.GetAxesFromWorldPoint(chuchu.transform.position) + ":");
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
        if (path.Length > 0)
        {
            try
            {
                if (stepPath == path.Length - 1)
                {
                    stepPath = 0;
                }
                string[] pos_grid = path[stepPath].Split(',');
                int x;
                int z;
                Int32.TryParse(pos_grid[0], out x);
                Int32.TryParse(pos_grid[1], out z);

                target = Grid.instance.GetWorldPointFromAxes(x, z);

                if (transform.position != target)
                {
                    FaceTarget();
                    if (!stop)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }
                    if (stop)
                    {
                        Vector3 movement = new Vector3(0, 0, -1);
                        movement *= speed * Time.deltaTime;
                        movement = transform.TransformDirection(movement);
                    }

                }
                if (transform.position == target)
                {
                    stepPath++;
                }

            }
            catch
            {
                //Debug.Log("Error convirtiendo string a entero chuchu");
            }
        }
    }
    /***
     * Método que calcula la rotación necesaria 
     * para el objeto con el propósito de que mire 
     * al objetivo actual de su ruta.
     */
    void FaceTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }
}

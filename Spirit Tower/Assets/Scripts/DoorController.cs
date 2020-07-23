using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/***
 * Clase encargada de administrar los distintos tipos de puertas que
 * aparecene en el juego
 */
public class DoorController : MonoBehaviour
{
    public Animator animator;
    public GameObject door;
    bool openDoor = false;
    bool isPlayerNear;

    public bool isLocked;
    public bool MasterDoor;
    public bool doorUnlocked;
    public AudioSource open;

    GameObject player;
    Player playerScript;

    public Text Avisos;

    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
        Avisos = GameObject.Find("Avisos").GetComponent<Text>();

    }

    /***
     * Método que se ejecuta una vez por frame
     * evaluando constantemente la información 
     * y el estado actual de las puertas
     * para su correcto funcionamiento.
     */
    private void Update()
    {
        if (isPlayerNear)
        {
            if (!isLocked || doorUnlocked)
            {
                open.Play();
                openDoor = true;
                animator.SetBool("openDoor", true);
            }

            if (isLocked && Input.GetKeyDown(KeyCode.Z) && playerScript.llaves > 0 && !MasterDoor)
            {
                open.Play();
                playerScript.llaves -= 1;
                openDoor = true;
                animator.SetBool("openDoor", true);
                isLocked = false;
            }

            if (MasterDoor && Input.GetKeyDown(KeyCode.Z) && playerScript.hasMasterKey == 1)
            {
                open.Play();
                openDoor = true;
                animator.SetBool("openDoor", true);
                doorUnlocked = true;
                playerScript.hasMasterKey -= 1;
                MasterDoor = false;
                isLocked = false;
            }
        }
    }

    /***
     * Método que detecta si entra otro collider, 
     * si entra el jugador revisa qué tipo de puerta es, 
     * qué llaves tiene el jugador para evaluar si se puede
     * abrir o no, notifica por medio de un mensaje en la interfaz
     * el estado de la puerta al jugador.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (playerScript.llaves > 0 && isLocked)
            {
                Avisos.text = "Presiona Z para abrir esta puerta";
            } 
            if (playerScript.llaves <= 0 && isLocked)
            {
                Avisos.text = "Necesitas una llave para abrir esta puerta";
            }
            if (MasterDoor)
            {
                Avisos.text = "Esta puerta se abre con una llave maestra";
            }

        }
    }
    /***
     * Método que detecta cuando un collider sale, y de ser este el caso
     * se cierra la puerta.
     */
    private void OnTriggerExit(Collider other)
    {
        Avisos.text = " ";
        isPlayerNear = false; 
        openDoor = false;
        animator.SetBool("openDoor", false);
    }
}

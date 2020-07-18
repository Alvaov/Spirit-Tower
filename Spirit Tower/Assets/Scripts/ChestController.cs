using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

/***
 * Clase encargada de el manejo del comportamiento de los cofres, sus animaciones, su estado y el item que guarda.
 */
public class ChestController : MonoBehaviour
{
    public Animator animator;
    public GameObject chest;
    public bool openChest = false;
    public bool ChestOpened = false; 
    public Text Avisos;
    public GameObject mark;

    public GameObject tempObj;

    GameObject player;
    Player playerScript;


    [System.Serializable]
    public class Tesoros 
    {
        public GameObject item;
    }

    public List<Tesoros> Contenidos = new List<Tesoros>();
    
    /***
     * Función encargada de aplicar el efecto del objeto en el cofre al jugador, 
     * de manera que el contenido del cofre cumpla con su propósito.
     * Así sea una llave, un corazón o una llave maestra.
     * @params none
     * @return none
     */
    public void sacarObjeto()
    {
        for (int j = 0; j < Contenidos.Count; j++)
        {
            tempObj = Instantiate(Contenidos[j].item, transform.position, Quaternion.identity);
            if (Contenidos[j].item.name == "Heart")
            {
                playerScript.health += 1;
            }

            if (Contenidos[j].item.name == "masterKey")
            {
                playerScript.hasMasterKey = 1;
            }

            if (Contenidos[j].item.name == "extraHeart")
            {
                playerScript.extraHealth += 1;
            }

            if (Contenidos[j].item.name == "Key")
            {
                playerScript.llaves += 1;
            }

            if (Contenidos[j].item.name == "SpeedUp")
            {
                //**
            }

            
            //Mas comportamiento para objetos aqui

        }
    }
    /***
     * Método encargada de inicializar los valores necesarios para los objetos de esta clase,
     * se ejecuta únicamente en el primer frame.
     * @params none
     * @return none
     */
    void Start()
    {
        animator = GetComponent<Animator>();
        mark.SetActive(false);
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
    }

    /***
     * Método que se ejecuta una vez por frame, donde se revisa la interacción del usuario con el cofre
     * así como se eleva, gira y elimina el contenido de los cofres.
     * @params none
     * @return none
     */
    private void Update()
    {
        if (openChest == true & (Input.GetKeyDown(KeyCode.Z) & (ChestOpened == false))){
            Avisos.text = " ";
            animator.SetBool("openChest", true);
            GameObject player = GameObject.Find("Damian2.0");
            Player playerScript = player.GetComponent<Player>();
            playerScript.tesoros += 1;
            sacarObjeto();
            ChestOpened = true;
        }

        if (ChestOpened == true)
        {
            mark.SetActive(true);
        }

        if(tempObj != null) { 
            tempObj.transform.Translate(transform.up * Time.deltaTime);
            tempObj.transform.Rotate(new Vector3(0, Time.deltaTime * 100, 0));
            if (tempObj.transform.position.y > 6.0f)
            {
                Destroy(tempObj);
            }
        }

    }

    /***
     * Método activado cuando un objeto entra en el collider definido, en caso de que el oobjeto
     * sea el jugador mismo, este muestra un texto con el aviso de la acción que debe hacer el jugador 
     * para abrir el cofre.
     * @params none
     * @return none
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            openChest = true;
            Avisos.text = "Presiona Z para abrir el cofre";
            if (ChestOpened == true)
            {
                Avisos.text = "Ya has abierto este cofre";
            }

        }
    }

    /***
     * Método activado cuando un objeto entra en el collider definido, 
     * este reinicia el texto con el aviso de la acción que debe hacer el jugador 
     * para abrir el cofre eliminandola de la vista del jugador.
     * @params none
     * @return none
     */
    private void OnTriggerExit(Collider other)
    {
        Avisos.text = " ";
        openChest = false;
        animator.SetBool("openChest", false);
    }
}






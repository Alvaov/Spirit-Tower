using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/***
 * Es la clase que administra la interacción del usuario con el menú de inicio del juego, 
 * dicha interfaz permite agregar un nombre así como conectarse al servidor para poder iniciar
 * el juego de manera apropiada.
 */
public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public InputField userNameField;
    public AudioSource clip;
    
    /***
     * Es la función con la cual se crea el objeto, previo a cualquier frame, 
     * es la encargada de guardar una referencia a sí misma en la variable
     * llamada instancia.
     */
    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    /***
     * Es el método que se llama al seleccionar el botón de conectar en la interfaz, 
     * es la encargada de impedir que se presione el botón más de una vez así como
     * también es el encargado de indicarle al cliente que se conecte con el servidor.
     */
    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        userNameField.interactable = false;
        Client.instance.ConnectToServer();
        Destroy(clip);
    }
}

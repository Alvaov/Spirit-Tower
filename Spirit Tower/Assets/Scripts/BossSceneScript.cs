using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * Clase encargada de avisar cuando el jugador entra en el escenario de la batalla final
 * para así comenzar a cargar los elementos necesarios para esta
 */
public class BossSceneScript : MonoBehaviour
{

    BossScript boss;

    // Start is called before the first frame update
    /***
     * Método encargado de obtener la referencia al script del boss
     * para activar su funcionamiento general. 
     * Se ejecuta únicamente en le primer frame.
     */
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
    }

    /***
     * Método que detecta colisiones y en caso de ser el jugador
     * activa el jefe final en todas sus funciones.
     */
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entra");
        if (other.gameObject.CompareTag("Player"))
        {
            boss.active = true;
        }
    }

    /***
     * Método que detecta colisiones y en caso de ser el jugador
     * desactiva todas las funciones del jefe final.
     */
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boss.active = false;
        }
    }

    // Update is called once per frame
    /***
     * Función que se ejecuta una vez por frame
     */
    void Update()
    {
        
    }
}

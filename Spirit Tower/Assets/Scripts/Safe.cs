using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * Clase utilizada por la zona designada como zona segura donde el jugador se puede resguardar de los espectros.
 */
public class Safe : MonoBehaviour
{
    public static bool safe = false;
    /***
     * Método que se ejecuta en el primer frame
     */
    void Start()
    {

    }

    /***
     * Método que detecta colisiones con otro collider, en caso de que el collider que entre
     * sea del jugador se le envía al server que el jugador entró en la zona segura
     * y así se ejecuta el procedimiento implicado para el retroceso de los espectros
     * por backtracking.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpectrumMovement.detected = false;
            safe = true;
            Client.instance.tcp.SendData("0:Safe:PlayerSafe:0000:");
        }
    }

    /***
     * Método que detecta cuando un objeto o collider sale del este, 
     * cambia el estado del jugador regresando la variable safe a falso.
     */
    private void OnTriggerExit(Collider other)
    {
        safe = false;
    }
}

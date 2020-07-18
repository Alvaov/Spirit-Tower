using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * Clase encargada de administras el ataque de los espectros.
 * Se comunica con el servidor y solicita que se ejecute la
 * acción respectiva en el jugador.
 */
public class SpectrumPunch : MonoBehaviour
{
    // Start is called before the first frame update
    /***
     * Método que detecta colisiones y en caso de ser el jugador
     * envía un mensaje al servidor para que se le notifique
     * que un espectro lo golpeó.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Player.defender)
            {
                Client.instance.tcp.SendData("0:Spectrum:Attack::");
            }
        }
    }
}

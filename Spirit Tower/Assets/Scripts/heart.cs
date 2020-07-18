using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

/***
 * Clase que maneja los corazones en 3D que ve el jugador al romper un jarrón
 * permite que cuando el jugador lo toco se agrege a su vida actual y así recupere
 * salud.
 */
public class heart : MonoBehaviour
{
    /***
     * Método que verifica si ocurrió un choque de collider, en caso 
     * de que el choque haya sido contra un jugador este se le suma a la vida
     * actual.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.Find("Damian2.0");
            Player playerScript = player.GetComponent<Player>();         
            playerScript.health += 1;
            Destroy(gameObject);
        }
    }
}

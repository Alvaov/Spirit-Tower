using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase encargada de administrar el comportamiento de la espada, 
 * guarda una referencia a la mano derecha el jugador para configurarse como su hijo
 */
public class SwordScript : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject player;
    // Start is called before the first frame update
    /***
     * Función que se ejecuta en el primer frame, encuentra al jugador y a la mano derecha
     * de este para poder asignarla como elemento padre.
     */
    void Start()
    {
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    /***
     * Método que detecta colisiones con los colliders del juego, en caso de ser el
     * correspondiente con el jugador confiruga como padre la mano derecha del jugador y
     * realiza corrección de la posición.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent = rightHand.transform;
            transform.localPosition = new Vector3(0, 0.003f, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase que comprende el funcionamiento del escudo,
 * permite que el jugador adquiera el escudo y de esta forma 
 * lo conserve para el resto de la aventura.
 */
public class ShieldScript : MonoBehaviour
{
    public GameObject leftHand;
    private bool onHand = false;
    // Start is called before the first frame update
    /***
     * Método que se ejecuta en el primer frame únicamente, encuentra el elemento de la mano izquierda
     * del jugador.
     */
    void Start()
    {
        leftHand = GameObject.FindGameObjectWithTag("LeftHand");
    }

    /***
     * Método que detecta colisiones y en caso de ser el jugador configura la mano izquierda como
     * el padre del escudo para así añadirlo al movimiento general del jugador.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent = leftHand.transform;
            transform.localPosition = new Vector3(0.0029f, 0.0147f, 0.0031f);
            onHand = true;
        }
    }

    /***
     * Método que se ejecuta una vez cada frame
     */
    void Update()
    {
        if (onHand)
        {
            Vector3 direction = (transform.position - leftHand.transform.position).normalized;
            Quaternion target = Quaternion.LookRotation(new Vector3(direction.x, direction.y, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * 10);
        }
    }

}

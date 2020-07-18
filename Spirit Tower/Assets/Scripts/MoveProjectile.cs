using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase encargada de controlar el movimiento
 * de los proyectiles de fuego que dispara
 * el espectro rojo cuando el jugador es detectado.
 */
public class MoveProjectile : MonoBehaviour
{

    public int speed = 10;
    public int rotationSpeed = 10;
    // Start is called before the first frame update

    /***
     * Método que se ejecuta en el primer frame únicamente.
     */
    void Start()
    {
        
    }

    /***
     * Método que detecta colisiones y en caso de ser el jugador envía el un daño de valor
     * 1 al servidor para que se le descuente esta cantidad en la vida que posea actualmente
     * en caso de chocar con el escudo o con otro collider se destruye
     * @params collider other
     */
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Client.instance.tcp.SendData("0:Player:Damage:1:");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        else 
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    /***
     * Método que hace que el proyectil avance
     * a una velocidad asignada y
     * que rote a un grado asignado
     * se ejecuta cada frame
     */
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
        transform.Rotate(0,0,2);
        if(Time.frameCount % 500 == 0)
        {
            Destroy(gameObject);
        }
    }
}

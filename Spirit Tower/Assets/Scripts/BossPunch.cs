using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/***
 * Clase asignada al arma del jefe final donde si logra golpear al jugador envía un mensaje al servidor para que este actualice la vida del jugador
 */
public class BossPunch : MonoBehaviour
{
    BossScript boss;


    // Start is called before the first frame update
    /***
     * Método que se ejecuta al instanciarse el el objeto del arma del jefe final, obtiene el script del jefe final
     */
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
    }

    /***
     *  Método utilziado para poder verificar si la colisión se dio mientras el jefe estaba atacando así validar si el jugador no se estaba defendiendo
     *  y enviar el mensaje respectivo al servidor
    */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Player.defender && boss.attack)
            {
                Client.instance.tcp.SendData("0:Boss:Attack::");
            }
        }
    }

    // Update is called once per frame
    /***
     * Función que se llama una vez por frame.
    */
    void Update()
    {
        
    }
}

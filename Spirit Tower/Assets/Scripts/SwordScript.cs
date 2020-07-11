using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent = rightHand.transform;
            transform.localPosition = new Vector3(0, 0.003f, 0);
        }
        if (other.gameObject.CompareTag("Spectrum"))
        {
            if (Player.atacar)
            {
                //Client.instance.tcp.SendData("Enviar id y tipo de enemigo, uno de daño");
            }
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            if (Player.atacar)
            {
                //Client.instance.tcp.SendData("se elimina ese enemigo, se le envía el server para más pts");
            }
        }
    }
}

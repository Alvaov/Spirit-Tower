using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPunch : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Player.defender)
            {
                Client.instance.tcp.SendData("0:Boss:Attack::");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

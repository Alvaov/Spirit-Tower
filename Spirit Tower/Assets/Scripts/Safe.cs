using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Safe : MonoBehaviour
{
    // Start is called before the first frame update
    public static bool safe = false;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SpectrumMovement.detected = false;
            safe = true;
            Client.instance.tcp.SendData("0:Safe:PlayerSafe:0000:");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        safe = false;
    }
}

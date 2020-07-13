using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectrumPunch : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Client.instance.tcp.SendData("0:Spectrum:Attack::");
        }
    }
}

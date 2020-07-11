using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameObject player = GameObject.Find("Player1");
            Player playerScript = player.GetComponent<Player>();         
            playerScript.health += 1;
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    private void Start()
    {
        GameObject player = GameObject.Find("Damian2.0");
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
        player.transform.position = spawn.transform.position;
    }
}

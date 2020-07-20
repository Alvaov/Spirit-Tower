using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPosition : MonoBehaviour
{
    public bool created = false;

    public void OnTriggerEnter(Collider other)
    {
        if (!created)
        {
            Debug.Log("collided");
            created = true;
            Grid.instance.CreateGrid();
            Grid.getGridWalls();
            Client.instance.createBoss();
        }
    }

    private void Start()
    {
        GameObject player = GameObject.Find("Damian2.0");
        GameObject spawn = GameObject.FindGameObjectWithTag("Respawn");
        player.transform.position = spawn.transform.position;
    }
}

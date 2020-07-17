using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    bool done = false;
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && done == false){
            done = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Grid.instance.CreateGrid();
            Grid.getGridWalls();
        }
    }
}


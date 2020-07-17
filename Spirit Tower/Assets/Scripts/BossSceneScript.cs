using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSceneScript : MonoBehaviour
{

    BossScript boss;

    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entra");
        if (other.gameObject.CompareTag("Player"))
        {
            boss.active = true;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            boss.active = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

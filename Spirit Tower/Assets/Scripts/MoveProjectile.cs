using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveProjectile : MonoBehaviour
{

    public int speed = 10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Choqué");
        if (other.gameObject.CompareTag("Player"))
        {
            Client.instance.tcp.SendData("0:Player:Damage:1");
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Pito");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}

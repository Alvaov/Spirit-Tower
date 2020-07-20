using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lampScript : MonoBehaviour
{

    public GameObject cadera;
    public GameObject player;
    private bool onHand = false;

    // Start is called before the first frame update
    void Start()
    {
        cadera = GameObject.FindGameObjectWithTag("LeftHip");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {

            transform.parent = cadera.transform;
            transform.localPosition = new Vector3(-0.0047f, 0.0047f, 0.0007f);
            onHand = true;
        }
    }
}

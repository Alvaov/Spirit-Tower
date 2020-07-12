using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject rightHand;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            transform.parent = rightHand.transform;
            transform.localPosition = new Vector3(0, 0.003f, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordScript : MonoBehaviour
{
    public GameObject rightHand;
    public bool handed;
    // Start is called before the first frame update
    void Start()
    {
        handed = false;
        rightHand = GameObject.FindGameObjectWithTag("RightHand");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            handed = true;
        }
    }

    void Update()
    {
        if (handed)
        {
            transform.parent = rightHand.transform;
            transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public Animator animator;
    public GameObject door;
    public bool openDoor = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            openDoor = true;
            animator.SetBool("openDoor",true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        openDoor = false;
        animator.SetBool("openDoor", false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public Animator animator;
    public GameObject chest;
    public bool openChest = false;
    public bool ChestOpened = false; 
    public Text Avisos;

    void Start()
    {
        animator = GetComponent<Animator>();
    }



    private void Update()
    {
        if (openChest == true & (Input.GetKeyDown(KeyCode.Z))){
            animator.SetBool("openChest", true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            openChest = true;
            ChestOpened = true; 
            Avisos.text = "Presiona Z para abrir el cofre";

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Avisos.text = " ";
        openChest = false;
        animator.SetBool("openChest", false);
    }
}






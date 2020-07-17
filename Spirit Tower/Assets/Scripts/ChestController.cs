﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class ChestController : MonoBehaviour
{
    public Animator animator;
    public GameObject chest;
    public bool openChest = false;
    public bool ChestOpened = false; 
    public Text Avisos;
    public GameObject mark;

    public GameObject tempObj;

    GameObject player;
    Player playerScript;


    [System.Serializable]
    public class Tesoros 
    {
        public GameObject item;
    }

    public List<Tesoros> Contenidos = new List<Tesoros>();
    
    public void sacarObjeto()
    {
        for (int j = 0; j < Contenidos.Count; j++)
        {
            tempObj = Instantiate(Contenidos[j].item, transform.position, Quaternion.identity);
            if (Contenidos[j].item.name == "Heart")
            {
                playerScript.health += 1;
            }

            if (Contenidos[j].item.name == "masterKey")
            {
                playerScript.hasMasterKey = 1;
            }

            if (Contenidos[j].item.name == "extraHeart")
            {
                playerScript.extraHealth += 1;
            }

            if (Contenidos[j].item.name == "Key")
            {
                playerScript.llaves += 1;
            }
        }
    }
        
    void Start()
    {
        animator = GetComponent<Animator>();
        mark.SetActive(false);
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
    }
   private void Update()
    {
        if (openChest == true & (Input.GetKeyDown(KeyCode.Z) & (ChestOpened == false))){
            Avisos.text = " ";
            animator.SetBool("openChest", true);
            playerScript.tesoros += 1;
            playerScript.monedas += 500;
            sacarObjeto();
            ChestOpened = true;
        }

        if (ChestOpened == true)
        {
            mark.SetActive(true);
        }

        if(tempObj != null) { 
            tempObj.transform.Translate(transform.up * Time.deltaTime);
            tempObj.transform.Rotate(new Vector3(0, Time.deltaTime * 100, 0));
            if (tempObj.transform.position.y > 6.0f)
            {
                Destroy(tempObj);
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            openChest = true;
            Avisos.text = "Presiona Z para abrir el cofre";
            if (ChestOpened == true)
            {
                Avisos.text = "Ya has abierto este cofre";
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        Avisos.text = " ";
        openChest = false;
        animator.SetBool("openChest", false);
    }
}

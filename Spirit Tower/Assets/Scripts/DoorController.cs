using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorController : MonoBehaviour
{
    public Animator animator;
    public GameObject door;
    bool openDoor = false;
    bool isPlayerNear;

    public bool isLocked;
    public bool MasterDoor;
    public bool doorUnlocked;

    GameObject player;
    Player playerScript;

    public Text Avisos;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
    }

    private void Update()
    {
        if (isPlayerNear)
        {
            if (!isLocked || doorUnlocked)
            {
                openDoor = true;
                animator.SetBool("openDoor", true);
            }

            if (isLocked && Input.GetKeyDown(KeyCode.Z) && playerScript.llaves > 0 && !MasterDoor)
            {
                playerScript.llaves -= 1;
                openDoor = true;
                animator.SetBool("openDoor", true);
                isLocked = false;
            }

            if (MasterDoor && Input.GetKeyDown(KeyCode.Z) && playerScript.hasMasterKey == 1)
            {   
                openDoor = true;
                animator.SetBool("openDoor", true);
                doorUnlocked = true;
                playerScript.hasMasterKey -= 1;
                MasterDoor = false;
                isLocked = false;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            isPlayerNear = true;

            if (playerScript.llaves > 0 && isLocked)
            {
                Avisos.text = "Presiona Z para abrir esta puerta";
            } 
            if (playerScript.llaves <= 0 && isLocked)
            {
                Avisos.text = "Necesitas una llave para abrir esta puerta";
            }
            if (MasterDoor)
            {
                Avisos.text = "Esta puerta se abre con una llave maestra";
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        Avisos.text = " ";
        isPlayerNear = false; 
        openDoor = false;
        animator.SetBool("openDoor", false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class SpectrumMovement : MonoBehaviour
{
    //Movimiento
    private Vector3 movement;
    public float horizontal;
    public float vertical;
    public float gravity = -9.8f;
    public float speed = 120;
    public int stepPath = 0;

    //Rango de visión
    public float visionRadius;
    public float visionAngle = 160f;

    //Aspectos generales
    public GameObject player;
    public CharacterController spectrum;
    public string[] path;// = { "0,0", "0,0" };
    private Vector3 target;
    public float frameInterval;
    public int myId;
    public static bool detected = false;
    public bool localDetected = false;
    public bool attack = false;
    public bool addedToList = false;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        spectrum = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        visionRadius = 10;
        myId = Client.spectrumId;
        Client.spectrumId += 1;
        frameInterval = 10+(myId*2);
        //movement = Grid.instance.GetWorldPointFromAxes(14, 51);
    }

    // Update is called once per frame
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
            {
                if(Client.instance.spectrums.getValorEnIndice(i).myId == this.myId)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.spectrums.añadirElementos(this);
            }
            Client.instance.tcp.SendData(myId + ":Spectrum:New:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position)+":");
        }

        float distance = Vector3.Distance(player.transform.position, transform.position); // faster than Vector3.Distance

        if (Time.frameCount % frameInterval == 0)
        {
            if (distance < visionRadius)
            {
                checkVisualRange();
            }
            if(distance < 7 && detected)
            {
                attacking();
            }
            if(detected == true)
            {
                localDetected = true;
            }
            if(Safe.safe == true && localDetected == true)
            {
                localDetected = false;
                animator.SetInteger("action",0);
                animator.SetBool("perseguir",false);
                Client.instance.tcp.SendData(myId + ":Spectrum:Backtracking:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
            }
            else if (detected == true)
            {
                Client.instance.tcp.SendData(myId + ":Spectrum:Detected:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
            }  
        }
        walk();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (!checkVisualRange())
            {
                Client.instance.tcp.SendData(myId + ":Spectrum:Damage:1:");
            }
        }
    }


    bool checkVisualRange()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < visionAngle * 0.5f){    
            detected = true;
            animator.SetInteger("action", 1);
            animator.SetBool("perseguir", true);
            return true;
        }
        return false;
    }

    private void walk()
    {
        if (path.Length > 0)
        {
            try
            {
                if (stepPath == path.Length - 1)
                {
                    stepPath = 0;
                }
                string[] pos_grid = path[stepPath].Split(',');
                int x;
                int z;
                Int32.TryParse(pos_grid[0], out x);
                Int32.TryParse(pos_grid[1], out z);
                
                target = Grid.instance.GetWorldPointFromAxes(x, z);
                
                if (transform.position != target)
                {
                    transform.LookAt(player.transform);
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                }if(transform.position == target)
                {
                    stepPath++;
                }

            }
            catch
            {
                Debug.Log("Error convirtiendo string a entero");
            }
        }
    }


    void attacking()
    {
        StartCoroutine(AttackRoutine());
    }


    IEnumerator AttackRoutine()
    {
        animator.SetBool("atacar", true);
        animator.SetInteger("action", 2);
        yield return new WaitForSeconds(1);
        if (detected)
        {
            animator.SetInteger("action", 1);
            animator.SetBool("atacar", false);
        }
        if (!detected)
        {
            animator.SetInteger("action", 0);
            animator.SetBool("atacar", false);
        }
    }
}

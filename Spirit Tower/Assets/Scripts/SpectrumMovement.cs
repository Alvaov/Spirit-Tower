using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Cryptography;
using UnityEngine;

public class SpectrumMovement : MonoBehaviour
{
    //Movimiento
    private Vector3 movement;
    public float horizontal;
    public float vertical;
    public float gravity = -9.8f;
    public float speed = 90;

    //Rango de visión
    public float visionRadius;
    public float visionAngle = 160f;

    //Aspectos generales
    public GameObject player;
    public CharacterController spectrum;
    public string[] path;
    private Vector3 target;
    public float frameInterval;
    public int myId;
    public static bool detected = false;
    public bool attack = false;
    public bool addedToList = false;

    // Start is called before the first frame update
    void Start()
    {
        spectrum = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        frameInterval = 3;
        visionRadius = 10;
        myId = Client.spectrumId;
        Client.spectrumId += 1;
       
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

            if (detected == true)
            {
                Client.instance.tcp.SendData(myId + ":Spectrum:Detected:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
            }
            
        }
        walk();
    }

    void checkVisualRange()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < visionAngle * 0.5f){
            //RaycastHit hit;
            Debug.Log("Detectado");
            //if (Physics.Raycast(transform.position,direction.normalized, out hit, visionRadius))
            //{
                //Debug.Log("Detectado2");
                detected = true;
                
            //}
        }
    }

    private void walk()
    {
        if (path.Length > 0)
        {
            string[] pos_grid = path[path.Length - 1].Split(',');
            int x = int.Parse(pos_grid[0]);
            int z = int.Parse(pos_grid[1]);
            target = Grid.instance.GetWorldPointFromAxes(x, z);
            if (transform.position != target){
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else{

            }
        }
    }

}

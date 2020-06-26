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
    public float speed = 10;

    //Rango de visión
    public float visionRadius;
    public float visionAngle = 160f;

    //Aspectos generales
    public GameObject player;
    public CharacterController spectrum;
    public string path;
    public float frameInterval;
    public int myId;
    public static bool detected = false;
    public bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        spectrum = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        frameInterval = 3;
        visionRadius = 10;
        myId = Client.spectrumId;
        Client.spectrumId += 1;
        Client.instance.tcp.SendData(myId + ":Spectrum:New:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) +","+myId+ ":");
        movement = Grid.instance.GetWorldPointFromAxes(14, 51) * speed * Time.deltaTime;
        spectrum.Move(movement);
    }

    // Update is called once per frame
    void Update()
    {

        
        float distance = Vector3.Distance(player.transform.position,transform.position); // faster than Vector3.Distance
        
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

        //Vector3 movement = Grid.instance.GetWorldPointFromAxes(14,51) * speed * Time.deltaTime;
        //movement = Vector3.ClampMagnitude(movement, 1);
        //spectrum.Move(movement);
        //Debug.Log(spectrum.transform.position);
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
    }

}

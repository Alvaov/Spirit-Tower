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
    public bool detected = false;

    //Aspectos generales
    public GameObject player;
    public CharacterController spectrum;
    public string path;
    public float frameInterval;
    public int myId;

    // Start is called before the first frame update
    void Start()
    {
        spectrum = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        frameInterval = 3;
        visionRadius = 20;
        myId = Client.spectrumId;
        Client.spectrumId += 1;

    }

    // Update is called once per frame
    void Update()
    {

        
        float distance = Vector3.Distance(player.transform.position,transform.position); // faster than Vector3.Distance
        if (distance < visionRadius)
        {
            if (Time.frameCount % frameInterval == 0)
            {
                checkVisualRange();
            }
        }
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

    }

    void checkVisualRange()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < visionAngle * 0.5f){
            RaycastHit hit;
            if(Physics.Raycast(transform.position,direction.normalized, out hit, visionRadius))
            {
                detected = true;
                Client.instance.tcp.SendData(myId+"Spectrum:detectado::");
                
            }
        }
    }

}

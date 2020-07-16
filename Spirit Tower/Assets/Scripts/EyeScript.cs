using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    //Aspectos generales
    public GameObject player;
    public AudioSource alert;
    public CharacterController spectralEye;
    public float frameInterval;
    public bool addedToList = false;
    public int id;

    //Rango de visión
    public float visionRadius;
    public float visionAngle = 160f;

    // Start is called before the first frame update
    void Start()
    {
        spectralEye = GetComponent<CharacterController>();
        player = GameObject.FindGameObjectWithTag("Player");
        alert = GetComponent<AudioSource>();
        frameInterval = 60+(id*12)+id;
        visionRadius = 20;
        id = Client.eyeId;
        Client.eyeId += 1;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (Player.atacar)
            {
                    Client.instance.tcp.SendData(id + ":Eye:Damage:1:");
            }
        }
        if (other.gameObject.CompareTag("Player"))
        {
            alert.Play();
            SpectrumMovement.detected = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.spectralEyes.getTamaño(); i++)
            {
                if (Client.instance.spectralEyes.getValorEnIndice(i).id == this.id)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.spectralEyes.añadirElementos(this);
            }
            Client.instance.tcp.SendData(id + ":Eye:New:" + Grid.instance.GetAxesFromWorldPoint(spectralEye.transform.position) + ":");
        }


    }
}

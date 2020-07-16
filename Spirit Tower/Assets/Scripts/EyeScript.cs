using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeScript : MonoBehaviour
{
    //Aspectos generales
    public GameObject player;
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

        float distance = Vector3.Distance(player.transform.position, transform.position); // faster than Vector3.Distance

        if (Time.frameCount % frameInterval == 0)
        {
            if (distance < visionRadius)
            {
                checkVisualRange();
            }
        }
    }


    bool checkVisualRange()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < visionAngle * 0.5f)
        {
            SpectrumMovement.detected = true;
            return true;
        }
        return false;
    }
}

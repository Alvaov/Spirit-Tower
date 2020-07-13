using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;


public class RatScript : MonoBehaviour
{

    public CharacterController rat;
    public int id;
    public int speed = 10;
    private Vector3 target;
    public float frameInterval = 20;
    public bool addedToList = false;
    public string[] path;


    // Start is called before the first frame update
    void Start()
    {
        rat = GetComponent<CharacterController>();
        id = Client.ratId;
        Client.ratId += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.rats.getTamaño(); i++)
            {
                if (Client.instance.rats.getValorEnIndice(i).id == this.id)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.rats.añadirElementos(this);
            }
            Client.instance.tcp.SendData(id + ":Rat:New:" + Grid.instance.GetAxesFromWorldPoint(rat.transform.position) + ":");
        }

        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData(id + ":Rat:Move:" + Grid.instance.GetAxesFromWorldPoint(rat.transform.position) + ":");
        }

        Walk();
    }


    void Walk()
    {
        string[] pos_grid = path[0].Split(',');
        int x;
        int z;
        Int32.TryParse(pos_grid[0], out x);
        Int32.TryParse(pos_grid[1], out z);

        target = Grid.instance.GetWorldPointFromAxes(x, z);

        if (transform.position != target)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        }
    }
}

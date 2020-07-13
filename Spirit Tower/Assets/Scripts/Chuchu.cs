using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class Chuchu : MonoBehaviour
{

    public CharacterController chuchu;
    public int id;
    public int speed = 1;
    public int stepPath = 0;
    private Vector3 target;
    public float frameInterval = 10;
    public bool addedToList = false;
    public string[] path;

    // Start is called before the first frame update
    void Start()
    {
        chuchu = GetComponent<CharacterController>();
        id = Client.chuchuId;
        Client.chuchuId += 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.chuchus.getTamaño(); i++)
            {
                if (Client.instance.chuchus.getValorEnIndice(i).id == this.id)
                {
                    exist = true;

                }
            }
            if (!exist)
            {
                Client.instance.chuchus.añadirElementos(this);
            }
            Client.instance.tcp.SendData(id + ":Chuchu:New:" + Grid.instance.GetAxesFromWorldPoint(chuchu.transform.position) + ":");

        }
        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData(id + ":Chuchu:Move:" + Grid.instance.GetAxesFromWorldPoint(chuchu.transform.position) + ":");
        }

        Walk();
    }


    void Walk()
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
                    FaceTarget();
                    transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

                }
                if (transform.position == target)
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

    void FaceTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }
}

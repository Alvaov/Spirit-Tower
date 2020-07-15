using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public int frameInterval = 1200;

    public GameObject firePoint;
    public GameObject[] vfx;

    private GameObject effectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (SpectrumMovement.detected)
        {
            if(Time.frameCount%frameInterval == 0)
            {
                SpawnVFX();
            }
        }

    }


    void SpawnVFX()
    {
        GameObject vfx;
        if (firePoint != null)
        {
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }
}

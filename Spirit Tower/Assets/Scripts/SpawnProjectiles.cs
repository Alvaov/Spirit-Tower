using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnProjectiles : MonoBehaviour
{
    public int frameInterval = 5000;

    public GameObject firePoint;
    public GameObject[] vfx;
    public AudioSource fireballCreation;
    private GameObject effectToSpawn;

    // Start is called before the first frame update
    void Start()
    {
        fireballCreation = GetComponent<AudioSource>();
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
            fireballCreation.Play();
            vfx = Instantiate(effectToSpawn, firePoint.transform.position, Quaternion.identity);
            vfx.transform.rotation = firePoint.transform.rotation;

        }
        else
        {
            Debug.Log("No Fire Point");
        }
    }
}

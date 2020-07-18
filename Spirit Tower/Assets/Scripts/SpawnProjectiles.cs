using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase que maneja la creación de los proyectiles
 * que disparan los espectros rojos. Los instancia de manera que
 * conserven la rotación del espectro y con intervalo de 5000 frames de diferencia.
 */
public class SpawnProjectiles : MonoBehaviour
{
    public int frameInterval = 5000;

    public GameObject firePoint;
    public GameObject[] vfx;
    public AudioSource fireballCreation;
    private GameObject effectToSpawn;

    // Start is called before the first frame update
    /***
     * Método que se ejecuta únicamente en el primer frame. 
     * Reproduce el sonido de creación del proyectil e 
     * instancia el array de efectos que posee la clase.
     */
    void Start()
    {
        fireballCreation = GetComponent<AudioSource>();
        effectToSpawn = vfx[0];
    }

    // Update is called once per frame
    /***
     * Método que se ejecuta una vez por frame,
     * instancia los proyectiles según el frameInterval establecido
     */
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

    /***
     * Método encargado de la creación de proyectiles y 
     * de su respectivo punto de instanciación para 
     * asegurar que continúe el camino esperado.
     */
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/***
 * Clase utilizada para el control del movimiento de la cámara encargada de seguir al jugador
 */
public class camaraJugador : MonoBehaviour
{
    public Vector3 offset;
    public GameObject target;
    [Range(0, 1)] public float lerpValue = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /***
     * Actualiza el movimiento de la cámara para que esta siga al jugador
     */
    void LateUpdate()
    {

        transform.position = Vector3.Lerp(
        transform.position,
        target.transform.position + offset,
        lerpValue);
    }
}


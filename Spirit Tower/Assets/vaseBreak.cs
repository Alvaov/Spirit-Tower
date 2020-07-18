using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
/***
 * Clase que maneja las acciones del jarrón, permite que este se rompa por acción del jugador, así como también da su contenido al jugador una vez roto.
 */
public class vaseBreak : MonoBehaviour
{
    public AudioSource breakingSound;
    public AudioClip clip;
    public GameObject contenido;
    public GameObject item;
    
    /***
     * Método que permite detectar si la espada golpeó al jarrón mientras el jugador estaba atacando, de ser el caso se destruye el jarrón
     * y se reproduce un sonido para crear la sensación de que se rompió.
     * @params other
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (Player.atacar)
            {
                item = Instantiate(contenido, transform.position, Quaternion.identity);
                item.transform.Translate(transform.up * 3);
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(gameObject);
            }
          
        }
    }
}

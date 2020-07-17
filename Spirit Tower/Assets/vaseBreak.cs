using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class vaseBreak : MonoBehaviour
{
    public AudioSource breakingSound;
    public AudioClip clip;
    public GameObject contenido;
    public GameObject item;
    GameObject player;
    Player playerScript;


    private void Start()
    {
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            if (Player.atacar)
            {
                playerScript.monedas += 50;
                item = Instantiate(contenido, transform.position, Quaternion.identity);
                item.transform.Translate(transform.up * 3);
                AudioSource.PlayClipAtPoint(clip, transform.position);
                Destroy(gameObject);
            }
          
        }
    }
}

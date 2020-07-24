using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class screamingStatue : MonoBehaviour
{
    public AudioSource clip;
    private void OnTriggerEnter(Collider other)
    {
        clip.Play();
    }
}

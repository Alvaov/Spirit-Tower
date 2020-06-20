﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    void LateUpdate()
    {

        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position + offset,
            lerpValue);
    }
}

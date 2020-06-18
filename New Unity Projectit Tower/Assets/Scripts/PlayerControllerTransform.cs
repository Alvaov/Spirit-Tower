using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour{

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        float hAxis = Input.GetAxis("Horizontal");
        float vAxis = Input.GetAxis("Vertical");

        //Vector3 movement = new Vector3(hAxis, 0, vAxis) * speed * Time.deltaTime;

        transform.Translate(hAxis * Time.deltaTime, 0f, vAxis * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    [SerializeField]
    Transform[] wayPoints;
    int currentWayPoint = 0;
    Rigidbody rigidbody;
    [SerializeField]
    float moveSpeed = 5;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

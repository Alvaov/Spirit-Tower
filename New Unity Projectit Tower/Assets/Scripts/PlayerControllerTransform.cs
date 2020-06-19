using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour{
    private Vector3 movement;
    private float horizontal;
    private float vertical;
    private float speed = 10;
    public CharacterController player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
    }
    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        
        movement = new Vector3(horizontal, 0f, vertical)* speed *Time.deltaTime;
        movement = Vector3.ClampMagnitude(movement, 1);
        player.Move(movement);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour{

    
    public float rightInput;
    public float forwardInput;
    
    public CharacterController player;
    private Vector3 movement;
    public float speed = 10;
    public Animator animator;
    public float gravity = -9.8f;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
           
            rightInput = Input.GetAxis("Horizontal");
            forwardInput = Input.GetAxis("Vertical");

            animator.SetFloat("vertical", Input.GetAxis("Vertical"));
            animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));
            movement = new Vector3(rightInput, 0f, forwardInput) * speed * Time.deltaTime;
            movement = Vector3.ClampMagnitude(movement, 1);
            player.Move(movement);
        }
        else
        {
            movement = new Vector3(0, gravity, 0) * Time.deltaTime;
            player.Move(movement);
        }
    }
    






}




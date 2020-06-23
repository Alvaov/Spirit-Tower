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
    public CameraFollow camera;

    private Vector3 velocity; 

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


            /*Vector3 camFwd = camera.transform.forward;
            Vector3 camRight = camera.transform.right;

            Vector3 targetLocation = vertical * camera.transform.forward;
            targetLocation += horizontal * camera.transform.right;

            Vector3 velocity = (targetLocation - transform.position).normalized;*/



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
    

   /* private void Update()
    {
        transform.Translate(velocity);
    }*/



  /*  void MovementInput(float forward, float right)
    {
        forwardInput = forward;
        rightInput = right;


        Vector3 camFwd = camera.transform.forward;
        Vector3 camRight = camera.transform.right;

        Vector3 targetLocation = forward * camera.transform.forward;
        targetLocation += right * camera.transform.right;

        velocity = (targetLocation - transform.position).normalized;
    }*/


    



}




using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;

public class PlayerControllerTransform : MonoBehaviour{
    private Vector3 movement;
    public float horizontal;
    public float vertical;
    public float gravity = -9.8f;
    public float speed = 10;
    public CharacterController player;
    public Animator animator;
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
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            animator.SetFloat("vertical", Input.GetAxis("Vertical"));
            animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));
            movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
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

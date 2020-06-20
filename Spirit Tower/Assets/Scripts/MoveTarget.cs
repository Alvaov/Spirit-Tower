using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    private Vector3 movement;
    private float horizontal;
    private float vertical;
    private float speed = 10;
    public CharacterController player;

    [SerializeField]
    Transform[] wayPoints;
    int currentWayPoint = 0;
    Rigidbody rigidbody;
    [SerializeField]
    float moveSpeed = 5;
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
        movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
        movement = Vector3.ClampMagnitude(movement, 1);
        player.Move(movement);
        string msg = ":Player:Posistion:x=" + transform.position.x + ",y=" + transform.position.y + ",z=" + transform.position.z;
        Client.instance.Send_Data(msg.Length+msg);
    }

}

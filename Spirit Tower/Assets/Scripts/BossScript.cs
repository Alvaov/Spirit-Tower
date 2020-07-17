﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class BossScript : MonoBehaviour
{
    public string[] movementPath;
    public bool created = false;
    public int stepPath;
    public Vector3 target;
    public float speed;
    public int life;
    public bool active = false;
    public int action;
    public GameObject player;
    public int actualPhase = 0;
    public CharacterController boss;
    public float frameInterval;
    Animator animator;

    public bool attack = false;

    // Start is called before the first frame update
    void Start()
    {
        speed = 10;
        stepPath = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        frameInterval = 60;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!created)
        {
            //Client.instance.tcp.SendData("0:Boss:New::");
        }

        if (active)
        {
            transform.LookAt(player.transform.position);
        }
        walk();
    }


    private void walk()
    {
        if (movementPath.Length > 0)
        {
            try
            {
                if (stepPath == movementPath.Length - 1)
                {
                    attacking();
                    stepPath = 0;
                    stopMoving();
                }
                string[] pos_grid = movementPath[stepPath].Split(',');
                string x = pos_grid[0];
                string y = pos_grid[1];
                int posX = Int32.Parse(x);
                int posZ = Int32.Parse(y);

                target = Grid.instance.GetWorldPointFromAxes(posX, posZ);
                target.y = 16.44f;

                if (transform.position != target)
                {
                    if (actualPhase == 0)
                    {
                        transform.LookAt(player.transform.position);
                        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }else if(actualPhase == 1)
                    {
                        FaceTarget();
                        transform.position = Vector3.MoveTowards(transform.position, target, speed*3 * Time.deltaTime);
                    }

                }

                if (transform.position == target)
                {
                    stepPath++;
                }
            }
            catch
            {
                Debug.Log("Error convirtiendo string a entero");
            }
        }
    }

    void FaceTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }

    vois stopMoving()
    {
        StartCoroutine(StopRoutine());
    }

    void attacking()
    {
        StartCoroutine(AttackRoutine());
    }


    IEnumerator AttackRoutine()
    {
        Vector3 target = player.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * 2 * Time.deltaTime);
        
        yield return new WaitForSeconds(1);
        animator.SetBool("atacar", true);
        animator.SetInteger("action", 2);
        animator.SetInteger("action", 1);
        animator.SetBool("atacar", false);
        
         animator.SetInteger("action", 0);
         animator.SetBool("atacar", false);
    }


    IEnumerator StopRoutine()
    {

    }
}

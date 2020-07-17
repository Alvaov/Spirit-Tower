using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossScript : MonoBehaviour
{

    public int life = 9;
    public int action;
    public CharacterController boss;
    public float frameInterval;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        boss = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        frameInterval = 30;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

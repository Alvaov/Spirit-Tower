﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPunch : MonoBehaviour
{
    BossScript boss;


    // Start is called before the first frame update
    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<BossScript>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!Player.defender && boss.attack)
            {
                Client.instance.tcp.SendData("0:Boss:Attack::");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

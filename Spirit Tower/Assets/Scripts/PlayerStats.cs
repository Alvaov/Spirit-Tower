using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class PlayerStats : MonoBehaviour
{
    public int health;
    public int numOfHearts;

    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;


    private void Update()
    {
        if (health <= 0)
        {  
            health = 0;
            //Morir :c
        }


        if (health > numOfHearts)
        {
            health = numOfHearts;
        }


        //Rebajar vida, por ahora
        if (Input.GetKeyDown(KeyCode.Space))
        {
            health--;
            // string msg = "Player:Health:" + health + ":";
            Client.instance.Send_Data("0:" + "Player:Health:" + health + ":");
        }


        //Aumentar vida, por ahora 
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            health++;
            // string msg = "Player:Health:" + health + ":";
            Client.instance.Send_Data("0:" + "Player:Health:" + health + ":");
        }


        for (int i = 0; i < hearts.Length; i++)
        {
         
            if(i < health)
            {
                hearts[i].sprite = fullHeart;
            } 
            else
            {
                hearts[i].sprite = emptyHeart;
            }

            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }

}

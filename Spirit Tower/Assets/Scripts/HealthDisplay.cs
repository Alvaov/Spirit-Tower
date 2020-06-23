using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Transactions;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{

    private int health = 5;
    public Text healthText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        healthText.text = "Salud: " + health;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            health--;
            string msg = "Salud restante: " + health;
            Client.instance.Send_Data(msg.Length + msg);
        }

    }
}

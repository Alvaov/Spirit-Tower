using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
   
    void Awake()
    {
        GameObject player = GameObject.Find("Damian2.0");
        GameObject clientManger = GameObject.Find("ClientManager");
        //GameObject playerInfo = GameObject.Find("PlayerInfo");
        if (player.gameObject.CompareTag("Player") && clientManger.gameObject.CompareTag("Keep")){
            //DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(player);
            DontDestroyOnLoad(clientManger);

        }

        /*if (playerInfo.gameObject.CompareTag("Destroy"))
        {
            Destroy(this.gameObject);
        }*/

    }
}

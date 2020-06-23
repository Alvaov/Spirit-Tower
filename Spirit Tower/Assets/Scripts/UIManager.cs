using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.GlobalIllumination;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public GameObject startMenu;
    public InputField userNameField;

    public GameObject luz;
    public Text titulo;


    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    public void ConnectToServer()
    {
        startMenu.SetActive(false);
        titulo.gameObject.SetActive(false);
        userNameField.interactable = false;
        luz.SetActive (false);
        SceneManager.LoadScene("Floor", LoadSceneMode.Additive);
        Client.instance.ConnectToServer();
        Client.instance.tcp.SendData("k:l:l:");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    public GameObject A;
    bool done = false;
    void OnTriggerEnter(Collider other){
        if (other.gameObject.CompareTag("Player") && done == false){
            done = true;
            SpectrumMovement.detected = false;
            A = other.gameObject;
            while (Client.instance.spectralEyes.getTamaño() > 0)
            {
                Client.instance.spectralEyes.Eliminar(0);
            }

            while (Client.instance.rats.getTamaño() > 0)
            {
                Client.instance.rats.Eliminar(0);
            }
            while (Client.instance.chuchus.getTamaño() > 0)
            {
                Client.instance.chuchus.Eliminar(0);
            }

            while (Client.instance.spectrums.getTamaño() > 0)
            {
                Client.instance.spectrums.Eliminar(0);
            }
            Client.spectrumId = 0;
            Client.chuchuId = 0;
            Client.ratId = 0;
            Client.eyeId = 0;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Grid.instance.CreateGrid();
            Grid.getGridWalls();
            Debug.Log("Escena cargada");
        }
    }
}


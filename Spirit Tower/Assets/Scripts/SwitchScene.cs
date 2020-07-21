using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading;
using UnityEngine.UI;

public class SwitchScene : MonoBehaviour
{
    GameObject A;
    bool done = false;
    public string levelName;
    public int sceneIndex;
    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;
    void OnTriggerEnter(Collider other)
    {
        if (StartPosition.created){
            
            if (other.gameObject.CompareTag("Player") && done == false)
            {
                StartPosition.created = false;
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

                //SceneManager.LoadScene(sceneIndex);
                //SceneManager.LoadScene(levelName);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                LoadLevel(levelName);
                Debug.Log("Escena cargada");
            }
        }

        void LoadLevel(string levelName)
        {
            StartCoroutine(LoadAsynchronously(levelName));
        }

        IEnumerator LoadAsynchronously(string levelName)
        {
            loadingScreen.SetActive(true);
            // guarda el estado de la operacion actual
            AsyncOperation operation = SceneManager.LoadSceneAsync(levelName);
            operation.allowSceneActivation = false;
            
            while (operation.isDone == false)
            {
                //float progress = Mathf.Clamp(operation.progress / .9f);

                slider.value = operation.progress;
                if(operation.progress == 0.9f)
                {
                    slider.value = 20f;
                    operation.allowSceneActivation = true;
                    progressText.text = operation.progress * 100f + "%";
                }       
                yield return null;
            }
        }
    }
}

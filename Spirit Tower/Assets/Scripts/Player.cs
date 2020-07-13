using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour{

    //Movimiento
    private Vector3 movement;
    public float horizontal;
    public float vertical;
    public float gravity = -9.8f;
    public float speed = 10;
    int frameInterval = 10;
    public CharacterController player;
    public Animator animator;

    //Salud
    public int health;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    //Monedas y tesoros
    public int monedas;
    public Text monedasText;
    public int tesoros;
    public Text tesorosText;
    public int tesorosMAX; //Numero de cofres que existan en el nivel 
    public Text Avisos;


    public Text llavesText;
    public int llaves;
    public int llavesMAX;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //MOVIMIENTO
        if (player.isGrounded)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");
            animator.SetFloat("vertical", Input.GetAxis("Vertical"));
            animator.SetFloat("horizontal", Input.GetAxis("Horizontal"));
            movement = new Vector3(horizontal, 0f, vertical) * speed * Time.deltaTime;
            movement = Vector3.ClampMagnitude(movement, 1);
            player.Move(movement);
        }
        else
        {
            movement = new Vector3(0, gravity, 0) * Time.deltaTime;
            player.Move(movement);
        }

        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData("0:Player:Position:" + Grid.instance.GetAxesFromWorldPoint(player.transform.position)+":");
        }

        //SALUD
        if (health <= 0) {
            health = 0;
            //Morir :c
        }

        if (health > numOfHearts) {
            health = numOfHearts;
        }

        //TODO: recibir dano de los wendigos
        if (Input.GetKeyDown(KeyCode.Space)) {
            health--;
            // string msg = "Player:Health:" + health + ":";
            Client.instance.Send_Data("0:Player:Health:" + health + ":");
        }

        //TODO: Curarse  
        if (Input.GetKeyDown(KeyCode.Backspace)) {
            health++;
            // string msg = "Player:Health:" + health + ":";
            Client.instance.Send_Data("0:Player:Health:" + health + ":");
        }

        for (int i = 0; i < hearts.Length; i++){

            if (i < health){
                hearts[i].sprite = fullHeart;
            } else{
                hearts[i].sprite = emptyHeart;
            }
            if (i < numOfHearts){
                hearts[i].enabled = true;
            } else {
                hearts[i].enabled = false;
            }
        }

        //MONEDAS Y TESOROS
        monedasText.text = ":" + monedas;

        if (Input.GetKeyDown(KeyCode.C)){
            monedas++;
            Client.instance.Send_Data("0:Player:Coins:" + monedas + ":");
        }

        tesorosMAX = 4; //Depende del nivel
        tesorosText.text = ":" + tesoros + "/" + tesorosMAX;
        if (Input.GetKeyDown(KeyCode.T))
        {
            if(tesoros < tesorosMAX)
            {
                
            }
            Client.instance.Send_Data("0:Player:Treasures:" + tesoros + "/" + tesorosMAX + ":");
        }

        //LLaves
        llavesMAX = 4;
        llavesText.text = ":" + llaves + "/" + llavesMAX;

    }
}

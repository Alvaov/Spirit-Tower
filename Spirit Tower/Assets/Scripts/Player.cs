using System;
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
    float rotation = 0f;
    float rotSpeed = 80;
    int frameInterval = 15;
    public CharacterController player;
    public Animator animator;

    //Acciones
    public bool agacharse = false;
    public static bool atacar = false;
    public bool defender = false;

    //Salud
    public int health;
    int numOfHearts = 5;
    public Image[] hearts;
    public Sprite fullHeart;
    public Sprite emptyHeart;

    public Image[] extraHearts;
    public int extraHealth;
    public Sprite extraHeart;
    int numOfextraHearts = 5;

    public int DamageTaken;
    public bool ImDead;

    //Monedas y tesoros
    public int monedas;
    public Text monedasText;
    public int tesoros;
    public Text tesorosText;
    public int tesorosMAX; //Numero de cofres que existan en el nivel 
    public Text Avisos;


    public Text llavesText;
    public int llaves;
    public int hasMasterKey;

    //Variables para el server


    public int tesorosTemp = 0;
    public int vidaTemp = 5;
    public int monedasTemp = 0;


    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        tesoros = tesorosTemp;
        monedas = monedasTemp;

    }

    // Update is called once per frame
    void Update()
    {
        if (ImDead)
        {
            Time.timeScale = 0;
        }

        Movement();
        GetInput();

        if (Time.frameCount % frameInterval == 0)
        {
            Client.instance.tcp.SendData("0:Player:Position:" + Grid.instance.GetAxesFromWorldPoint(player.transform.position)+":");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            agacharse = true;
            animator.SetBool("agacharse", true);
        }

        //LIMITES
        if (extraHealth > numOfextraHearts){ 
            extraHealth = numOfextraHearts;
        }
        if (health > numOfHearts) { 
            health = numOfHearts;
        }
       
        //Manejo de la vida
        if (DamageTaken != 0) 
        {     
            if (extraHealth != 0)
            {
                extraHealth -= DamageTaken;
                DamageTaken = -1 * extraHealth;
            } 
            if (extraHealth < 0)
            {
                extraHealth = 0;
            }
            health -= DamageTaken;
            DamageTaken = 0;
        }
        if (health <= 0)
        {
            if (!ImDead)
            {
                health = 0;
                Client.instance.Send_Data("0:Player:Health:" + health + ":");
            }
        }
        //Enviar datos al server
        if (health != vidaTemp)
        {
            Client.instance.Send_Data("0:Player:Health:" + health + ":");
            vidaTemp = health;
        }

        //Testeo
        if (Input.GetKeyDown(KeyCode.Space))
        {
            DamageTaken += 1;
        }

        if (Input.GetKeyDown(KeyCode.Backspace)) {

            if (health >= 5)    //Esto funciona
            {
                extraHealth++;
            }
            else {  
                health++; 
            }
            Client.instance.Send_Data("0:Player:Health:" + health + ":");
        }

        //Corazones en la interfaz
        /*for (int i = 0; i < extraHearts.Length; i++)
        {
            if (i < extraHealth) {
                extraHearts[i].enabled = true;
            }
            else {
                extraHearts[i].enabled = false;
            }
        }
        for (int i = 0; i < hearts.Length; i++){

            if (i < health){
                hearts[i].sprite = fullHeart;
            } else{
                hearts[i].sprite = emptyHeart;
            }
        }*/

        //MONEDAS Y TESOROS
        monedasText.text = ":" + monedas;

        if ((monedas != monedasTemp)){
            Client.instance.Send_Data("0:Player:Coins:" + monedas + ":");
            monedasTemp = monedas;
        }
        
        
        tesorosMAX = 4; //Depende del nivel
        tesorosText.text = ":" + tesoros + "/" + tesorosMAX;
        if ((tesoros != tesorosTemp))
        {
            Client.instance.Send_Data("0:Player:Treasures:" + tesoros + "/" + tesorosMAX + ":");
            tesorosTemp = tesoros;
        }

        //LLaves
        llavesText.text = ":" + llaves;

    }

    void Movement()
    {
        if (player.isGrounded)
        {
            if (Input.GetKey(KeyCode.W))
            {
                if (animator.GetBool("atacar") || animator.GetBool("defender"))
                {
                    return;
                }
                if (!animator.GetBool("atacar") || !animator.GetBool("defender"))
                {
                    animator.SetBool("correr", true);
                    animator.SetInteger("action", 1);
                    movement = new Vector3(0, 0, 1);
                    movement *= speed * Time.deltaTime;
                    movement = transform.TransformDirection(movement);
                }
            }
            else if (Input.GetKey(KeyCode.S))
            {
                if (animator.GetBool("atacar") || animator.GetBool("defender"))
                {
                    return;
                }
                if (!animator.GetBool("atacar") || !animator.GetBool("defender"))
                {
                    animator.SetBool("correr", true);
                    animator.SetInteger("action", 1);
                    movement = new Vector3(0, 0, -1);
                    movement *= speed * Time.deltaTime;
                    movement = transform.TransformDirection(movement);
                }
            }

        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            animator.SetBool("correr", false);
            animator.SetInteger("action", 0);
            movement = new Vector3(0, 0, 0);
            movement *= speed * Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("correr", false);
            animator.SetInteger("action", 0);
            movement = new Vector3(0, 0, 0);
            movement *= speed * Time.deltaTime;
        }
        rotation += Input.GetAxis("Horizontal") * rotSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, rotation, 0);
        if (!player.isGrounded)
        {
            movement = new Vector3(0, gravity, 0) * Time.deltaTime;
        }

        player.Move(movement);

    }

    void GetInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (animator.GetBool("correr"))
            {
             //   Debug.Log("Ya no voy a correr");
                animator.SetBool("correr", false);
                animator.SetInteger("action", 0);
            }
            if (!animator.GetBool("correr"))
            {
                Attacking();
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            if (animator.GetBool("correr"))
            {
                animator.SetBool("correr", false);
                animator.SetInteger("action", 0);
            }
            if (!animator.GetBool("correr"))
            {
                animator.SetBool("defender", true);
                Debug.Log("Escudo!!!");
                Deffending();
            }
        }
    }

    void Attacking()
    {
        StartCoroutine(AttackRoutine());
    }

    void Deffending()
    {
        StartCoroutine(DeffendRoutine());
    }

    IEnumerator DeffendRoutine()
    {
        animator.SetBool("defender", true);
        animator.SetInteger("action", 3);
        yield return new WaitForSeconds(1);
        animator.SetInteger("action", 0);
        animator.SetBool("defender", false);
    }

    IEnumerator AttackRoutine()
    {
        atacar = true;
        animator.SetBool("atacar", true);
        animator.SetInteger("action", 2);
        yield return new WaitForSeconds(1);
        animator.SetInteger("action", 0);
        animator.SetBool("atacar", false);
        atacar = false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

/***
 * Clase encargada de administrar todos los
 * aspectos requeridos por los espectros
 * de los diferentes niveles
 */
public class SpectrumMovement : MonoBehaviour
{
    //Movimiento
    private Vector3 movement;
    public Vector3 teleportPoint;
    public float horizontal;
    public float vertical;
    public float gravity = -9.8f;
    public float startSpeed = 50;
    public float speed = 50;
    public float followSpeed = 0;
    public int stepPath = 0;
    public bool scared = false;
    public bool perseguir = false;
    public bool goingBack = false;
    public string[] path;
    public string[] patrolPath;
    public Vector3 target;

    //Rango de visión
    public float visionRadius;
    public float visionAngle = 160f;

    //Aspectos generales
    public string tipo;
    public bool teleport = false;
    public bool teleported = false;
    public CharacterController spectrum;
    public float frameInterval;
    public int myId;
    public static bool detected = false;
    public bool localDetected = false;
    public bool attack = false;
    public bool addedToList = false;
    public AudioSource actualSound;
    public AudioClip[] sounds;
    private bool breathing = false;
    private bool sonando = false;
    Animator animator;
    GameObject player;
    Player playerScript;

    // Start is called before the first frame update
    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    void Start()
    {
        spectrum = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        visionRadius = 10;
        myId = Client.spectrumId;
        Client.spectrumId += 1;
        frameInterval = 25+(myId*12)+myId;
        player = GameObject.Find("Damian2.0");
        playerScript = player.GetComponent<Player>();
        actualSound = GetComponent<AudioSource>();
        //movement = Grid.instance.GetWorldPointFromAxes(14, 51);
    }
    /***
     * Método que detecta si entra otro collider, 
     * si entra la espada notifica al server,
     * si entra una rata evita que el espectro 
     * se mueva pues está asustado.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            scared = true;
        }

        if (other.gameObject.CompareTag("Sword"))
        {
            if (Player.atacar)
            {
                if (!checkVisualRange()) //Atacar por la espalda
                {
                    playerScript.monedas += 100;
                    Client.instance.tcp.SendData(myId + ":Spectrum:Damage:1:");
                    Destroy(gameObject);
                }
            }
        }
    }
    /***
     * Verifica cuando un objeto sale del collider
     * en caso de ser la rata regresa el estado
     * de asustado a false y vuelve a caminar.
     */
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Rat"))
        {
            scared = false;
        }
    }

    public void MoveSound()
    {
        if (Time.frameCount % frameInterval != 0)
        {
            actualSound.clip = sounds[0];
            actualSound.Play();
        }
    }

    /***
     * Método predefinido de Unity para dibujar
     * figuras utilizadas para el debugging del juego.
     */
    void OnDrawGizmos()
    {
       /* Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position,visionRadius);*/
    }

    // Update is called once per frame

    /***
     * Método que se ejecuta una vez por frame
     * evaluando constantemente la información 
     * y el estado actual del espectro
     * para su correcto funcionamiento.
     */
    void Update()
    {
        if (!addedToList)
        {
            bool exist = false;
            for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
            {
                if(Client.instance.spectrums.getValorEnIndice(i).myId == this.myId)
                {
                    exist = true;
                }
            }
            if (!exist)
            {
                Client.instance.spectrums.añadirElementos(this);
            }
            Client.instance.tcp.SendData(myId + ":Spectrum:New:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position)+";"+tipo+":");
        }

        float distance = Vector3.Distance(player.transform.position, transform.position); // faster than Vector3.Distance

        if (Time.frameCount % frameInterval == 0)
        {
            if (teleport)
            {
                teleport = false;
                transform.position = transform.position = Vector3.MoveTowards(transform.position, teleportPoint, 200000 * Time.deltaTime);
            }
            if (distance < visionRadius)
            {
                checkVisualRange();
            }
            if(distance < 7 && detected)
            {
                attacking();
            }
            if(detected == true && perseguir == false)
            {
                localDetected = true;
                perseguir = true;
                animator.SetInteger("action", 1);
                animator.SetBool("perseguir", true);
            }
            if(Safe.safe == true && localDetected == true)
            {
                //localDetected = false;
                animator.SetInteger("action",0);
                animator.SetBool("perseguir",false);
                perseguir = false;
                Client.instance.tcp.SendData(myId + ":Spectrum:Backtracking:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
            }
            else if (detected == true)
            {
                if (tipo != "blue")
                {
                    Client.instance.tcp.SendData(myId + ":Spectrum:Detected:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
                }
                if (tipo == "blue" && !teleported)
                {
                    Client.instance.tcp.SendData(myId+":Spectrum:Teleport:"+ Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
                }
                if (tipo == "blue" && teleported)
                {
                    Client.instance.tcp.SendData(myId + ":Spectrum:Detected:" + Grid.instance.GetAxesFromWorldPoint(spectrum.transform.position) + ":");
                }
                speed = followSpeed;
            }  
        }

        if(Time.frameCount % frameInterval == 0 && !sonando)
        {
            if (tipo != "red")
            {
                actualSound.clip = sounds[1];
                actualSound.Play();
            }
        }


        walk();
    }
    /***
     * Método que verifica si el jugador está en un rango de ataque.
     * Calcula un círculo de radio variable dividido por una amplitud
     * de 160 grados que corresponde al frente del espectro.
     * True si está en frente, false si se encuentra detrás.
     * @return bool
     */
    bool checkVisualRange()
    {
        Vector3 direction = player.transform.position - transform.position;

        float angle = Vector3.Angle(direction, transform.forward);

        if (angle < visionAngle * 0.5f){
            if (!Safe.safe)
            {
                detected = true;
                animator.SetInteger("action", 1);
                animator.SetBool("perseguir", true);
                return true;
            }
        }
        return false;
    }
    /***
     * Método encargado de procesar los elementos que tenga en el array
     * de strings que contiene el camino indicado por el servidor
     * se encarga de recorrer esta lista y en caso de llegar al final recorre nuevamente la lista
     * debido a que esta se actualiza constantemente por parte del servidor.
     */
    private void walk()
    {
        if (path.Length > 0)
        {
            try
            {
                if (goingBack)
                {
                    if (stepPath == path.Length - 1)
                    {
                        path = patrolPath;
                        goingBack = false;
                        stepPath = 0;
                    }
                }
                if (stepPath == path.Length - 1)
                {
                    stepPath = 0;
                }
                string[] pos_grid = path[stepPath].Split(',');
                string x = pos_grid[0];
                string y = pos_grid[1];
                int posX = Int32.Parse(x);
                int posZ = Int32.Parse(y);
                
                target = Grid.instance.GetWorldPointFromAxes(posX, posZ);
                
                if (transform.position != target)
                {
                    if (!detected)
                    {
                        FaceTarget();
                    }if (detected)
                    {
                        transform.LookAt(player.transform.position);
                    }
                    if (!scared)
                    {
                        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }

                }if(transform.position == target)
                {
                    stepPath++;
                }
            }
            catch
            {
                Debug.Log("Error convirtiendo string a entero");
            }
        }
    }

    /***
     * Método que calcula la rotación necesaria 
     * para el objeto con el propósito de que mire 
     * al objetivo actual de su ruta.
     */
    void FaceTarget()
    {
        Vector3 direction = (target - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 2f);

    }

    /***
     * Método que es llamado cuando el espectro deberá 
     * moverse en acciones de atacar al jugador.
     */
    void attacking()
    {
        StartCoroutine(AttackRoutine());
    }

    /***
     * Método donde se ejecuta la rutina
     * que corresponde a la acción de 
     * atacar del espectro
     */
    IEnumerator AttackRoutine()
    {
        if(tipo != "red")
        {
            sonando = true;
            actualSound.clip = sounds[2];
            actualSound.Play();
            animator.SetBool("atacar", true);
            animator.SetInteger("action", 2);
            yield return new WaitForSeconds(2);
        }
        if(tipo == "red")
        {
            sonando = true;
            actualSound.clip = sounds[2];
            actualSound.Play();
            animator.SetBool("atacar", true);
            animator.SetInteger("action", 2);
            yield return new WaitForSeconds(5);
            actualSound.clip = sounds[0];
        }
        if (detected)
        {
            animator.SetInteger("action", 1);
            animator.SetBool("atacar", false);
        }
        if (!detected)
        {
            animator.SetInteger("action", 0);
            animator.SetBool("atacar", false);
        }
        sonando = false;
    }
}

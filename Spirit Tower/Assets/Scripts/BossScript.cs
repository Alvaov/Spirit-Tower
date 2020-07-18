using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;
/***
 * Clase que representa y controla el comportamiento del
 * jefe final escogido para el último piso de la torre.
 */
public class BossScript : MonoBehaviour
{
    public string[] movementPath;
    public bool created = false;
    public int stepPath;
    public Vector3 target;
    public float speed;
    public int life;
    public bool active = false;
    public int action;
    public GameObject player;
    public int actualPhase = 0;
    public CharacterController boss;
    public float frameInterval;
    public bool atacar = false;
    public bool cubrir = false;
    Animator animator;
    private bool changed = false;

    public bool attack = false;

    // Start is called before the first frame update
    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    void Start()
    {
        speed = 25;
        stepPath = 0;
        player = GameObject.FindGameObjectWithTag("Player");
        boss = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        frameInterval = 60;
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Sword"))
        {
            Client.instance.tcp.SendData("0:Boss:Damage:1");
        }
    }

    // Update is called once per frame
    /***
     * Método que se ejecuta cada frame
     */
    void Update()
    {
        if (!created)
        {
            Client.instance.tcp.SendData("0:Boss:New::");
        }

        if (active)
        {
            animator.SetBool("active", true);
            transform.LookAt(player.transform.position);
            if(life == 3 && !changed)
            {
                changed = true;
                Client.instance.tcp.SendData("0:Boss:Phase:1:");
                animator.SetInteger("action", 3);
                stepPath = 0;
            }
            if (life == 0)
            {
                animator.SetInteger("action", 6);
            }
            if (!cubrir && !atacar)
            {
                walk();
            }
            if (atacar)
            {
                if(actualPhase == 1)
                {
                    //transform.LookAt(player.transform.position);
                    Vector3 target = player.transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, target, (speed - 5) * 4 * Time.deltaTime);
                    if (transform.position == target)
                    {
                        //StartCoroutine(AttackRoutine());
                        animator.SetInteger("action", 4);
                        cubrir = true;
                        atacar = false;
                    }
                }
                if (actualPhase == 0)
                {
                    Vector3 target = player.transform.position;
                    transform.position = Vector3.MoveTowards(transform.position, target, (speed - 5) * 4 * Time.deltaTime);
                    if (transform.position == target)
                    {
                        //StartCoroutine(AttackRoutine());
                        animator.SetInteger("action", 2);
                        cubrir = true;
                        atacar = false;
                    }
                }
            }
            if (cubrir) {
                string[] pos_grid = movementPath[movementPath.Length-2].Split(',');
                string x = pos_grid[0];
                string y = pos_grid[1];
                int posX = Int32.Parse(x);
                int posZ = Int32.Parse(y);

                Vector3 target = Grid.instance.GetWorldPointFromAxes(posX, posZ);
                target.y = 10;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * 2 * Time.deltaTime);
            }
        }
    }

    /***
     * Método encargado de procesar los elementos que tenga en el array
     * de strings que contiene el camino indicado por el servidor
     * se encarga de recorrer esta lista y en caso de llegar al final recorre nuevamente la lista
     * debido a que esta se actualiza constantemente por parte del servidor.
     */
    private void walk()
    {
        if (movementPath.Length > 0)
        {
            try
            {
                if (stepPath == movementPath.Length - 1)
                {
                    attacking();
                    stopMoving();
                    stepPath = 0;
                }
                string[] pos_grid = movementPath[stepPath].Split(',');
                string x = pos_grid[0];
                string y = pos_grid[1];
                int posX = Int32.Parse(x);
                int posZ = Int32.Parse(y);

                target = Grid.instance.GetWorldPointFromAxes(posX, posZ);
                

                if (transform.position != target)
                {
                    if (actualPhase == 0)
                    {
                        target.y = 16.44f;
                        transform.LookAt(player.transform.position);
                        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
                    }else if(actualPhase == 1)
                    {
                        target.y = 10f;
                        FaceTarget();
                        transform.position = Vector3.MoveTowards(transform.position, target, (speed-10) * Time.deltaTime);
                    }

                }

                if (transform.position == target)
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
     * Método que es llamado cuando el jefe deberá 
     * quedarse quieto dando oportunidad 
     * al jugador de que lo ataque.
     */
    void stopMoving()
    {
        cubrir = true;
        StartCoroutine(StopRoutine());
        cubrir = false;
    }

    /***
     * Método que es llamado cuando el jefe deberá 
     * moverse en acciones de atacar al jugador.
     */
    void attacking()
    {
        atacar = true;
        //StartCoroutine(AttackRoutine());
    }

    /***
     * Método donde se ejecuta la rutina
     * que corresponde a la acción de 
     * atacar del jefe final
     */
    IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(0);
        animator.SetInteger("action", 2);
    }



    /***
     * Método donde se ejecuta la rutina
     * que corresponde a la acción de 
     * detenerse del jefe final
     */
    IEnumerator StopRoutine()
    {
        if(actualPhase == 0)
        {
            animator.SetInteger("action", 1);
            yield return new WaitForSeconds(3);
            animator.SetInteger("action", 7);
            yield return new WaitForSeconds(3);
            animator.SetInteger("action", 0);
            cubrir = false;
        }
        else if(actualPhase == 1)
        {
            animator.SetInteger("action", 3);
            yield return new WaitForSeconds(1);
            animator.SetInteger("action", 7);
            yield return new WaitForSeconds(3);
            animator.SetInteger("action", 3);
            cubrir = false;
        }

    }
}

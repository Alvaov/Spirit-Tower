using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

/***
 * Clase encargada de ser el cliente, comunicación con el server,
 * interpretación de mensajes así como de ejecutar las acciones
 * que indique el servidor por medio de estos mensajes
 */
public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;
    public Lista<SpectrumMovement> spectrums;
    public Lista<RatScript> rats;
    public Lista<Chuchu> chuchus;
    public Lista<EyeScript> spectralEyes;
    public string ip = "127.0.0.1";
    public int port = 54100;
    public int myId = 0;
    public static int spectrumId = 0;
    public static int ratId = 0;
    public static int chuchuId = 0;
    public static int eyeId = 0;
    public TCP tcp;

    GameObject player;
    static Player playerScript;
    static BossScript boss;

    /***
     * Método que se ejecuta apenas se crea el objeto, asigna una instancia
     * en una variable.
     */
    private void Awake()
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
    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    public void Start()
    {
        tcp = new TCP();
        spectrums = new Lista<SpectrumMovement>();
        rats = new Lista<RatScript>();
        player = GameObject.Find("Damian2.0");

        createBoss();
        /*try
        {
            boss = GameObject.Find("DemonGirlMesh").GetComponent<BossScript>();
        }
        catch (Exception e)
        {
            Debug.Log("No se asignó el jefe");
            Debug.Log(e);
        }*/
        playerScript = player.GetComponent<Player>();
        chuchus = new Lista<Chuchu>();
        spectralEyes = new Lista<EyeScript>();
    }

    public void createBoss() {
        try
        {
            boss = GameObject.Find("DemonGirlMesh").GetComponent<BossScript>();
        }
        catch (Exception e)
        {
            Debug.Log("No se asignó el jefe");
            Debug.Log(e);
        }
    }

    /***
     * Método que se conecta al servidor de igual forma que envía 
     * el estado de las paredes al servidor para el reflejo del mapa.
     */
    public void ConnectToServer()
    {
        tcp.Connect();
        Grid.getGridWalls();
    }

    /***
     * Método que envía datos al servidor
     * @parameter string msg
     */
    public void Send_Data(string msg)
    {
        if (instance.tcp != null)
        {
            tcp.SendData(msg);
        }
    }
    /***
     * Clase que comprende el socket TCP que se conecta al servidor.
     */
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

        /***
         * Método que conecta el socket al servidor según el ip
         * y el puerto establecidos.
        */
        public void Connect()
        {
            socket = new TcpClient
            {
                ReceiveBufferSize = dataBufferSize,
                SendBufferSize = dataBufferSize
            };

            receiveBuffer = new byte[dataBufferSize];
            socket.BeginConnect(instance.ip, instance.port, ConnectCallback, socket);
        }

        /***
         * Método que realiza una conexión tipo callback.
         */
        private void ConnectCallback(IAsyncResult _result)
        {
            socket.EndConnect(_result);

            if (!socket.Connected)
            {
                return;
            }

            stream = socket.GetStream();

            stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
        }

        /***
         * Método que envía la información codificada en byte[] al servidor directamente
         * del socket.
         */
        public void SendData(String dataToSend)
        {
            try
            {
                byte[] buffer = Encoding.ASCII.GetBytes(dataToSend);
                stream = socket.GetStream();
                stream.Write(buffer, 0, buffer.Length);

                //Debug.Log("se mando el dato");
            }
            catch (Exception _ex)
            {
                //Debug.Log($"Error sending data to player via TCP: {_ex}");
            }
        }

        /***
         * Método encargado de recibir la información del servidor
         * de manera asicnrónica.
         */
        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                string msg = Encoding.UTF8.GetString(_data, 0, _data.Length);
                handleData(msg);
            }
            catch
            {
                Debug.Log("Error recibiendo dato del servidor");
            }
        }

        /***
         * Método encargado de parsear, interpretar y ejecutar los mensaje
         * que recibe del servidor.
         */
        private void handleData(string msg)
        {
            string[] msg_arr = msg.Split(':');

            /* *** JUGADOR *** */ 
            if (msg_arr[1] == "Player")
            {
                if (msg_arr[2] == "Damage")
                {
                    playerScript.DamageTaken = int.Parse(msg_arr[3]);
                }

                if (msg_arr[2] == "Dead")
                {
                    playerScript.ImDead = true;
                }

            }

            /* *** ESPECTRO *** */ 
            if (msg_arr[1] == "Spectrum")
            {
                if (msg_arr[2] == "Pathfinding")
                {
                    string[] actualPath = msg_arr[3].Split(';');
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        SpectrumMovement espectroActual = Client.instance.spectrums.getValorEnIndice(i);
                        if (espectroActual.myId == int.Parse(msg_arr[0]))
                        {
                            espectroActual.path = actualPath;
                        }
                    }
                }
                if (msg_arr[2] == "Backtracking")
                {
                    Debug.Log(msg);
                    string[] actualPath = msg_arr[3].Split(';');
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        SpectrumMovement espectroActual = Client.instance.spectrums.getValorEnIndice(i);
                        if (espectroActual.myId == int.Parse(msg_arr[0]))
                        {
                            espectroActual.path = actualPath;
                            espectroActual.localDetected = false;
                            espectroActual.teleported = false;
                            espectroActual.goingBack = true;
                            espectroActual.speed = espectroActual.startSpeed;
                        }
                    }
                }
                else if (msg_arr[2] == "Created")
                {
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectrums.getValorEnIndice(i).myId == int.Parse(msg_arr[0]))
                        {
                            SpectrumMovement espectro = Client.instance.spectrums.getValorEnIndice(i);
                            string[] spectrumInfo = msg_arr[3].Split('*');
                            espectro.addedToList = true;
                            espectro.patrolPath = spectrumInfo[0].Split(';');
                            espectro.path = spectrumInfo[0].Split(';');

                            //Asignar genéticos
                            string[] DNA = spectrumInfo[1].Split(',');
                            espectro.followSpeed = float.Parse(DNA[0]) / 10;
                            espectro.startSpeed = float.Parse(DNA[1]) / 10;
                            espectro.speed = float.Parse(DNA[1]) / 10;
                            espectro.visionRadius = float.Parse(DNA[2]) / 10;

                        }
                    }
                }
                else if (msg_arr[2] == "Teleport")
                {
                    Debug.Log(msg);
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectrums.getValorEnIndice(i).myId == int.Parse(msg_arr[0]))
                        {
                            SpectrumMovement espectro = Client.instance.spectrums.getValorEnIndice(i);
                            espectro.teleported = true;
                            string[] target = msg_arr[3].Split(',');
                            string x = target[0];
                            string y = target[1];
                            int posX = Int32.Parse(x);
                            int posY = Int32.Parse(y);
                            Vector3 position = Grid.instance.GetWorldPointFromAxes(posX, posY);
                            espectro.teleportPoint = position;
                            espectro.teleport = true;

                        }
                    }
                }
                else if (msg_arr[2] == "Dead")
                {
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectrums.getValorEnIndice(i).myId == int.Parse(msg_arr[0]))
                        {
                            Client.instance.spectrums.Eliminar(i);
                            Debug.Log("AAAAAAAAAAAAAAAAAAAAAHHHHHHHHHHHHHHH M");
                        }
                    }
                }
            }

            if (msg_arr[1] == "Rat")
            {
                if (msg_arr[2] == "Created")
                {
                    for (int i = 0; i < Client.instance.rats.getTamaño(); i++)
                    {
                        if (Client.instance.rats.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            Client.instance.rats.getValorEnIndice(i).addedToList = true;

                            }
                    }
                }
                if (msg_arr[2] == "Move")
                {
                    for (int i = 0; i < Client.instance.rats.getTamaño(); i++)
                    {
                        if (Client.instance.rats.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            string[] actualPath = { msg_arr[3] };
                            Client.instance.rats.getValorEnIndice(i).path = actualPath;

                            }
                    }
                }
            }
            if (msg_arr[1] == "Chuchu")
            {
                if (msg_arr[2] == "Created")
                {
                    for (int i = 0; i < Client.instance.chuchus.getTamaño(); i++)
                    {
                        if (Client.instance.chuchus.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            Client.instance.chuchus.getValorEnIndice(i).addedToList = true;

                            }
                    }
                }
                if (msg_arr[2] == "Move")
                {
                    for (int i = 0; i < Client.instance.chuchus.getTamaño(); i++)
                    {
                        if (Client.instance.chuchus.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            string[] actualPath = msg_arr[3].Split(';');
                            Client.instance.chuchus.getValorEnIndice(i).path = actualPath;

                            }
                    }
                }

                if (msg_arr[2] == "Dead")
                {
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.chuchus.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            Client.instance.chuchus.Eliminar(i);
                        }
                    }
                }
            }

            if (msg_arr[1] == "Eye")
            {
                if (msg_arr[2] == "Created")
                {
                    for (int i = 0; i < Client.instance.spectralEyes.getTamaño(); i++)
                    {
                        if (Client.instance.spectralEyes.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            Client.instance.spectralEyes.getValorEnIndice(i).addedToList = true;

                        }
                    }
                }

                if (msg_arr[2] == "Dead")
                {
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectralEyes.getValorEnIndice(i).id == int.Parse(msg_arr[0]))
                        {
                            Client.instance.spectralEyes.Eliminar(i);
                        }
                    }

                }
            }
            if(msg_arr[1] == "Boss")
            {
                if (msg_arr[2] == "Created")
                {
                    try
                    {
                        boss.created = true;
                        string[] bossInfo = msg_arr[3].Split('*');
                        boss.life = Int32.Parse(bossInfo[1]);
                        boss.movementPath = bossInfo[0].Split(';');
                    }
                    catch(Exception e)
                    {
                        Debug.Log(e);
                    }
                }
                if (msg_arr[2] == "Damage")
                {
                    boss.life -= 1;
                }
                if (msg_arr[2] == "Phase")
                {
                    boss.movementPath = msg_arr[3].Split(';');
                    boss.actualPhase += 1;
                }
            }
        }
    }
}

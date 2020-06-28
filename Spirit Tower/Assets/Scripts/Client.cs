using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

public class Client : MonoBehaviour
{
    public static Client instance;
    public static int dataBufferSize = 4096;
    public Lista<SpectrumMovement> spectrums;
    public string ip = "127.0.0.1";
    public int port = 54100;
    public int myId = 0;
    public static int spectrumId = 0;
    public TCP tcp;

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

    public void Start()
    {
        tcp = new TCP();
        spectrums = new Lista<SpectrumMovement>();
    }

    public void ConnectToServer()
    {
        tcp.Connect();
        Grid.getGridWalls();
    }
    public void Send_Data(string msg)
    {
        if(instance.tcp != null)
        {
            tcp.SendData(msg);
        }
    }
    public class TCP
    {
        public TcpClient socket;

        private NetworkStream stream;
        private byte[] receiveBuffer;

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
                Debug.Log($"Error sending data to player via TCP: {_ex}");
            }
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    // TODO: disconnect
                    return;
                }

                byte[] _data = new byte[_byteLength];
                Array.Copy(receiveBuffer, _data, _byteLength);

                // TODO: handle data
                stream.BeginRead(receiveBuffer, 0, dataBufferSize, ReceiveCallback, null);
                string msg = Encoding.UTF8.GetString(_data, 0, _data.Length);
                handleData(msg);
            }
            catch
            {
                Debug.Log("Error recibiendo dato del servidor");
            }
        }

        private void handleData(string msg)
        {
            string[] msg_arr = msg.Split(':'); 
            if (msg_arr[1] == "Spectrum")
            {
                if(msg_arr[2] == "Pathfinding")
                {
                    //Buscar al espectro por ID
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectrums.getValorEnIndice(i).myId == int.Parse(msg_arr[0]))
                        {
                            //Debug.Log(msg);
                            Client.instance.spectrums.getValorEnIndice(i).path = msg_arr[3].Split(';');

                        }
                    }
                }
                else if(msg_arr[2]== "Created")
                {
                    for (int i = 0; i < Client.instance.spectrums.getTamaño(); i++)
                    {
                        if (Client.instance.spectrums.getValorEnIndice(i).myId == int.Parse(msg_arr[0]))
                        {
                            Client.instance.spectrums.getValorEnIndice(i).addedToList = true;

                        }
                    }
                }
                
            }

        }
    }

}
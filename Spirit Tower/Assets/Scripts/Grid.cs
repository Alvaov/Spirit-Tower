﻿using UnityEngine;
using System.Collections;
using System.Threading;

public class Grid : MonoBehaviour
{
    public LayerMask unwalkableMask;
    public Vector2 gridWorldSize;
    public float nodeRadius;
    static Node[,] grid;
    float nodeDiameter;
    int gridSizeX, gridSizeY;
    public Transform player;
    public static Grid instance;
    private void Start(){
        instance = this;
        //size
        nodeDiameter = nodeRadius*10;
        Debug.Log(nodeDiameter);
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    void CreateGrid(){
        grid = new Node[gridSizeX, gridSizeY];
        Vector3 worldBottomLeft = transform.position - Vector3.right * gridWorldSize.x / 2 - Vector3.forward * gridWorldSize.y / 2;

        for(int x= 0; x < gridSizeX; x++){
            for (int y = 0; y < gridSizeY; y++){
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x * nodeDiameter + nodeRadius) + Vector3.forward * (y * nodeDiameter + nodeRadius);
                bool walkable = !(Physics.CheckSphere(worldPoint, nodeDiameter, unwalkableMask)); // true->collision walkable-> false
                grid[x, y] = new Node(walkable, worldPoint,x,y);
            }
        }
    }
    //Convert real world position into exact grid location
    public string GetAxesFromWorldPoint(Vector3 worldPosition){
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; //z because the axis
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX) * percentX);
        int y = Mathf.RoundToInt((gridSizeY) * percentY);
        return x + "," + y; //return the position in grid
        
    }

    public Vector3 GetWorldPointFromAxes(int x, int y)
    {
        int horizontal = (29/3)*x-301;
        int vertical = 10*y - 315;
        Vector3 movement = new Vector3(horizontal, 0f, vertical) ;
        Debug.Log(movement);
        return movement; //return the position in world

    }

    public Node GetNodeFromWorldPoint(Vector3 worldPosition)
    {
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; //z because the axis
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX) * percentX);
        int y = Mathf.RoundToInt((gridSizeY) * percentY);
        return grid[x,y]; //return the position in grid

    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(gridWorldSize.x, 1, gridWorldSize.y));
        if(grid!= null)
        {
            Node playerNode = GetNodeFromWorldPoint(player.position); //get the location on grid
            foreach(Node n in grid)
            {
                if (n.walkable) {
                    Gizmos.color = Color.white;
                } else {
                    Gizmos.color = Color.red;
                }
                if(playerNode == n)
                {
                    Gizmos.color = Color.cyan;
                }
                Gizmos.DrawCube(n.worldPosition, Vector3.one * (nodeDiameter - .1f));
            }
        }
    }

    public static void getGridWalls()
    {
        foreach (Node n in grid)
        {
            if (n.walkable == false)
            {
                Client.instance.tcp.SendData("0:Grid:Obstacle:" + n.x + "," + n.y + ":");
                Thread.Sleep(5);
            }
        }
    }
    
}

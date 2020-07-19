using UnityEngine;
using System.Collections;
using System.Threading;

/***
 * Clase encargada del manejo completo del grid necesario
 * para gran mayoría de funcionalidades del juego
 */
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

    /***
     * Método que se ejecuta en el primer frame y 
     * se encarga de inicializar las variables 
     * necesarias para el correcto funcionamiento.
     */
    private void Start(){
        instance = this;
        //size
        nodeDiameter = nodeRadius*5;
        gridSizeX = Mathf.RoundToInt(gridWorldSize.x / nodeDiameter);
        gridSizeY = Mathf.RoundToInt(gridWorldSize.y / nodeDiameter);
        CreateGrid();

    }

    /***
     * Crea el grid basado en el tamaño del mapa y el tamaño del grid predefinido
     */
    public void CreateGrid(){
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

    /***
     * Convierte un punto del mundo 3D a un punto de dos dimensiones
     * @return string
     */
    public string GetAxesFromWorldPoint(Vector3 worldPosition){
        float percentX = (worldPosition.x + gridWorldSize.x / 2) / gridWorldSize.x;
        float percentY = (worldPosition.z + gridWorldSize.y / 2) / gridWorldSize.y; //z because the axis
        percentX = Mathf.Clamp01(percentX);
        percentY = Mathf.Clamp01(percentY);

        int x = Mathf.RoundToInt((gridSizeX) * percentX);
        int y = Mathf.RoundToInt((gridSizeY) * percentY);
        return x + "," + y; //return the position in grid
        
    }

    /***
     * Convierte un punto del mundo 2D a un punto de tres dimensiones
     * @return string
     */
    public Vector3 GetWorldPointFromAxes(int x, int y)
    {
        float horizontal = 5*x-299.3f;
        float vertical = 5*y-298.9f;
        Vector3 movement = new Vector3(horizontal, 0f, vertical) ;
        return movement; //return the position in world

    }

    /***
     * Convierte un punto del mundo 2D a un punto de tres dimensiones
     * @return Node
     */
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


    /***
     * Se encarga de obtener los nodos que son obstáculos 
     * y los envía al server para obtener un reflejo del mapa
     * del lado del servidor
     */
    public static void getGridWalls()
    {   
        Thread.Sleep(20);
        Client.instance.tcp.SendData("0:Grid:New::");
        Thread.Sleep(1600);
        foreach (Node n in grid)
        {
            if (n.walkable == false)
            {
                Client.instance.tcp.SendData("0:Grid:Obstacle:" + n.x + "," + n.y + ":");
                Thread.Sleep(10);
            }
        }
    }
    
}

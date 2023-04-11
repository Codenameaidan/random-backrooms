using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadGuy : MonoBehaviour
{
    private GameObject player;
    public float timeToMoveOneBlock = 0.5f;
    public GameObject MARKER_TEMP;

    enum Status{
        MOVING,
        CHASING,
        NOTHING,
    }

    Status status = Status.NOTHING;

    private const int MAX_SEARCH_ITERATIONS = 25000;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(GoTo(Functions.convertToGridPos(new Vector3(player.transform.position.x, 1, player.transform.position.z))));
    }

    // Update is called once per frame
    void Update()
    {
        int damping = 20;
        var target = player.transform;
        
        var lookPos = target.position - transform.position;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping); 

        //Search Mode
        
        //Chase mode 1
        //var step =  1 * Time.deltaTime; // calculate distance to move
        //transform.position = Vector3.MoveTowards(transform.position, target.position, step);

    }

    private IEnumerator GoTo(Vector3Int target) //Modified A* pathfinding
    {
        //queue = queue of PATHS
        Queue<List<Vector3Int>> queue = new Queue<List<Vector3Int>>();

        List<Vector3Int> path = new List<Vector3Int>();
        path.Add(Functions.convertToGridPos(new Vector3(this.transform.position.x, 1, this.transform.position.z)));//new Vector3Int((int)this.gameObject.transform.position.x, (int)this.gameObject.transform.position.y));

        queue.Enqueue(path);
        
        //Hashset of which coordinates have been visited
        HashSet<Vector3Int> visited = new HashSet<Vector3Int>();
        //visited.Add(target);

        int count = 0;

        bool found = false;

        while(queue.Count != 0){
            count++;
        
            //If not found within maxSearchIterations, just stop looking.
            if(count > MAX_SEARCH_ITERATIONS){
                Debug.Log("MAX ITERATIONS REACHED");
                break;
            }

            //Path we're looking into
            path = queue.Dequeue();//[queue.Count-1];

            Vector3Int last = path[path.Count-1];

            if(last == null)
                break;

            visited.Add(last);



            if(last.x == target.x && last.z == target.z){
                //Debug.Log("FOUND PATH");
                found = true;
                break;
            }

            List<Vector3Int> neighbors = Functions.getNeighbors(last);// Maze.maze.getRoadNeighbors(last);

            

            if(neighbors.Contains(target)){//map.targetBorders(last,target)){
                found = true;
                path.Add(target);
                break;
            }

            foreach(Vector3Int road in neighbors){
                //GameObject.Instantiate(MARKER_TEMP, Functions.convertToWorldPos(road), new Quaternion(0,0,0,0));
                if(road != null && !visited.Contains(road)){
                    List<Vector3Int> newpath = new List<Vector3Int>(path);
                    newpath.Add(road);
                    queue.Enqueue(newpath);
                }
            }
           
        }

        //Debug.Log("Iterations:"+count);

        if(found){
            this.GetComponent<SpriteRenderer>().enabled = true;
            int prevX = (int)this.gameObject.transform.position.x;
            int prevZ = (int)this.gameObject.transform.position.z;

            foreach(Vector3Int road in path)
            {
                MoveTo(Functions.convertToWorldPos(road));

                prevX = road.x;
                prevZ = road.z;

                yield return new WaitForSeconds(timeToMoveOneBlock);  
            }
            //Arrive();
        }else{
            Debug.Log("Car spawned at ("+(int)this.gameObject.transform.position.x+", "+(int)this.gameObject.transform.position.y + ") was unable to find path to ("+target.x+", "+target.y+") !");
            Destroy(this.gameObject);
        }        
    }

    private void MoveTo(Vector3 coordinate)
    {
        iTween.MoveTo(this.gameObject, iTween.Hash("x", coordinate.x , "y", 1, "z", coordinate.z, "time", timeToMoveOneBlock, "easetype", iTween.EaseType.linear));
    }
}

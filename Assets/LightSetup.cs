using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSetup : MonoBehaviour
{
    public GameObject light;

    public int sizeX = 150;
    public int sizeZ = 150;
    public int spacing = 15;
    public float height = 1.75f;

    public float minDistanceForLights = 10;

    private Transform player;
    private List<GameObject> lights = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart(0.25f));
        player = GameObject.FindGameObjectWithTag("Player").transform;
        
    }


     IEnumerator LateStart(float waitTime)
     {
         yield return new WaitForSeconds(waitTime); 
            //Start late because we need to wait for the maze to generate.
            //TODO Eventually, will just integrate this class into MazeCreation to avoid this bodge...
         PlaceLights();
     }


     void PlaceLights(){
        //Place lights in a grid (every 3) as long as they do not overlap with any walls
        for(int x = 0; x<sizeX; x+=spacing){
            for(int z = 0; z<sizeZ; z+=spacing){

                Vector3 pos = new Vector3((float)x,height,z);

                Collider[] c = Physics.OverlapSphere(pos,0.5f);
                bool skip = false;
                foreach(Collider collider in c){
                    if(collider.gameObject.tag == "Wall"){
                        
                        skip = true;
                        break;
                    }
                }
                if(skip)  continue;

                GameObject g = GameObject.Instantiate(
                    light,
                    pos,
                    Quaternion.Euler(90,0,0)
                );

                lights.Add(g);
            }
        }
     }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject light in lights){ //Check all lights and only enable those within minDistance of player.
            if(Vector3.Distance(light.transform.position, player.position) < minDistanceForLights ){
                light.SetActive(true);
            }else{
                light.SetActive(false);

            }
        }
    }
}

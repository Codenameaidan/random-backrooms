//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze : MonoBehaviour
{
    public int sizeX;
    public int sizeY;

    public GameObject[] objects;
    public static bool[,] maze;
    private GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        //Maze generation
        maze = new bool[sizeX, sizeY];

        List<int[]> frontiers = new List<int[]>();

        int x = Random.Range(0,sizeX);
        int y = Random.Range(0,sizeY);
        frontiers.Add(new int[] { x, y, x, y });

        while (frontiers.Count != 0)
        {
            int index = Random.Range(0,frontiers.Count);
            int[] f = frontiers[index];
            frontiers.RemoveAt(index);

            x = f[2];
            y = f[3];
            if (maze[x, y] == false)
            {
                maze[f[0], f[1]] = maze[x, y] = true;
                if (x >= 2 && maze[x - 2, y] == false)
                    frontiers.Add(new int[] { x - 1, y, x - 2, y });
                if (y >= 2 && maze[x, y - 2] == false)
                    frontiers.Add(new int[] { x, y - 1, x, y - 2 });
                if (x < sizeX - 2 && maze[x + 2, y] == false)
                    frontiers.Add(new int[] { x + 1, y, x + 2, y });
                if (y < sizeY - 2 && maze[x, y + 2] == false)
                    frontiers.Add(new int[] { x, y + 1, x, y + 2 });
            }
        }
        
        //Randomly place 2x2 holes in the maze (if we get overlaps, who cares?)
        int numRandomHoles = Random.Range(30,50);
        for(int i = 0; i < numRandomHoles; i++){
            int randX = Random.Range(3,sizeX-3);
            int randZ = Random.Range(3,sizeY-3);

            maze[randX, randZ] = true;
            maze[randX+1, randZ] = true;
            maze[randX, randZ+1] = true;
            maze[randX+1, randZ+1] = true;
        }


        this.Instantiate();


        //Place the player in a sensible place (where there is no maze wall) 
        int randPlayerPosX;
        int randPlayerPosZ;
        do{
            randPlayerPosX = Random.Range(15,sizeX-15);
            randPlayerPosZ = Random.Range(15,sizeY-15);
        }while(maze[randPlayerPosX, randPlayerPosZ] == false);
        player.transform.position = Functions.convertToWorldPos(new Vector3Int(randPlayerPosX, 1, randPlayerPosZ));
    }

    //Create maze in world space
    void Instantiate(){
        for(int x = 0; x < maze.GetLength(0); x++){
         for(int z = 0; z < maze.GetLength(1); z++){

            bool val = maze[x,z];
            if(val)
                continue;

            GameObject.Instantiate(
                objects[0],
                Functions.convertToWorldPos(new Vector3Int(x,1,z)),
                new Quaternion(0,0,0,0)
            );
         }   
        }
    }
}


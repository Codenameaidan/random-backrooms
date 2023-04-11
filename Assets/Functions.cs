using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Functions
{
    public static Vector3 convertToWorldPos(Vector3Int vec)
    {
        return new Vector3( 
            vec.x * 3, 
            vec.y, 
            vec.z * 3
        );
    }

    public static Vector3Int convertToGridPos(Vector3 vec){
        return new Vector3Int( 
                    (int) ((vec.x + 1.5) / 3),
                    1, 
                    (int) ((vec.z + 1.5) / 3)
        );
    }

    public static List<Vector3Int> getNeighbors(Vector3Int vec){ //Todo move this to Maze...
        List<Vector3Int> foundSpaces = new List<Vector3Int>();

        if(vec.x >= 1 && Maze.maze[vec.x-1, vec.z]){
            foundSpaces.Add(new Vector3Int(vec.x-1, 1, vec.z));
        }
        if(vec.z >= 1 && Maze.maze[vec.x, vec.z-1]){
            foundSpaces.Add(new Vector3Int(vec.x, 1, vec.z-1));
        }
        if(vec.x < Maze.maze.GetLength(0) -1  && Maze.maze[vec.x+1, vec.z]){
            foundSpaces.Add(new Vector3Int(vec.x+1, 1, vec.z));
        }
        if(vec.z < Maze.maze.GetLength(1) - 1  && Maze.maze[vec.x, vec.z+1]){
            foundSpaces.Add(new Vector3Int(vec.x, 1, vec.z+1));
        }

        return foundSpaces;
    }
}

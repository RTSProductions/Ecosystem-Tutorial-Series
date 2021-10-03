using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    //The size of the map
    [Range(10, 100)]
    public float range;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Draw gizmos
    private void OnDrawGizmos()
    {
        //Set the color to blue
        Gizmos.color = Color.blue;

        //Draw a cube
        Gizmos.DrawCube(Vector3.zero, new Vector3(range, range, range));
    }

    //A mothed to get a random position on the map
    public Vector3 getRandomPosition()
    {
        //Create a random position
        Vector3 position = new Vector3(Random.Range(-range, range), 5, Random.Range(-range, range));

        //Check if its above the ground
        if (!Physics.Raycast(position, Vector3.down, 10))
        {
            //Get a new random position
            position = getRandomPosition();
        }

        //Return the random position
        return position;
    }
}

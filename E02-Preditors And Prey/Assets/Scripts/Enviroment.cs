using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [Range(10, 1000)]
    public float range;
    //Get an array of all the animal populations
    public InitalPopulations[] initalPopulations;

    public LayerMask animalLayer;

    // Start is called before the first frame update
    void Start()
    {
        //Loop through each animal population
        foreach(InitalPopulations pop in initalPopulations)
        {
            //Spawn one animal for each of the count
            for (int i = 0; i < pop.count; i++)
            {
                SpawnAnimal(pop.prefab);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;

        Gizmos.DrawSphere(transform.position, range);
    }

    public Vector3 getRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-range, range), 5, Random.Range(-range, range));

        if (!Physics.Raycast(position, Vector3.down, 10))
        {
            position = getRandomPosition();
        }

        return position;
    }

    //Get a run position away from the preditor
    public Vector3 getRunAwayPosition(Transform target)
    {
        //Set the inital quota
        Vector3 position = Vector3.zero;

        //Set the inital quota
        float currentDistance = 1;

        //Loop 20 times
        for (int i = 0; i < 20; i++)
        {
            //Get a random position
            Vector3 tryPosition = getRandomPosition();

            //Calculate the distance
            float dist = Vector3.Distance(tryPosition, target.position);

            //Check if its greater than the current distance
            if (dist > currentDistance)
            {
                //Set the position to our new position
                position = tryPosition;
                //Set the distance to our new distance
                currentDistance = dist;
            }
        }

        return position;
    }

    public void SpawnAnimal(GameObject prefab)
    {
        Vector3 spawnPoint = getRandomPosition();

        spawnPoint = new Vector3(spawnPoint.x, prefab.transform.position.y, spawnPoint.z);

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}

//Population class
[System.Serializable]
public class InitalPopulations
{
    //Name of the array element
    public string name;
    //Animal prefab
    public GameObject prefab;
    //Inital population count
    public int count;
}

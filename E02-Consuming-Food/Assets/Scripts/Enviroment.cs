using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [Range(10, 1000)]
    public float range;

    //The animal prefab
    public GameObject animal;

    //The food prefab
    public GameObject food;

    //The animal count
    public int animalCount;

    //The food count
    public int foodCount;

    // Start is called before the first frame update
    void Start()
    {
        //Loop over the food count
        for (int i = 0; i < foodCount; i++)
        {
            //Spawn food
            SpawnAnimal(food);
        }
        //Loop over the animal count
        for (int i = 0; i < animalCount; i++)
        {
            //Spawn animal
            SpawnAnimal(animal);
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

    //Spanw animal method
    public void SpawnAnimal(GameObject prefab)
    {
        //Spawn point
        Vector3 spawnPoint = getRandomPosition();

        //Editing the spawn point to avoid floating animals
        spawnPoint = new Vector3(spawnPoint.x, prefab.transform.position.y, spawnPoint.z);

        //Spawning the animal
        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}

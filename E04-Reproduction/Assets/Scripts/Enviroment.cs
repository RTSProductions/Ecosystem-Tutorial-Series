using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enviroment : MonoBehaviour
{
    [Range(10, 1000)]
    public float range;
    public InitalPopulations[] initalPopulations;

    public LayerMask animalLayer;

    // Start is called before the first frame update
    void Start()
    {
        foreach(InitalPopulations pop in initalPopulations)
        {
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

    public Vector3 getRunAwayPosition(Transform target)
    {
        Vector3 position = Vector3.zero;

        float currentDistance = 1;

        for (int i = 0; i < 20; i++)
        {
            Vector3 tryPosition = getRandomPosition();

            float dist = Vector3.Distance(tryPosition, target.position);

            if (dist > currentDistance)
            {
                position = tryPosition;
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

[System.Serializable]
public class InitalPopulations
{
    public string name;
    public GameObject prefab;
    public int count;
}

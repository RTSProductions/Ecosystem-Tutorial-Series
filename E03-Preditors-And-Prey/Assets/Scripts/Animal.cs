using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //Made this private
    Vector3 targetPosition;

    public Species species;

    public Species diet;

    public float hunger;

    bool foundIntrest = false;

    bool rangeOne = false;

    //Check if there is any danger
    bool danger = false;

    //Made this private
    float timeToDeathByHunger = 200;

    public float speed = 10;

    public float visionRadius = 30;

    Enviroment enviroment;

    // Start is called before the first frame update
    void Start()
    {
        enviroment = FindObjectOfType<Enviroment>();

        targetPosition = enviroment.getRandomPosition();

        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();

        trigger.radius = visionRadius;
        trigger.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        rangeOne = dist <= 1;

        if (dist <= 1)
        {
            if (!foundIntrest)
            {
                targetPosition = enviroment.getRandomPosition();
                //Set danger to false
                danger = false;
            }
        }

        float hungerTime = speed / 10;

        hunger += Time.deltaTime * hungerTime / timeToDeathByHunger;

        if (hunger >= 1)
        {
            GetComponent<LiveEntity>().Die(CauseOfDeath.hunger);
        }

        Move(targetPosition);
    }

    void Move(Vector3 targetPosition)
    {
        Vector3 target = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        transform.LookAt(target);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            if (entity.species == diet)
            {
                targetPosition = other.transform.position;
                foundIntrest = true;

                if (rangeOne)
                {
                    hunger = 0;
                    entity.Die(CauseOfDeath.beingEaten);
                    foundIntrest = false;
                }
            }
            //Check if there's a preditor
            if (entity.species != Species.redCube && entity.gameObject.GetComponent<Animal>().diet == species && !danger)
            {
                //Run away
                targetPosition = enviroment.getRunAwayPosition(entity.transform);

                //We've noticed danger
                danger = true;
            }
        }
        else
        {
            if (foundIntrest == true)
            {
                foundIntrest = false;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Vector3 targetPosition;

    public Species species;

    public Species diet;

    //The animal's hunger
    public float hunger;

    //Checking if the animal has found their intrest
    bool foundIntrest = false;

    //Checking if they are close to the intrest
    bool rangeOne = false;

    //The time till the animal's death by hunger
    public float timeToDeathByHunger = 200;

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

        //Checking the distance
        rangeOne = dist <= 1;

        if (dist <= 1)
        {
            if (!foundIntrest)
            {
                targetPosition = enviroment.getRandomPosition();
            }
        }

        //Getting the hunger time based on the speed of the animal
        float hungerTime = speed / 10;

        //Increasing the hunger
        hunger += Time.deltaTime * hungerTime / timeToDeathByHunger;

        //Checking if they are too hungry
        if (hunger >= 1)
        {
            //Killing the animal
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
                //Setting fount intrest to true
                foundIntrest = true;

                //Checking if they are within eating range
                if (rangeOne)
                {
                    //Setting the hunger to 0
                    hunger = 0;
                    //Killing the animal
                    entity.Die(CauseOfDeath.beingEaten);
                    //Setting fount intrest to false
                    foundIntrest = false;
                }
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

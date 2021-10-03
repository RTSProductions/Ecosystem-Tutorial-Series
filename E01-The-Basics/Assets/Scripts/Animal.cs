using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    //The position the animal is moving to
    public Vector3 targetPosition;

    //The species of the animal
    public Species species;

    //The diet of the animal
    public Species diet;

    //The speed of the animal
    public float speed = 10;

    //The vision radius of the animal
    public float visionRadius = 30;

    //The enviroment
    Enviroment enviroment;

    // Start is called before the first frame update
    void Start()
    {
        //Get the enviroment
        enviroment = FindObjectOfType<Enviroment>();

        //Set the target position to a random position
        targetPosition = enviroment.getRandomPosition();

        //Add a trigger to the animal
        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();

        //Set the radius of the trigger to the vision radius
        trigger.radius = visionRadius;
        //Set it to a trigger
        trigger.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Check the distance between the animal and it's target position
        float dist = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        //Check if the distance is less than or equal to 1
        if (dist <= 1)
        {
            //Set the target position to a random position
            targetPosition = enviroment.getRandomPosition();
        }

        //Move the animal
        Move(targetPosition);
    }

    //Move method
    void Move(Vector3 targetPosition)
    {
        //Create a new target position so the animal doesn't move on the y axis
        Vector3 target = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        //Move to the target position
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        //Look at the target position
        transform.LookAt(target);
    }

    //Check if anything enters the vision radius 
    private void OnTriggerEnter(Collider other)
    {
        //Check if they are a Live Entity
        if (other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            //Check if we can eat them
            if (entity.species == diet)
            {
                //Set the target position to the position of the animal
                targetPosition = other.transform.position;
            }
        }
    }

    //Check if something is still in the vision radius 
    private void OnTriggerStay(Collider other)
    {
        //Check if they are a Live Entity
        if (other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            //Check if we can eat them
            if (entity.species == diet)
            {
                //Set the target position to the position of the animal
                targetPosition = other.transform.position;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    // Initalize the target position
    Vector3 targetPosition = new Vector3(0, 0, 0);

    public Species species;

    public Species diet;

    public float hunger;

    bool foundIntrest = false;

    bool danger = false;

    float timeToDeathByHunger = 200;

    public float speed = 10;

    // Gender of the animal
    public bool male;

    // The amount of how much the animal values reproducing over other things
    public float reprocuctiveUrge = 0.3f;

    public float visionRadius = 30;

    // The amount of time before the animal can reproduce again
    float childCoolDown = 10;

    // The time before which an animal can not have another child
    public float timeToNextChild;

    Enviroment enviroment;

    // Start is called before the first frame update
    void Start()
    {
        enviroment = FindObjectOfType<Enviroment>();

        // Assign a random value to the gender
        male = Random.value < 0.5f;

        targetPosition = enviroment.getRandomPosition();

        SphereCollider trigger = gameObject.AddComponent<SphereCollider>();

        trigger.radius = visionRadius;
        trigger.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

        if (dist <= 1) // Use distance instead of 'rangeOne'
        {
            if (!foundIntrest)
            {
                targetPosition = enviroment.getRandomPosition();
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
            // Check to make sure that the other species is the same as ours
            if (entity.species == species)
            {
                if (reprocuctiveUrge > hunger) // Make sure reproduction is needed
                {
                    // Get the animal script of the species
                    Animal ani = entity.GetComponent<Animal>();

                    // Make sure reproduction is possible
                    if (ani.reprocuctiveUrge > ani.hunger && Time.time >= timeToNextChild && Time.time >= ani.timeToNextChild)
                    {
                        // Set the target position
                        targetPosition = ani.transform.position;

                        // Check the distance again
                        float dist = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));

                        // Make sure that they are close enough to reproduce
                        if (dist <= 1)
                        {
                            // Set reproduction cool down
                            timeToNextChild = Time.time + childCoolDown;

                            // Make sure the animal giving birth is female
                            if (!male)
                            {
                                // Clone the animal
                                var child = Instantiate(this.gameObject);

                                // Get the animal script
                                Animal childAnimal = child.GetComponent<Animal>();

                                // Enable the animal script
                                childAnimal.enabled = true;

                                // Enable the live entity script
                                child.GetComponent<LiveEntity>().enabled = true;

                                // Randomize the gender
                                childAnimal.male = Random.value < 0.5f;

                                // Reset the hunger of the child
                                childAnimal.hunger = 0;
                            }
                            
                            // Print a log to the console for debuging
                            Debug.Log("New clones!");
                        }
                    }
                }
            }    
            if (entity.species == diet)
            {
                targetPosition = other.transform.position;
                foundIntrest = true;

                float dist = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z)); // Get the distance between the animal and it's target position

                if (dist <= 1) // Use the distance instead of rangeOne
                {
                    hunger = 0;
                    entity.Die(CauseOfDeath.beingEaten);
                    foundIntrest = false;
                }
            }
            if (entity.species != Species.redCube && entity.gameObject.GetComponent<Animal>().diet == species && !danger)
            {
                //targetPosition = enviroment.getRunAwayPosition(entity.transform);

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

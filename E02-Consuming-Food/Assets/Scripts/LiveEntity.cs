using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiveEntity : MonoBehaviour
{
    public Species species;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Die method
    public void Die(CauseOfDeath cause)
    {
        Destroy(gameObject);

        Debug.Log(gameObject.name + "'s cause of death was '" + cause + "'");
    }
}

//Cause of death enum
public enum CauseOfDeath
{
    hunger, beingEaten,
}

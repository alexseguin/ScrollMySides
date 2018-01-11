using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : MonoBehaviour {

    public Transform dest; // drag the destination object here var sound: AudioClip; // define a teleport sound, if you want
    public Transform boulder;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Boulder")
        {
            
            var newObject = Instantiate(boulder, dest.position, dest.rotation);
            // move the player and align it to the dest object: 
            // if some sound defined, play it at the destination: 

        }
    }
}
